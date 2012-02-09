using System;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Interface for http context inspecting authentication failure configuration section. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IFailureHandlerConfiguration
    {
        /// <summary>   Gets or sets a value indicating whether the require SSL. </summary>
        /// <value> true if require SSL, false if not. </value>
        bool RequireSsl { get; set; }
    }
}