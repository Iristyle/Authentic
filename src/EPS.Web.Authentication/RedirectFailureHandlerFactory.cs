using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    /// <summary>   Redirect on authentication failure handler factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class RedirectFailureHandlerFactory :
            FailureHandlerFactoryBase<IRedirectFailureHandlerConfiguration>
    {
        #region IFailureHandlerFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.RedirectOnAuthenticationFailureHandler"/>, returning it as a <see
        /// cref="T:
        /// EPS.Web.Authentication.Abstractions.IFailureHandler{IRedirectOnAuthenticationFailureHandlerConfigurationSection}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A redirect on failure handler. </returns>
        public override IFailureHandler<IRedirectFailureHandlerConfiguration> Construct(
            IRedirectFailureHandlerConfiguration config)
        {
            return new RedirectFailureHandler(config);
        }
        #endregion
    }
}