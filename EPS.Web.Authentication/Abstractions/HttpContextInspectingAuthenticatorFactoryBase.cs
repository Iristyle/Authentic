using System;
using Common.Logging;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    //TODO: 5-24-2010 -- consider this for a future refactoring
    /*
    public abstract class HttpContextInspectingAuthenticatorFactoryBase<T, AuthenticatorType> : 
        IHttpContextInspectingAuthenticatorFactory<T>
            where T : HttpContextInspectingAuthenticatorConfigurationElement
            where AuthenticatorType: IHttpContextInspectingAuthenticator<T>, new()
     */

    /// <summary>   
    /// A base class that should be implemented by a configured authentication handler factory.  This factory is responsible for creating an
    /// instance of the actual http context inspector following a standard protocol defined in the interface <see cref="T:
    /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticatorFactory{T}"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public abstract class HttpContextInspectingAuthenticatorFactoryBase<T> :
        IHttpContextInspectingAuthenticatorFactory<T>
        where T : HttpContextInspectingAuthenticatorConfigurationElement
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        /// <summary>   Gets the log4net log instance. </summary>
        /// <value> The log. </value>
        protected ILog Log
        {
            get { return log; }
        }

        #region IHttpContextInspectingAuthenticatorFactory<T> Members
        /// <summary>   
        /// Constructs an instance of the configured <see cref="T:EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator{T}"/>.
        /// Must be implemented by derived classes. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of an Http context inspector / authenticator. </returns>
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
        /// <summary>   
        /// Constructs an instance of the configured <see cref="T:EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator"/>.
        /// Intended to be called from infrastructure -- use generic method instead. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of an Http context inspector / authenticator. </returns>
        public IHttpContextInspectingAuthenticator Construct(HttpContextInspectingAuthenticatorConfigurationElement config)
        {
            return Construct((T)config);
        }
        #endregion
    }
}
