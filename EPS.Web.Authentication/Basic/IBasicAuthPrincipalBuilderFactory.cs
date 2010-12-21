using System;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    public interface IBasicAuthPrincipalBuilderFactory
    {
        IBasicAuthPrincipalBuilder Construct(BasicAuthenticationHeaderInspectorConfigurationElement config);
    }
}
