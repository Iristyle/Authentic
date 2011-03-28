using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    /// <summary>   Simple authentication failure handler factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class SimpleAuthenticationFailureHandlerFactory :
                HttpContextInspectingAuthenticationFailureHandlerFactoryBase<ISimpleAuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.SimpleAuthenticationFailureHandler"/>, returning it as a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandler{ISimpleAuthenticationFailureHandlerConfigurationS
        /// ection}"/>. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A simple authentication failure handler instance. </returns>
        public override IHttpContextInspectingAuthenticationFailureHandler<ISimpleAuthenticationFailureHandlerConfigurationSection> Construct(
            ISimpleAuthenticationFailureHandlerConfigurationSection config)
        {
            return new SimpleAuthenticationFailureHandler(config);
        }
        #endregion
    }
}