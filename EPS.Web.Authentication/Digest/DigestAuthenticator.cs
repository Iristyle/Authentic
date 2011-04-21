using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using EPS.Text;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;
using EPS.Web.Authentication.Security;
using EPS.Web.Authentication.Utility;
using EPS.Web.Management;

namespace EPS.Web.Authentication.Digest
{
	/// <summary>   
	/// Digest authentication inspecting authenticator.  Inspects HTTP 'Authorization' headers and generates IPrincipals based on the configured
	/// MembershipProvider. 
	/// </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class DigestAuthenticator :
		AuthenticatorBase<IDigestAuthenticatorConfiguration>
	{
		PrivateHashEncoder privateHashEncoder;
		/// <summary>   Initializes a new instance of the DigestAuthenticator class. </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		public DigestAuthenticator(IDigestAuthenticatorConfiguration config)
			: base(config)
		{
			//TODO: 4-6-2011 -- find a better way to hook up the validation logic here so that it matches up with the config class
			if (null == config) { throw new ArgumentNullException("config"); }
			if (null == config.PrivateKey) { throw new ArgumentNullException("config", "IDigestAuthenticatorConfiguration.PrivateKey is null"); }
			if (null == config.Realm) { throw new ArgumentNullException("config", "IDigestAuthenticatorConfiguration.Realm is null"); }
			if (string.IsNullOrWhiteSpace(config.PrivateKey)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.PrivateKey must not be whitespace", "config"); }
			if (config.PrivateKey.Length < 8) { throw new ArgumentException("IDigestAuthenticatorConfiguration.PrivateKey must be at least 8 characters", "config"); }
			if (string.IsNullOrWhiteSpace(config.Realm)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.Realm must not be whitespace", "config"); }
			if (config.NonceValidDuration < TimeSpan.FromSeconds(20)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.NonceValidDuration must be at least 20 seconds", "config"); }
			if (config.NonceValidDuration > TimeSpan.FromMinutes(60)) { throw new ArgumentException("IDigestAuthenticatorConfiguration.NonceValidDuration must be less than 60 minutes", "config"); }

			privateHashEncoder = new PrivateHashEncoder(config.PrivateKey);
		}

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
				DigestHeader digestHeader;
				if (!HttpDigestAuthHeaderParser.TryExtractDigestHeader(context.Request.HttpMethod.ToEnumFromEnumValue<HttpMethodNames>(), authHeader, out digestHeader))
				{
					//don't log an event since there was nothing to succeed / fail
					return new AuthenticationResult(false, null, "No digest credentials found in HTTP header");
				}

				var membershipProvider = MembershipProviderLocator.GetProvider(Configuration.ProviderName);
				if (null == membershipProvider)
				{
					throw new ArgumentException("MembershipProvider specified in configuration cannot be found, but is required to lookup user password for comparison");
				}

				string userPassword = membershipProvider.GetUser(digestHeader.UserName, false).GetPassword();

				//three things validate this digest request -- that the nonce matches the given address, that its not stale
				//and that the credentials match the given realm / opaque / password
				if (NonceManager.Validate(digestHeader.Nonce, context.Request.UserHostAddress, privateHashEncoder) &&
					!NonceManager.IsStale(digestHeader.Nonce, Configuration.NonceValidDuration) &&
					digestHeader.MatchesCredentials(Configuration.Realm, Opaque.Current(), userPassword))
				{
					if (null != membershipProvider)
					{
						new AuthenticationSuccessEvent(this, digestHeader.UserName).Raise();
					}

					IPrincipal principal = GetPrincipal(context, digestHeader.UserName, userPassword);
					IIdentity identity = null != principal ? principal.Identity : null;
					if (null != identity && identity.IsAuthenticated)
					{
						new AuthenticationSuccessEvent(this, identity.Name).Raise();
						return new AuthenticationResult(true, principal, string.Empty);
					}
				}

				new AuthenticationFailureEvent(this, digestHeader.UserName).Raise();
				return new AuthenticationResult(false, null, string.Empty);
			}
			catch (Exception)
			{
				new AuthenticationFailureEvent(this, string.Empty).Raise();
				throw;
			}
		}

		private IPrincipal GetPrincipal(HttpContextBase context, string username, string password)
		{
			//if configuration specifies a plug-in principal builder, then use what's specified
			//this allows us to accept digest auth credentials, but return custom principal objects
			//i.e. digest username /password -> MyPrincipal!
			if (null != Configuration.PrincipalBuilder)
				return Configuration.PrincipalBuilder.ConstructPrincipal(context, username, password);

			//otherwise, use our generic identities / principals create principal and set Context.User
			GenericIdentity id = new GenericIdentity(username, "CustomDigest");
			if (!string.IsNullOrEmpty(Configuration.RoleProviderName))
				return new FixedProviderRolePrincipal(RoleProviderHelper.GetProviderByName(Configuration.RoleProviderName), id);

			return new GenericPrincipal(id, null);
		}
	}
}