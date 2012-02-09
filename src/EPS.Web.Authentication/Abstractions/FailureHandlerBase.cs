using System.Collections.Generic;
using System.Web;
using Common.Logging;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
	/// <summary>   
	/// A base class for implementing custom http context inspection failure handlers as defined by <see cref="T:
	/// EPS.Web.Authentication.Abstractions.IFailureHandler{T}"/>. 
	/// </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	public abstract class FailureHandlerBase<T> :
			IFailureHandler<T>
			where T : class, IFailureHandlerConfiguration
	{
		private static readonly ILog log = LogManager.GetCurrentClassLogger();
		private readonly T _config;

		private FailureHandlerBase()
		{ }

		/// <summary>   Constructor. </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		protected FailureHandlerBase(T config)
		{
			this._config = config;
		}

		/// <summary>   
		/// Gets the base configuration section -- don't use this as it may not contain customized configuration information. Intended to be used
		/// only by infrastructure. 
		/// </summary>
		/// <value> The configuration. </value>
		IFailureHandlerConfiguration IFailureHandler.Configuration
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

		/// <summary>	Executes the authentication failure action.  Must be implemented in derived classes. </summary>
		/// <remarks>	8/24/2011. </remarks>
		/// <param name="context">		   	The context. </param>
		/// <param name="inspectorResults">	The set of failed inspector results. </param>
		/// <returns>
		/// The inspector should return an IPrincipal for use when all other authentication inspector attempts fail and a boolean value
		/// describing whether or not to terminate the HTTP request.
		/// </returns>
		public abstract FailureHandlerAction OnAuthenticationFailure(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults);
	}
}