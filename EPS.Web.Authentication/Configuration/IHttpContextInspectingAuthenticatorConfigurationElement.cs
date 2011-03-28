using System;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Interface for http context inspecting authenticator configuration element. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IHttpContextInspectingAuthenticatorConfigurationElement
    {
        /// <summary>   Gets or sets the name of the role provider used to validate the principal. </summary>
        /// <value> The name of the role provider. </value>
        string RoleProviderName { get; set; }
        /// <summary>   Gets or sets the human-friendly name / key for this inspector. </summary>
        /// <value> The name. </value>
        string Name { get; set; }
        /// <summary>   Gets or sets the FullName of the factory class Type. </summary>
        /// <value> The factory. </value>
        string Factory { get; set; }
        /// <summary>   Gets or sets a value indicating whether the require SSL. </summary>
        /// <value> true if require SSL, false if not. </value>
        bool RequireSsl { get; set; }
        /// <summary>   Gets the name of the custom configuration section that will be loaded and passed on to clients of this class. </summary>
        /// <value> The name of the custom configuration section. </value>
        string CustomConfigurationSectionName { get; }
    }
}
