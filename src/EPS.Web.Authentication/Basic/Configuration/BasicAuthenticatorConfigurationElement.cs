using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   
    /// Basic authentication header inspector configuration element that uses the MembershipProvider for authentication and builds Principals
    /// with the specified PrincipalBuilderFactory  (class that implements <see cref="T: EPS.Web.Abstractions.IPrincipalBuilderFactory"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class BasicAuthenticatorConfigurationElement : 
        AuthenticatorConfigurationElement, 
        IBasicAuthenticatorConfiguration
    { }
}
