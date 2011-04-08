using System;
using System.Collections.Generic;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Interface for http context inspecting authentication module section. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IModuleConfiguration
    {
        /// <summary>   Gets or sets a value indicating whether the authentication module is enabled. </summary>
        /// <value> true if enabled, false if not. </value>
        bool Enabled { get; set; }
        
        /// <summary>   Gets the failure handler instance. </summary>
        /// <value> The failure handler. </value>
        IFailureHandler FailureHandler { get; }

        /// <summary>   Gets the collection of defined inspectors. </summary>
        /// <value> The inspectors. </value>
        IEnumerable<IAuthenticator> Inspectors { get; }
    }
}