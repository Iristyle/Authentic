using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   An implementation of a simple Basic authentication failure handler factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticationFailureHandlerFactory : 
        HttpContextInspectingAuthenticationFailureHandlerFactoryBase<AuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Basic.AuthenticationFailureHandler"/>, returning a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandler{AuthenticationFailureHandlerConfigurationSe
        /// ction}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A new failure handler instance. </returns>
        public override IHttpContextInspectingAuthenticationFailureHandler<AuthenticationFailureHandlerConfigurationSection> Construct(AuthenticationFailureHandlerConfigurationSection config)
        {
            return new AuthenticationFailureHandler(config);
        }
        #endregion
    }
}
