using System;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication
{
    //would like to just use HttpHeaderInspectingAuthenticationFailureConfigurationSection here, but compiler won't let us
    //so we have to cook up a renamed version of the class and call it AuthenticationFailureRedirectHandlerConfigurationSection
    public class RedirectOnAuthenticationFailureHandlerFactory :
            HttpContextInspectingAuthenticationFailureHandlerFactoryBase<RedirectOnAuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        public override IHttpContextInspectingAuthenticationFailureHandler<RedirectOnAuthenticationFailureHandlerConfigurationSection> Construct(
            RedirectOnAuthenticationFailureHandlerConfigurationSection config)
        {
            return new RedirectOnAuthenticationFailureHandler(config);
        }
        #endregion
    }
}