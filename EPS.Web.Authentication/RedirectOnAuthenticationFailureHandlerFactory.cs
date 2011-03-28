using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    /// <summary>   Redirect on authentication failure handler factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class RedirectOnAuthenticationFailureHandlerFactory :
            HttpContextInspectingAuthenticationFailureHandlerFactoryBase<IRedirectOnAuthenticationFailureHandlerConfigurationSection>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.RedirectOnAuthenticationFailureHandler"/>, returning it as a <see
        /// cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandler{IRedirectOnAuthenticationFailureHandlerConfigurat
        /// ionSection}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A redirect on failure handler. </returns>
        public override IHttpContextInspectingAuthenticationFailureHandler<IRedirectOnAuthenticationFailureHandlerConfigurationSection> Construct(
            IRedirectOnAuthenticationFailureHandlerConfigurationSection config)
        {
            return new RedirectOnAuthenticationFailureHandler(config);
        }
        #endregion
    }
}