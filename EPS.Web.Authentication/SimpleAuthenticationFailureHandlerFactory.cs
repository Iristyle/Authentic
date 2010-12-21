using System;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication
{

    public class SimpleAuthenticationFailureHandlerFactory :
                HttpContextInspectingAuthenticationFailureHandlerFactoryBase<SimpleAuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        public override IHttpContextInspectingAuthenticationFailureHandler<SimpleAuthenticationFailureHandlerConfigurationSection> Construct(
            SimpleAuthenticationFailureHandlerConfigurationSection config)
        {
            return new SimpleAuthenticationFailureHandler(config);
        }
        #endregion
    }
}
