using System;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Interface for redirect on authentication failure handler configuration section. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IRedirectFailureHandlerConfiguration :
        IFailureHandlerConfiguration
    {
        /// <summary>   Gets or sets URI for the redirect on a failed request. </summary>
        /// <value> The redirect uri. </value>
        Uri RedirectUri { get; set; }
    }
}
