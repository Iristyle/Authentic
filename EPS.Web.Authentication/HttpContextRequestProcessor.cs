using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using Common.Logging;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
	/// <summary>	
	/// This class is responsible for processing an HttpContext, given a set of configuration, and handling the request appropriately.
	/// Inspection of the HttpContext is delegated to the configured plugins that follow our simple design guidelines. Inspectors are
	/// prioritized on a first-come, first-serve basis. If no suitable inspectors (that return a valid IPrincipal) are found, then the system
	/// fails over to the configured failure handler. The static 'Configure' should be implemented near the root of a given web application,
	/// to configure authenticators through code. 
	/// </summary>
	/// <remarks>	ebrown, 6/10/2011. </remarks>
	public class HttpContextRequestProcessor
	{
		private static readonly ILog log = LogManager.GetCurrentClassLogger();

		/// <summary>	Process this object. </summary>
		/// <remarks>	ebrown, 6/10/2011. </remarks>
		/// <param name="context">			The context. </param>
		/// <param name="configuration">	The configuration. </param>
		public static void Process(HttpContextBase context, IHttpAuthenticationConfiguration configuration)
		{
			// check if module is enabled
			if (!configuration.Enabled)
				return;

			log.InfoFormat(CultureInfo.InvariantCulture, "AuthenticateRequest - processing HTTP headers for authentication information");

			var inspectors = configuration.Inspectors
				.ToDictionary(i => i, i => default(AuthenticationResult));

			if (inspectors.Count > 0 && inspectors.All(i => i.Key.Configuration.RequireSsl) && !context.Request.IsSecureConnection)
			{
				log.ErrorFormat(CultureInfo.InvariantCulture, "All inspector configurations require SSL, but request is not secure");
				context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				var application = context.ApplicationInstance;
				if (null != application)
				{
					application.CompleteRequest();
				}
			}

			//try each of our inspectors in configured order - if one is successful and creates an IPrincipal, return
			if (AnyInspectorSuccesful(context, inspectors))
				return;

			//since we've dropped down this far, it means we have a problem -- nothing authenticated -- so we look at 
			//our config to determine how to respond to the failure 
			ExecuteFailureHandler(context, inspectors, configuration.FailureHandler);
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "Rare instance when this is reasonable, since we're implementing a service locator pattern where first handler to process context successfully first wins, and we eat failing handler errors")]
		private static bool AnyInspectorSuccesful(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectors)
		{
			foreach (var inspector in inspectors.Keys.ToList())
			{
				// if SSL is required by configuration but not enabled, skip to the next inspector
				if (inspector.Configuration.RequireSsl && !context.Request.IsSecureConnection)
				{
					inspectors[inspector] = new AuthenticationResult(false, null, "Inspector requires SSL, but this is not an SSL request");
					continue;
				}

				//denies always take precedence over allows
				try
				{
					inspectors[inspector] = inspector.Authenticate(context);
				}
				catch (Exception ex)
				{
					string msg = String.Format(CultureInfo.InvariantCulture, "Unexpected error authenticating with inspector [{0}] of type [{1}]", inspector.Name, inspector.GetType().Name);
					log.Error(msg, ex);
					inspectors[inspector] = new AuthenticationResult(false, null, msg);
				}

				//TODO: 9-16-2008 -- determine how we should handle this IsAuthenticated bit in OnyxPrincipal -- may have to be careful b/c of the 'denied users' ability
				if (!inspectors[inspector].Success) //null == principal || !principal.Identity.IsAuthenticated)
				{
					log.InfoFormat(CultureInfo.InvariantCulture, "{0} found nothing to validate in current HTTP header", inspector.Name);
					continue;
				}

				context.User = inspectors[inspector].Principal;
				return true;
			}

			return false;
		}

		private static void ExecuteFailureHandler(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectors, IFailureHandler failureHandler)
		{
			if (null == failureHandler)
				return;
			
			var application = context.ApplicationInstance;

			if (failureHandler.Configuration.RequireSsl && !context.Request.IsSecureConnection)
			{
				log.ErrorFormat(CultureInfo.InvariantCulture, "Inspector configuration requires SSL, but request is not secure");
				context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				
				//this is a guard since we can't effectively mock CompleteRequest in tests
				if (null != application)
				{					
					application.CompleteRequest();
				}
				return;
			}

			//new AuthenticationFailureEvent(this, (null != inspectors[failureInspector] && null != inspectors[failureInspector].Identity ? inspectors[failureInspector].Identity.Name : string.Empty)).Raise();
			var failureAction = failureHandler.OnAuthenticationFailure(context, inspectors);
			context.User = failureAction.User;

			if (failureAction.ShouldTerminateRequest)
			{
				//this is a guard since we can't effectively mock CompleteRequest in tests
				if (null != application)
				{					
					application.CompleteRequest();
				}
			}
		}
	}
}