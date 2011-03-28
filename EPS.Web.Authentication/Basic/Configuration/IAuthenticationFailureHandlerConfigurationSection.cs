using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   Interface for authentication failure handler configuration section. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IAuthenticationFailureHandlerConfigurationSection : 
        IHttpContextInspectingAuthenticationFailureConfigurationSection
    {
        /// <summary>   Gets or sets the realm of the cookie on an outgoing cookie request. </summary>
        /// <value> The realm. </value>
        string Realm { get; set; }
    }
}
