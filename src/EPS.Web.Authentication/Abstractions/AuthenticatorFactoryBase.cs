using System;
using Common.Logging;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    //TODO: 5-24-2010 -- consider this for a future refactoring
    /*
    public abstract class AuthenticatorFactoryBase<T, AuthenticatorType> : 
        IAuthenticatorFactory<T>
            where T : HttpContextInspectingAuthenticatorConfigurationElement
            where AuthenticatorType: IAuthenticator<T>, new()
     */

    /// <summary>   
    /// A base class that should be implemented by a configured authentication handler factory.  This factory is responsible for creating an
    /// instance of the actual http context inspector following a standard protocol defined in the interface <see cref="T:
    /// EPS.Web.Authentication.Abstractions.IAuthenticatorFactory{T}"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public abstract class AuthenticatorFactoryBase<T> :
        IAuthenticatorFactory<T>
            where T : class, IAuthenticatorConfiguration
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        /// <summary>   Gets the log4net log instance. </summary>
        /// <value> The log. </value>
        protected ILog Log
        {
            get { return log; }
        }

        #region IAuthenticatorFactory<T> Members
        /// <summary>   
        /// Constructs an instance of the configured <see cref="T:EPS.Web.Authentication.Abstractions.IAuthenticator{T}"/>.
        /// Must be implemented by derived classes. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of an Http context inspector / authenticator. </returns>
        public abstract IAuthenticator<T> Construct(T config);
        //TODO: 5-24-2010 -- consider this for a future refactoring
        /*
        public virtual IHttpContextInspectingAuthenticator<T> Construct(T config)
        {
            return new AuthenticatorType();
        }
        */
        #endregion

        #region IAuthenticatorFactory Members
        /// <summary>   
        /// Constructs an instance of the configured <see cref="T:EPS.Web.Authentication.Abstractions.IAuthenticator"/>.
        /// Intended to be called from infrastructure -- use generic method instead. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of an Http context inspector / authenticator. </returns>
        public IAuthenticator Construct(IAuthenticatorConfiguration config)
        {
            return Construct((T)config);
        }
        #endregion
    }
}