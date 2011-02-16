using System;

namespace EPS.Web.Configuration
{
    /// <summary>   The interface for a configuration section that allows us to define settings as they apply to mobile devices. </summary>
    /// <remarks>   ebrown, 2/10/2011. </remarks>
    public interface IMobileConfiguration
    {
        /// <summary>   Gets the name of the cookie that can be used to override mobile or standard view. </summary>
        /// <value> The cookie name. </value>
        string OverrideCookie { get; }
    }
}