using System;
using System.Configuration;

namespace EPS.Web.Configuration
{
    /// <summary>   A configuration section that allows us to define settings as they apply to mobile devices. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class MobileConfigurationSection : ConfigurationSection
    {
        /// <summary> Full configuration path of the MobileConfigurationSection </summary>
        public const string ConfigurationPath = "eps.web/mobile";

        /// <summary>   Gets the name of the cookie that can be used to override mobile or standard view. </summary>
        /// <value> The cookie name. </value>
        [ConfigurationProperty("overrideCookie", IsRequired = true)]
        public string OverrideCookie
        {
            get { return (string)this["overrideCookie"]; }
        }
    }
}
