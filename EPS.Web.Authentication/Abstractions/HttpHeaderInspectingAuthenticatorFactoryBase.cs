using System;
using EPS.Web.Authentication.Configuration;
using log4net;

namespace EPS.Web.Authentication.Abstractions
{
    //TODO: 5-24-2010 -- consider this for a future refactoring
    /*
    public abstract class HttpContextInspectingAuthenticatorFactoryBase<T, AuthenticatorType> : 
        IHttpContextInspectingAuthenticatorFactory<T>
            where T : HttpContextInspectingAuthenticatorConfigurationElement
            where AuthenticatorType: IHttpContextInspectingAuthenticator<T>, new()
     */
    public abstract class HttpContextInspectingAuthenticatorFactoryBase<T> :
        IHttpContextInspectingAuthenticatorFactory<T>
        where T : HttpContextInspectingAuthenticatorConfigurationElement
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IHttpContextInspectingAuthenticatorFactory<T> Members
        public abstract IHttpContextInspectingAuthenticator<T> Construct(T config);
        //TODO: 5-24-2010 -- consider this for a future refactoring
        /*
        public virtual IHttpContextInspectingAuthenticator<T> Construct(T config)
        {
            return new AuthenticatorType();
        }
        */
        #endregion

        #region IHttpContextInspectingAuthenticatorFactory Members
        public IHttpContextInspectingAuthenticator Construct(HttpContextInspectingAuthenticatorConfigurationElement config)
        {
            return Construct((T)config);
        }
        #endregion
    }
}
