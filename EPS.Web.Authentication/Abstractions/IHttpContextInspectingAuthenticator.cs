using System;
using System.Web;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    public interface IHttpContextInspectingAuthenticator
    {
        string Name { get; }
        InspectorAuthenticationResult Authenticate(HttpContextBase context);
        HttpContextInspectingAuthenticatorConfigurationElement Configuration { get; }
    }

    public interface IHttpContextInspectingAuthenticator<out T> : IHttpContextInspectingAuthenticator
        where T: HttpContextInspectingAuthenticatorConfigurationElement
    {
        new T Configuration { get; }
    }
}