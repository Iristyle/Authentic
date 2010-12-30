using System;
using System.Configuration;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   Basic authentication failure handler configuration section. </summary>
    /// <remarks>   ebrown, 12/30/2010. </remarks>
    public class BasicAuthenticationFailureHandlerConfigurationSection : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        /// <summary>   Gets or sets the realm of the cookie on an outgoing cookie request. </summary>
        /// <value> The realm. </value>
        [ConfigurationProperty("realm", DefaultValue = "localhost")]
        [StringValidator(MinLength = 1)]
        public string Realm
        {
            get { return (string)this["realm"]; }
            set { base["realm"] = value; }
        }
    }
}
