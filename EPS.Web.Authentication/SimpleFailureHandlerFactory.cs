using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    /// <summary>   Simple authentication failure handler factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class SimpleFailureHandlerFactory :
                FailureHandlerFactoryBase<ISimpleFailureHandlerConfiguration>
    {
        #region IHttpHeaderInspectingAuthenticationFailureHandlerFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.SimpleFailureHandler"/>, returning it as a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IFailureHandler{ISimpleAuthenticationFailureHandlerConfigurationSection}"/>. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A simple authentication failure handler instance. </returns>
        public override IFailureHandler<ISimpleFailureHandlerConfiguration> Construct(
            ISimpleFailureHandlerConfiguration config)
        {
            return new SimpleFailureHandler(config);
        }
        #endregion
    }
}