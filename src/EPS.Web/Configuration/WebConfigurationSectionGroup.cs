using System;
using System.Configuration;

namespace EPS.Web.Configuration
{
    /// <summary>   A ConfigurationSectionGroup that maintains all settings applicable to the EPS.Web assembly. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class WebConfigurationSectionGroup : ConfigurationSectionGroup
    {
        /// <summary> Full path of the configuration section</summary>
        public static readonly string ConfigurationPath = "eps.web";

        /// <summary>   Gets the MobileConfigurationSection. </summary>
        /// <value> The nested MobileConfigurationSection. </value>
        [ConfigurationProperty("mobile", IsRequired = false)]
        public MobileConfigurationSection Mobile
        {
            get { return (MobileConfigurationSection)Sections["mobile"]; }
        }

        /// <summary>   Gets the RoutingConfigurationSection. </summary>
        /// <value> The nested RoutingConfigurationSection. </value>
        [ConfigurationProperty("routing", IsRequired = false)]
        public RoutingConfigurationSection Routing
        {
            get { return (RoutingConfigurationSection)Sections["routing"]; }
        }

        /// <summary>   Gets the FileHttpHandlerConfigurationSection. </summary>
        /// <value> The nested FileHttpHandlerConfigurationSection. </value>
        [ConfigurationProperty("fileHttpHandler", IsRequired = false)]
        public FileHttpHandlerConfigurationSection FileHttpHandler
        {
            get { return (FileHttpHandlerConfigurationSection)Sections["fileHttpHandler"]; }
        }
    }
}
