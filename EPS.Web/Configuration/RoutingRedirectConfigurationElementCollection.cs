using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EPS.Configuration;

namespace EPS.Web.Configuration
{
    /// <summary>   Collection of RoutingRedirectConfigurationElements read from config. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class RoutingRedirectConfigurationElementCollection : ConfigurationElementCollection<string, RoutingRedirectConfigurationElement>
    {
        /// <summary>   Uses SourceUrl as a key. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="element">  The element. </param>
        /// <returns>   The element key (SourceUrl). </returns>
        public override string GetElementKey(RoutingRedirectConfigurationElement element)
        {
            return element.SourceUrl;
        }

        /// <summary>   Gets a mapping of source url to target url as defined in config. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <returns>   The url map. </returns>
        public Dictionary<string, string> GetUrlMap()
        {
            return this.OfType<RoutingRedirectConfigurationElement>().ToDictionary(r => r.SourceUrl, r => r.TargetUrl);
        }
    }
}
