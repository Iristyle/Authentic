using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EPS.Configuration;

namespace EPS.Web.Configuration
{
    /// <summary>   Collection of RoutingRedirectConfigurationElements read from config. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class RoutingRedirectConfigurationElementCollection : 
        ConfigurationElementCollection<string, RoutingRedirectConfigurationElement>
    {
        /// <summary>   Uses SourceUrl as a key. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="element">  The element. </param>
        /// <returns>   The element key (SourceUrl). </returns>
        public override string GetElementKey(RoutingRedirectConfigurationElement element)
        {
            if (null == element) { throw new ArgumentNullException("element"); }

            return element.SourceUrl;
        }

        /// <summary>   Gets a mapping of source url to target url as defined in config. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <returns>   The url map. </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a candidate for a property since it performs a Linq query / Dictionary construction")]
        public Dictionary<string, string> GetUrlMap()
        {
            return this.OfType<RoutingRedirectConfigurationElement>().ToDictionary(r => r.SourceUrl, r => r.TargetUrl);
        }
    }
}
