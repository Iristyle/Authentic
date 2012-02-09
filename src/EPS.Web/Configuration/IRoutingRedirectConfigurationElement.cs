using System;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Configuration
{
    /// <summary>   Interface that specifies a routing redirect configuration providing a source to target mapping. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public interface IRoutingRedirectConfigurationElement
    {
        /// <summary>   Gets or sets the source URL. </summary>
        /// <value> The source url. </value>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~")]
        string SourceUrl { get; set; }

        /// <summary>   Gets or sets the target URL. </summary>
        /// <value> The target url. </value>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~")]
        string TargetUrl { get; set; }
    }
}
