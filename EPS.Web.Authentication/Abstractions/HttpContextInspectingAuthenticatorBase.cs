using System;
using System.Web;
using EPS.Web.Authentication.Configuration;
using log4net;

namespace EPS.Web.Authentication.Abstractions
{
    /// <summary>   
    /// A base class for implementing custom http context inspection handlers as defined by <see cref="T:
    /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator{T}"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public abstract class HttpContextInspectingAuthenticatorBase<T> : IHttpContextInspectingAuthenticator<T> where T : HttpContextInspectingAuthenticatorConfigurationElement
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        private readonly T config;
        
        private HttpContextInspectingAuthenticatorBase()
        { }

        /// <summary>   Constructor that must be called by implementors to properly pass configuration information. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        protected HttpContextInspectingAuthenticatorBase(T config)
        {
            this.config = config;
        }

        /// <summary>   Gets the log4net log instance. </summary>
        /// <value> The log. </value>
        protected ILog Log
        {
            get { return log; }
        }

        /// <summary>   Gets the human-friendly name of the inspector. </summary>
        /// <value> The name. </value>
        public string Name
        {
            get { return ((HttpContextInspectingAuthenticatorConfigurationElement)Configuration).Name; }
        }

        /*
        HttpContextInspectingAuthenticatorConfigurationElement IHttpContextInspectingAuthenticator<T>.Configuration
        {
            get { return config; }
        }
        */

        /// <summary>   Gets the base configuration instance -- intended to be used by infrastructure. </summary>
        /// <value> The configuration. </value>
        HttpContextInspectingAuthenticatorConfigurationElement IHttpContextInspectingAuthenticator.Configuration
        {
            get { return config; }
        }

        /// <summary>   Gets the specific configuration instance as specified in the generic class definition. </summary>
        /// <value> The configuration. </value>
        public T Configuration
        {
            get { return config; }
        }

        /// <summary>   Authenticates based on a given HttpContextBase. </summary>
        /// <param name="context">  The context. </param>
        /// <returns>   A result instance that indicates where the authentication failed, and if succesful returns an IPrincipal instance. </returns>
        public abstract InspectorAuthenticationResult Authenticate(HttpContextBase context);
    }
}
