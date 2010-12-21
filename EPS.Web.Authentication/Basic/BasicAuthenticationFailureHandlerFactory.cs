using System;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Basic.Configuration;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Basic
{
    public class BasicAuthenticationFailureHandlerFactory : 
        HttpContextInspectingAuthenticationFailureHandlerFactoryBase<BasicAuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        public override IHttpContextInspectingAuthenticationFailureHandler<BasicAuthenticationFailureHandlerConfigurationSection> Construct(BasicAuthenticationFailureHandlerConfigurationSection config)
        {
            return new BasicAuthenticationFailureHandler(config);
        }
        #endregion
    }
}
