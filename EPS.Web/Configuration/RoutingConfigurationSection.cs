using System;
using System.Configuration;

namespace EPS.Web.Configuration
{
    /// <summary>   A configuration section that defines global routing settings. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class RoutingConfigurationSection : ConfigurationSection, IRoutingConfigurationSection
    {
        /// <summary> Default path in the configuration file </summary>
        public static readonly string ConfigurationPath = "eps.web/routing";

        /// <summary>   Gets the permanent redirects of source to target URL. </summary>
        /// <value> The permanent redirects. </value>
        [ConfigurationProperty("permanentRedirects", IsRequired = true)]
        [ConfigurationCollection(typeof(RoutingRedirectConfigurationElementCollection))]
        public RoutingRedirectConfigurationElementCollection PermanentRedirects
        {
            get { return (RoutingRedirectConfigurationElementCollection)base["permanentRedirects"]; }
        }

        /// <summary>   Gets a value indicating whether permanent redirects are enabled. </summary>
        /// <value> true if enabled, false if not. </value>
        [ConfigurationProperty("enabled", DefaultValue = true, IsRequired = false)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
        }

        //sadly, PostDeserialize wouldn't be called until this type was touched, so we cannot autoRegister through PostDeserialize as it will
        //be too late in the application lifecycle        
    }
}
