using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    public interface IHttpContextInspectingAuthenticatorFactory
    {
        IHttpContextInspectingAuthenticator Construct(HttpContextInspectingAuthenticatorConfigurationElement config);
    }

    public interface IHttpContextInspectingAuthenticatorFactory<T> : IHttpContextInspectingAuthenticatorFactory
        where T: HttpContextInspectingAuthenticatorConfigurationElement
    {
        IHttpContextInspectingAuthenticator<T> Construct(T config);
    }
}
