using System;
using System.Globalization;
using System.Net;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;
using EPS.Web.Authentication.Security;
using EPS.Web.Authentication.Utility;
using EPS.Web.Management;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   
    /// Basic authentication inspecting authenticator.  Inspects HTTP 'Authorization' headers and generates IPrincipals based on the configured
    /// MembershipProvider. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticationInspectingAuthenticator : 
        HttpContextInspectingAuthenticatorBase<IAuthenticationHeaderInspectorConfigurationElement>
    {
        /// <summary>   Initializes a new instance of the AuthenticationInspectingAuthenticator class. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        public AuthenticationInspectingAuthenticator(IAuthenticationHeaderInspectorConfigurationElement config) 
            : base(config) {}
        
        /// <summary>   Authenticates a HttpContextBase given a specified MembershipProvider. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <exception cref="Exception">                Thrown when an unexpected exception occurs. </exception>
        /// <param name="context">  The context. </param>
        /// <returns>   A success or failure if the MembershipProvider validated the credentials found in the header. </returns>
        public override InspectorAuthenticationResult Authenticate(HttpContextBase context)
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
                    return new InspectorAuthenticationResult(false, null, "No basic credentials found in HTTP header");
                }

                var membershipProvider = MembershipProviderLocator.GetProvider(Configuration.ProviderName);

                //either we don't need to validate, or the user specified a validator
                if (null == membershipProvider || membershipProvider.ValidateUser(credentials.UserName, credentials.Password))
                {
                    if (null != membershipProvider)
                    {
                        new AuthenticationSuccessEvent(this, credentials.UserName).Raise();
                    }

                    IPrincipal principal = GetPrincipal(context, credentials.UserName, credentials.Password);
                    IIdentity identity = null != principal ? principal.Identity : null;
                    if (null != identity && identity.IsAuthenticated)
                    {
                        new AuthenticationSuccessEvent(this, identity.Name).Raise();
                        return new InspectorAuthenticationResult(true, principal, string.Empty);
                    }
                }

                new AuthenticationFailureEvent(this, credentials.UserName).Raise();
                return new InspectorAuthenticationResult(false, null, string.Empty);
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
            //this allows use to accept basic auth credentials, but return custom principal objects
            //i.e. MKM username /password -> OnyxPrincipal!
            IBasicAuthPrincipalBuilder principalBuilder = Configuration.GetPrincipalBuilder();
            if (null != principalBuilder)
                return principalBuilder.ConstructPrincipal(context, username, password);

            //otherwise, use our generic identities / principals create principal and set Context.User
            GenericIdentity id = new GenericIdentity(username, "CustomBasic");
            if (!string.IsNullOrEmpty(Configuration.RoleProviderName))
                return new FixedProviderRolePrincipal(RoleProviderHelper.GetProviderByName(Configuration.RoleProviderName), id);

            return new GenericPrincipal(id, null);
        }
    }
}