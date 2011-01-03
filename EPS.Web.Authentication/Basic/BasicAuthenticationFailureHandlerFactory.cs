using System;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Basic.Configuration;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   An implementation of a simple Basic authentication failure handler factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class BasicAuthenticationFailureHandlerFactory : 
        HttpContextInspectingAuthenticationFailureHandlerFactoryBase<BasicAuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Basic.BasicAuthenticationFailureHandler"/>, returning a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandler{BasicAuthenticationFailureHandlerConfigurationSe
        /// ction}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A new failure handler instance. </returns>
        public override IHttpContextInspectingAuthenticationFailureHandler<BasicAuthenticationFailureHandlerConfigurationSection> Construct(BasicAuthenticationFailureHandlerConfigurationSection config)
        {
            return new BasicAuthenticationFailureHandler(config);
        }
        #endregion
    }
}
