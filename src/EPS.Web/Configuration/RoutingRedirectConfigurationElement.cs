using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Configuration
{
    /// <summary>   Routing redirect configuration element that provides a source to target mapping. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class RoutingRedirectConfigurationElement : ConfigurationElement, IRoutingRedirectConfigurationElement
    {
        /// <summary>   Gets or sets the source URL. </summary>
        /// <value> The source url. </value>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~"), 
        ConfigurationProperty("sourceUrl", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired, DefaultValue = "")]
        public string SourceUrl
        {
            get { return (string)this["sourceUrl"]; }
            set { this["sourceUrl"] = value; }
        }

        /// <summary>   Gets or sets the target URL. </summary>
        /// <value> The target url. </value>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~"), 
        ConfigurationProperty("targetUrl", IsRequired = true, DefaultValue = "")]
        public string TargetUrl
        {
            get { return (string)this["targetUrl"]; }
            set { this["targetUrl"] = value; }
        }
    }
}