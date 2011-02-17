using System;
using EPS.Configuration;

namespace EPS.Web.Configuration
{
    /// <summary>   Collection of RoutingRedirectConfigurationElements read from config. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class RoutingRedirectConfigurationElementDictionary : 
        ConfigurationElementDictionary<string, RoutingRedirectConfigurationElement>        
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
    }
}
