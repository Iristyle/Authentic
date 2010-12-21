using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    public interface IHttpContextInspectingAuthenticationFailureHandler
    {
        IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults);
        HttpContextInspectingAuthenticationFailureConfigurationSection Configuration { get; }
    }

    public interface IHttpContextInspectingAuthenticationFailureHandler<T>
        : IHttpContextInspectingAuthenticationFailureHandler
        where T : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        new IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults);
        new T Configuration { get; }
    }
}
