using System;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   A class that supports code-based configuration. </summary>
    /// <remarks>   ebrown, 4/8/2011. </remarks>
    public class AuthenticatorConfiguration :
            IAuthenticatorConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the HttpContextInspectingAuthenticatorConfiguration class.
        /// </summary>
        public AuthenticatorConfiguration(string name, IAuthenticator authenticator, IPrincipalBuilder principalBuilder)
        {
            //TODO: 4-8-2011 -- create a FleutnValidator that matches up with this and call it here
            Name = name;
            Authenticator = authenticator;
            PrincipalBuilder = principalBuilder;
        }

        /// <summary>   Gets or sets the name of the role provider used to validate the principal. </summary>
        /// <value> The name of the role provider. </value>
        public string RoleProviderName { get; set; }

        /// <summary>   Gets or sets the human-friendly name / key for this inspector. </summary>
        /// <value> The name. </value>
        public string Name { get; set; }

        /// <summary>   Gets or sets a value indicating whether the require SSL. </summary>
        /// <value> true if require SSL, false if not. </value>
        public bool RequireSsl { get; set; }

        /// <summary>   
        /// Get or sets the name of the MembershipProvider to be used.  By default no membership provider is used as it may be just as costly as
        /// extracting a principal. If a simpler membership provider exists that can provide a faster validation of user credentials than a full
        /// IPrincipal extraction, then it makes sense to use a membershipProvider.  Specify 'default' to use the default configured
        /// MembershipProvider for the system. 
        /// </summary>
        /// <value> The name of the provider. </value>
        public string ProviderName { get; set; }
        
        /// <summary>   Gets or sets the authenticator instance. </summary>
        /// <value> The authenticator. </value>
        public IAuthenticator Authenticator { get; private set; }

        /// <summary>   Gets or sets the principal builder instance. </summary>
        /// <value> The builder instance. </value>
        public IPrincipalBuilder PrincipalBuilder { get; private set; }
    }
}
