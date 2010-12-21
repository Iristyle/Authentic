using System;
using EPS.Web.Authentication.Configuration;
using log4net;

namespace EPS.Web.Authentication.Abstractions
{
    public abstract class HttpContextInspectingAuthenticationFailureHandlerFactoryBase<T> :
           IHttpContextInspectingAuthenticationFailureHandlerFactory<T>
           where T : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IHttpContextInspectingAuthenticationFailureHandlerFactory<T> Members
        public abstract IHttpContextInspectingAuthenticationFailureHandler<T> Construct(T config);
        #endregion

        #region IHttpContextInspectingAuthenticationFailureHandlerFactory Members
        public IHttpContextInspectingAuthenticationFailureHandler Construct(HttpContextInspectingAuthenticationFailureConfigurationSection config)
        {
            return Construct((T)config);
        }
        #endregion
    }
}
