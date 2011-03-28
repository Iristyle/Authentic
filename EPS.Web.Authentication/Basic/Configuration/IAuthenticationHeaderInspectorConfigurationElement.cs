using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   Interface for basic authentication header inspector configuration element. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IAuthenticationHeaderInspectorConfigurationElement :
            IHttpContextInspectingAuthenticatorConfigurationElement
    {
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

        /// <summary>   
        /// Gets the principal builder factory instance implementing <see cref="T:EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilderFactory" />, 
        /// and uses that to create instances of <see cref="T:EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilder" /> given the specified configuration. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <returns>   The principal builder instance or null if the PrincipalBuilderFactory property is not properly configured. </returns>
        IBasicAuthPrincipalBuilder GetPrincipalBuilder();
    }
}
