using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   Interface for authentication failure handler configuration section for basic authentication. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IBasicFailureHandlerConfiguration : 
        IFailureHandlerConfiguration
    {
        /// <summary>   Gets or sets the realm of the cookie on an outgoing cookie request. </summary>
        /// <value> The realm. </value>
        string Realm { get; set; }
    }
}
