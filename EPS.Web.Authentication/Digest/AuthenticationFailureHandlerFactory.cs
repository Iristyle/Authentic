using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;

namespace EPS.Web.Authentication.Digest
{
    /// <summary>   An implementation of a simple Digest authentication failure handler factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticationFailureHandlerFactory : 
        HttpContextInspectingAuthenticationFailureHandlerFactoryBase<IAuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Digest.AuthenticationFailureHandler"/>, returning a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandler{IAuthenticationFailureHandlerConfigurationSe
        /// ction}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A new failure handler instance. </returns>
        public override IHttpContextInspectingAuthenticationFailureHandler<IAuthenticationFailureHandlerConfigurationSection> Construct(IAuthenticationFailureHandlerConfigurationSection config)
        {
            return new AuthenticationFailureHandler(config);
        }
        #endregion
    }
}