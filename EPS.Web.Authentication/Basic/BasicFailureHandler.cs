using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
	/// <summary>   A failure handler that sends out a basic authentication WWW-Authenticate header if authentication fails. </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "AvoidRedundancyInTypeNameRule", Justification = "Redundancy in type name is to avoid naming clashes / make class name more clear")]
	public class BasicFailureHandler :
		FailureHandlerBase<IBasicFailureHandlerConfiguration>
	{
		/// <summary>   Initializes a new instance of the BasicFailureHandler class given configuration values. </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <param name="config">   The configuration. </param>
		public BasicFailureHandler(IBasicFailureHandlerConfiguration config)
			: base(config) { }

		#region IFailureHandler Members
		/// <summary>   
		/// Executes the authentication failure action, sending a WWW-Authenticate header with the configured realm out through the context and
		/// setting the HTTP status code to 401 (Unauthorized). 
		/// </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <param name="context">          The incoming context. </param>
		/// <param name="inspectorResults"> The set of failed inspector results. </param>
		/// <returns>   Null -- no IPrincipal is returned as the response is completed after sending the authenticate header. </returns>
		public override IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults)
		{
			if (null == context) { throw new ArgumentNullException("context"); }

			context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			context.Response.AddHeader("WWW-Authenticate", String.Format(CultureInfo.InvariantCulture, "Basic realm=\"{0}\"", Configuration.Realm));

			//this is a guard since we can't effectively mock CompleteRequest in tests
			var application = context.ApplicationInstance;
			if (null != application)
			{
				application.CompleteRequest();
			}

			return null;
		}
		#endregion
	}
}