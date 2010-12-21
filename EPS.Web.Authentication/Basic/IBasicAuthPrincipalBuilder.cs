using System;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    public interface IBasicAuthPrincipalBuilder
    {
        string Name { get; }
        BasicAuthenticationHeaderInspectorConfigurationElement Configuration { get; }
        IPrincipal ConstructPrincipal(HttpContextBase context, string username, string password);
    }
}
