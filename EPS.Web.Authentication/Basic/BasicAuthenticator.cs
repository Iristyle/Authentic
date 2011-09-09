using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;
using EPS.Web.Authentication.Security;
using EPS.Web.Authentication.Utility;
using EPS.Web.Management;

namespace EPS.Web.Authentication.Basic
{
	/// <summary>   
	/// Basic authentication inspecting authenticator.  Inspects HTTP 'Authorization' headers and generates IPrincipals based on the
	/// configured MembershipProvider. 
	/// </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class BasicAuthenticator :
		AuthenticatorBase<IBasicAuthenticatorConfiguration>
	{
		/// <summary>   Initializes a new instance of the BasicAuthenticator class. </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		public BasicAuthenticator(IBasicAuthenticatorConfiguration config)
			: base(config) { }

		/// <summary>   Authenticates a HttpContextBase given a specified MembershipProvider. </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <exception cref="Exception">                Thrown when an unexpected exception occurs. </exception>
		/// <param name="context">  The context. </param>
		/// <returns>   A success or failure if the MembershipProvider validated the credentials found in the header. </returns>
		public override AuthenticationResult Authenticate(HttpContextBase context)
		{
			try
			{
				if (null == context) { throw new ArgumentNullException("context"); }

				string authHeader = context.Request.Headers["Authorization"];

				Log.InfoFormat(CultureInfo.InvariantCulture, "Authorization header [{0}] received", authHeader.IfMissing("**Missing**"));

				////attempt to extract credentials from header
				NetworkCredential credentials;
				if (!HttpBasicAuthHeaderParser.TryExtractCredentialsFromHeader(authHeader, out credentials))
				{
					//don't log an event since there was nothing to succeed / fail
					return new AuthenticationResult(false, null, "No basic credentials found in HTTP header");
				}

				var membershipProvider = MembershipProviderLocator.GetProvider(Configuration.ProviderName);
				MembershipUser membershipUser = null;
				if (null != membershipProvider && membershipProvider.ValidateUser(credentials.UserName, credentials.Password))
				{
					membershipUser = membershipProvider.GetUser(credentials.UserName, true);
				}

				//either we don't need to validate, or the user specified a validator
				if (null == membershipProvider || null != membershipUser)
				{
					if (null != membershipProvider)
					{
						new AuthenticationSuccessEvent(this, credentials.UserName).Raise();
					}

					IPrincipal principal = GetPrincipal(context, membershipUser, credentials);
					IIdentity identity = null != principal ? principal.Identity : null;
					if (null != identity && identity.IsAuthenticated)
					{
						new AuthenticationSuccessEvent(this, identity.Name).Raise();
						return new AuthenticationResult(true, principal, string.Empty);
					}
				}

				new AuthenticationFailureEvent(this, credentials.UserName).Raise();
				return new AuthenticationResult(false, null, string.Empty);
			}
			catch (Exception)
			{
				new AuthenticationFailureEvent(this, string.Empty).Raise();
				throw;
			}
		}

		private IPrincipal GetPrincipal(HttpContextBase context, MembershipUser membershipUser, NetworkCredential credential)
		{
			//if configuration specifies a plug-in principal builder, then use what's specified
			//this allows use to accept basic auth credentials, but return custom principal objects
			//i.e. username /password -> custom principal!
			if (null != Configuration.PrincipalBuilder)
				return Configuration.PrincipalBuilder.ConstructPrincipal(context, membershipUser, credential);

			//otherwise, use our generic identities / principals create principal and set Context.User
			GenericIdentity id = new GenericIdentity(credential.UserName, "CustomBasic");
			if (!string.IsNullOrEmpty(Configuration.RoleProviderName))
				return new FixedProviderRolePrincipal(RoleProviderHelper.GetProviderByName(Configuration.RoleProviderName), id);

			return new GenericPrincipal(id, null);
		}
	}
}