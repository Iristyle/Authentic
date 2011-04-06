using System;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using EPS.Text;
using EPS.Web.Abstractions;
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
    public class AuthenticationInspectingAuthenticator : 
        HttpContextInspectingAuthenticatorBase<IAuthenticationHeaderInspectorConfigurationElement>
    {
        PrivateHashEncoder privateHashEncoder;
        /// <summary>   Initializes a new instance of the AuthenticationInspectingAuthenticator class. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        public AuthenticationInspectingAuthenticator(IAuthenticationHeaderInspectorConfigurationElement config) 
            : base(config) 
        {
            privateHashEncoder = new PrivateHashEncoder(config.PrivateKey);
        }
        
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
                DigestHeader digestHeader;
                if (!HttpDigestAuthHeaderParser.TryExtractDigestHeader(context.Request.HttpMethod.ToEnumFromEnumValue<HttpMethodNames>(), authHeader, out digestHeader))
                {
                    //don't log an event since there was nothing to succeed / fail
                    return new InspectorAuthenticationResult(false, null, "No digest credentials found in HTTP header");
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
                        return new InspectorAuthenticationResult(true, principal, string.Empty);
                    }
                }

                new AuthenticationFailureEvent(this, digestHeader.UserName).Raise();
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
            //this allows us to accept digest auth credentials, but return custom principal objects
            //i.e. digest username /password -> MyPrincipal!
            IPrincipalBuilder principalBuilder = Configuration.GetPrincipalBuilder();
            if (null != principalBuilder)
                return principalBuilder.ConstructPrincipal(context, username, password);

            //otherwise, use our generic identities / principals create principal and set Context.User
            GenericIdentity id = new GenericIdentity(username, "CustomDigest");
            if (!string.IsNullOrEmpty(Configuration.RoleProviderName))
                return new FixedProviderRolePrincipal(RoleProviderHelper.GetProviderByName(Configuration.RoleProviderName), id);

            return new GenericPrincipal(id, null);
        }
    }
}