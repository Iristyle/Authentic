using System;
using System.Configuration;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Configuration
{
    public class RedirectOnAuthenticationFailureHandlerConfigurationSection : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        [ConfigurationProperty("redirectUri", IsRequired = true)]
        public Uri RedirectUri
        {
            get { return (Uri)this["redirectUri"]; }
            set { base["redirectUri"] = value; }
        }
    }
}
