using System;
using System.Configuration;

namespace EPS.Web.Authentication.Configuration
{
    public class AuthenticationConfigurationSectionGroup : ConfigurationSectionGroup
    {                
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

        [ConfigurationProperty("onyxAuthentication", IsRequired = false)]
        public Onyx.Configuration.AuthenticationConfigurationSection OnyxAuthentication
        {
            get { return (Onyx.Configuration.AuthenticationConfigurationSection)Sections["onyxAuthentication"]; }
        }

        [ConfigurationProperty("corporateAuthentication", IsRequired = false)]
        public Corporate.Configuration.AuthenticationConfigurationSection CorporateAuthentication
        {
            get { return (Corporate.Configuration.AuthenticationConfigurationSection)Sections["corporateAuthentication"]; }
        }

        [ConfigurationProperty("dna", IsRequired = false)]
        public DNAConfigurationSection DNA
        {
            get { return (DNAConfigurationSection)Sections["dna"]; }
        }
        */
    }
}
