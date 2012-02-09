using System;
using System.Collections.Generic;

namespace EPS.Web.Configuration
{
    /// <summary>   The interface for the routing configuration section. </summary>
    /// <remarks>   ebrown, 2/9/2011. </remarks>
    public interface IRoutingConfiguration
    {
        /// <summary>   Gets the permanent redirects of source to target URL. </summary>
        /// <value> The permanent redirects. </value>
        IDictionary<string, RoutingRedirectConfigurationElement> PermanentRedirects { get; }
        
        /// <summary>   Gets a value indicating whether permanent redirects are enabled. </summary>
        /// <value> true if enabled, false if not. </value>
        bool Enabled { get; }
    }
}
