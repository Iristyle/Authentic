using System;
using System.Collections.Generic;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Interface for http context inspecting authentication module section. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IHttpContextInspectingAuthenticationModuleSection
    {
        /// <summary>   Gets or sets a value indicating whether the authentication module is enabled. </summary>
        /// <value> true if enabled, false if not. </value>
        bool Enabled { get; set; }
        /// <summary>   Gets or sets the name of the failure handler factory. </summary>
        /// <value> The name of the failure handler factory. </value>
        string FailureHandlerFactoryName { get; set; }
        /// <summary>   Gets the name of the custom failure handler configuration section. </summary>
        /// <value> The name of the custom failure handler configuration section. </value>
        string CustomFailureHandlerConfigurationSectionName { get; }
        /// <summary>   Gets the custom failure handler configuration section. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ConfigurationErrorsException"> Thrown when there are configuration errors. </exception>
        /// <returns>   The custom failure handler configuration section. </returns>
        IHttpContextInspectingAuthenticationFailureConfigurationSection GetCustomFailureHandlerConfigurationSection();
        /// <summary>   Gets the collection of defined inspectors. </summary>
        /// <value> The inspectors. </value>
        IDictionary<string, IHttpContextInspectingAuthenticatorConfigurationElement> Inspectors { get; }
    }
}
