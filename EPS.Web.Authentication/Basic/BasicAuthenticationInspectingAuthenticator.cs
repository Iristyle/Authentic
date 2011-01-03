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
using EPS.Web.Management;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   
    /// Basic authentication inspecting authenticator.  Inspects HTTP 'Authorization' headers and generates IPrincipals based on the configured
    /// MembershipProvider. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class BasicAuthenticationInspectingAuthenticator : 
        HttpContextInspectingAuthenticatorBase<BasicAuthenticationHeaderInspectorConfigurationElement>
    {
        /// <summary>   Constructs an instance of a basic authenticator. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        public BasicAuthenticationInspectingAuthenticator(BasicAuthenticationHeaderInspectorConfigurationElement config) 
            : base(config) {}
        
        /// <summary>   Authenticates a HttpContextBase given a specified MembershipProvider. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="context">  The context. </param>
        /// <returns>   A success or failure if the MembershipProvider validated the credentials found in the header. </returns>
        public override InspectorAuthenticationResult Authenticate(HttpContextBase context)
        {            
            string authHeader = context.Request.Headers["Authorization"];
            
            Log.InfoFormat(CultureInfo.InvariantCulture, "Authorization header [{0}] received", authHeader.IfMissing("**Missing**"));

            try
            {
                //attempt to extract credentials from header
                NetworkCredential credentials;
                if (!HttpBasicAuthHeaderParser.TryExtractCredentialsFromHeader(authHeader, out credentials))
                    //don't log an event since there was nothing to succeed / fail
                    return new InspectorAuthenticationResult(false, null, "No basic credentials found in HTTP header");

                var membershipProvider = GetMembershipProvider();

                //either we don't need to validate, or the user specified a validator
                if (null == membershipProvider || membershipProvider.ValidateUser(credentials.UserName, credentials.Password))
                {
                    if (null != membershipProvider)
                        new AuthenticationSuccessEvent(this, credentials.UserName).Raise();

                    IPrincipal principal = GetPrincipal(context, credentials.UserName, credentials.Password);
                    if (null != principal && null != principal.Identity && principal.Identity.IsAuthenticated)
                    {
                        new AuthenticationSuccessEvent(this, principal.Identity.Name).Raise();
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

        /// <summary>   Gets the membership provider defined in configuration, or null if not specified. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentOutOfRangeException">  Thrown when one or more arguments are outside the required range. </exception>
        /// <returns>   The membership provider that is used to validate incoming credentials. </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "This is not suitable for a property as configuration is inspected and exceptions may be thrown")]
        public MembershipProvider GetMembershipProvider()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ProviderName))
            {
                return null;
            }

            if (string.Equals(Configuration.ProviderName, "default", StringComparison.OrdinalIgnoreCase))
            {
                MembershipProvider currentProvider = Membership.Provider;
                Log.InfoFormat(CultureInfo.InvariantCulture, "Default provider of [{0}] selected", (null != currentProvider ? currentProvider.Name.IfMissing("N/A") : "N/A"));
                return currentProvider;
            }
            else
            {
                MembershipProvider provider = Membership.Providers[Configuration.ProviderName];
                if (provider == null)
                    throw new ArgumentOutOfRangeException(String.Format(CultureInfo.InvariantCulture, "Provider {0} specified in configuration not found", Configuration.ProviderName));
                Log.InfoFormat(CultureInfo.InvariantCulture, "Custom provider of [{0}] specified in configuration selected", provider.Name.IfMissing("*No Name*"));
                return provider;
            }
        }
    }
}
