using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    public interface IHttpContextInspectingAuthenticationFailureHandlerFactory
    {
        IHttpContextInspectingAuthenticationFailureHandler Construct(HttpContextInspectingAuthenticationFailureConfigurationSection config);
    }

    public interface IHttpContextInspectingAuthenticationFailureHandlerFactory<T> : 
        IHttpContextInspectingAuthenticationFailureHandlerFactory
        where T : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        IHttpContextInspectingAuthenticationFailureHandler<T> Construct(T config);
    }

}