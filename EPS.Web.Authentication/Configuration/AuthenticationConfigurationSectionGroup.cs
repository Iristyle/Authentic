using System;
using System.Configuration;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   The configuration section group that defines how the authentication system works. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticationConfigurationSectionGroup : ConfigurationSectionGroup
    {                
        /// <summary>   Gets the configuration section responsible for configuring the header inspector. </summary>
        /// <value> The header inspector. </value>
        [ConfigurationProperty("httpHeaderAuthentication", IsRequired = false)]
        public HttpContextInspectingAuthenticationModuleSection HeaderInspector
        {
            get { return (HttpContextInspectingAuthenticationModuleSection)Sections["httpHeaderAuthentication"]; }
        }

        /*

        [ConfigurationProperty("openIdAuthentication", IsRequired = false)]
        public OpenId.Configuration.AuthenticationConfigurationSection OpenIdAuthentication
        {
            get { return (OpenId.Configuration.AuthenticationConfigurationSection)Sections["openIdAuthentication"]; }
        }

        [ConfigurationProperty("dna", IsRequired = false)]
        public DNAConfigurationSection DNA
        {
            get { return (DNAConfigurationSection)Sections["dna"]; }
        }
        */
    }
}
