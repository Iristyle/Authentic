using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   
    /// Basic authentication header inspector configuration element that defines which MembershipProvider to use for authentication and which
    /// PrincipalBuilderFactory to use (class that implements <see cref="T:
    /// EPS.Web.Abstractions.IPrincipalBuilderFactory"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticationHeaderInspectorConfigurationElement : 
        HttpContextInspectingAuthenticatorConfigurationElement, 
        IAuthenticationHeaderInspectorConfigurationElement
    { }
}
