using System;
using EPS.Web.Authentication.Basic.Configuration;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Basic
{
    public class BasicAuthenticationInspectingAuthenticatorFactory :
        HttpContextInspectingAuthenticatorFactoryBase<BasicAuthenticationHeaderInspectorConfigurationElement>
    {
        #region IHttpHeaderInspectingAuthenticatorFactory Members
        public override IHttpContextInspectingAuthenticator<BasicAuthenticationHeaderInspectorConfigurationElement> Construct(BasicAuthenticationHeaderInspectorConfigurationElement config)
        {
            return new BasicAuthenticationInspectingAuthenticator(config);
        }
        #endregion
    }
}
