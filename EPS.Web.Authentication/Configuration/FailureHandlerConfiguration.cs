using System;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Failure handler configuration for use in code configured scenarios. </summary>
    /// <remarks>   ebrown, 4/8/2011. </remarks>
    public class FailureHandlerConfiguration : 
        IFailureHandlerConfiguration
    {
        /// <summary>   Gets or sets a value indicating whether the require SSL. </summary>
        /// <value> true if require SSL, false if not. Default is false. </value>
        public bool RequireSsl { get; set; }
    }
}
