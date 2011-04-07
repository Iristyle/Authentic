using System;
using EPS.Web.Abstractions;

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
        string CustomConfigurationSectionName { get; set; }

        /// <summary>   
        /// Get or sets the name of the MembershipProvider to be used.  By default no membership provider is used as it may be just as costly as
        /// extracting a principal. If a simpler membership provider exists that can provide a faster validation of user credentials than a full
        /// IPrincipal extraction, then it makes sense to use a membershipProvider.  Specify 'default' to use the default configured
        /// MembershipProvider for the system. 
        /// </summary>
        /// <value> The name of the provider. </value>
        string ProviderName { get; set; }
        /// <summary>   Gets or sets the principal builder factory Type name. </summary>
        /// <value> 
        /// The FullName for the type of the principal builder factory -- i.e. the class that implements <see cref="T:EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilderFactory" />. 
        /// </value>
        string PrincipalBuilderFactory { get; set; }
    }
}
