using System;
using System.Configuration;
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
    public class BasicAuthenticationInspectingAuthenticator : HttpContextInspectingAuthenticatorBase<BasicAuthenticationHeaderInspectorConfigurationElement>
    {
        internal BasicAuthenticationInspectingAuthenticator(BasicAuthenticationHeaderInspectorConfigurationElement config) 
            : base(config) {}

        public override InspectorAuthenticationResult Authenticate(HttpContextBase context)
        {            
            string authHeader = context.Request.Headers["Authorization"];

            log.InfoFormat("Authorization header [{0}] received", 
                authHeader.IfMissing("**Missing**"));

            try
            {
                //attempt to extract credentials from header
                NetworkCredential credentials;
                if (!HttpBasicAuthHeaderParser.TryExtractCredentialsFromHeader(authHeader, out credentials))
                    //don't log an event since there was nothing to succeed / fail
                    return new InspectorAuthenticationResult() { Success = false, Principal = null };

                //either we don't need to validate, or the user specified a validator
                if (null == MembershipProvider || MembershipProvider.ValidateUser(credentials.UserName, credentials.Password))
                {
                    if (null != MembershipProvider)
                        new AuthenticationSuccessEvent(this, credentials.UserName).Raise();

                    IPrincipal principal = GetPrincipal(context, credentials.UserName, credentials.Password);
                    if (null != principal && null != principal.Identity && principal.Identity.IsAuthenticated)
                    {
                        new AuthenticationSuccessEvent(this, principal.Identity.Name).Raise();
                        return new InspectorAuthenticationResult() { Success = true, Principal = principal };
                    }
                }

                new AuthenticationFailureEvent(this, credentials.UserName).Raise();
                return new InspectorAuthenticationResult() { Success = false, Principal = null };
            }
            catch (Exception)
            {
                new AuthenticationFailureEvent(this, string.Empty).Raise();
                return null;
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

        public MembershipProvider MembershipProvider
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Configuration.ProviderName))
                    return null;

                if (string.Equals(Configuration.ProviderName, "default", StringComparison.OrdinalIgnoreCase))
                {
                    MembershipProvider currentProvider = Membership.Provider;
                    log.InfoFormat("Default provider of [{0}] selected", (null != currentProvider ? currentProvider.Name.IfMissing("N/A") : "N/A"));
                    return currentProvider;
                }
                else
                {
                    MembershipProvider provider = Membership.Providers[Configuration.ProviderName];
                    if (provider == null)
                        throw new ConfigurationErrorsException(String.Format("Provider {0} not found", Configuration.ProviderName));

                    log.InfoFormat("Custom provider of [{0}] selected", provider.Name.IfMissing("*No Name*"));
                    return provider;
                }
            }
        }
    }
}
