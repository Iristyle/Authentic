using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
    /// <summary>   Interface for basic authentication header inspector configuration element. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IAuthenticationHeaderInspectorConfigurationElement :
            IHttpContextInspectingAuthenticatorConfigurationElement
    { }
}
