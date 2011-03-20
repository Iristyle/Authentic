using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using Common.Logging;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    /// <summary>   
    /// A base class for implementing custom http context inspection failure handlers as defined by <see cref="T:
    /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandler{T}"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public abstract class HttpContextInspectingAuthenticationFailureHandlerBase<T> :
            IHttpContextInspectingAuthenticationFailureHandler<T>
            where T : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private readonly T _config;

        private HttpContextInspectingAuthenticationFailureHandlerBase()
        { }

        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        protected HttpContextInspectingAuthenticationFailureHandlerBase(T config)
        {
            this._config = config;
        }

        /// <summary>   
        /// Gets the base configuration section -- don't use this as it may not contain customized configuration information. Intended to be used
        /// only by infrastructure. 
        /// </summary>
        /// <value> The configuration. </value>
        HttpContextInspectingAuthenticationFailureConfigurationSection IHttpContextInspectingAuthenticationFailureHandler.Configuration
        {
            get { return _config; }
        }

        /// <summary>   Gets the log4net log instance. </summary>
        /// <value> The log. </value>
        protected ILog Log
        {
            get { return log; }
        }

        /// <summary>   Gets the typed configuration section defined in the objects class. </summary>
        /// <value> The configuration. </value>
        public T Configuration
        {
            get { return _config; }
        }

        /// <summary>   Executes the authentication failure action.  Must be implemented in derived classes. </summary>
        /// <param name="context">          The context. </param>
        /// <param name="inspectorResults"> The set of failed inspector results. </param>
        /// <returns>   The inspector may end the HTTP request or return an IPrincipal for use when all other authentication inspector attempts fail. </returns>
        public abstract IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults);
    }
}