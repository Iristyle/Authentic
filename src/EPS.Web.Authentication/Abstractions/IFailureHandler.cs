using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
	/// <summary>   Interface for http context inspecting authentication failure handler. </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	public interface IFailureHandler
	{
		/// <summary>	Executes the authentication failure action. </summary>
		/// <param name="context">		   	The context. </param>
		/// <param name="inspectorResults">	The set of failed inspector results. </param>
		/// <returns>
		/// The inspector should return an IPrincipal for use when all other authentication inspector attempts fail and a boolean value
		/// describing whether or not to terminate the HTTP request.
		/// </returns>
		FailureHandlerAction OnAuthenticationFailure(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults);

		/// <summary>   Gets the configuration. </summary>
		/// <value> The configuration. </value>
		IFailureHandlerConfiguration Configuration { get; }
	}

	/// <summary>   
	/// Generic version of the IFailureHandler interface that allows specialization of configuration. 
	/// </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	public interface IFailureHandler<T>
		: IFailureHandler
		where T : class, IFailureHandlerConfiguration
	{
		/// <summary>   Gets the configuration. </summary>
		/// <value> The configuration. </value>
		new T Configuration { get; }
	}
}