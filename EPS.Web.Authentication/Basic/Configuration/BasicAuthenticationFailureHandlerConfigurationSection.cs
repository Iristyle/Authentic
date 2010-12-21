using System;
using System.Configuration;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    public class BasicAuthenticationFailureHandlerConfigurationSection : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        [ConfigurationProperty("realm", DefaultValue = "localhost")]
        [StringValidator(MinLength = 1)]
        public string Realm
        {
            get { return (string)this["realm"]; }
            set { base["realm"] = value; }
        }
    }
}
