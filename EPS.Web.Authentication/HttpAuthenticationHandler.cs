using System;
using System.Web;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{

	/// <summary>   
	/// This is the meat of the authentication system.  This class is the top-level HttpModule responsible for delegating inspection of
	/// HttpContext to any plugins that follow our simple design guidelines. Inspectors are prioritized on a first-come, first-serve basis
	/// into the ProcessRequest handler. If no suitable inspectors (that return a valid IPrincipal) are found, then the system
	/// fails over to the configured failure handler. The static 'Configure' should be implemented near the root of a given web application,
	/// to configure authenticators through code.
	/// </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	public class HttpAuthenticationHandler : HttpHandlerBase
	{
		private IHttpAuthenticationConfiguration configuration;
		//TODO: 6-10-2011 -- we should provide a similar hook for the standard .config parsers to publish this information up to the module level
		private static Func<HttpAuthenticationHandler, IHttpAuthenticationConfiguration> _configure;
		private static string RequestProcessedKey = Guid.NewGuid().ToString("N");

		/// <summary>	
		/// A user defined configuration function so that the module may be configured in code.  This should be implemented as close to the root
		/// of the application as possible, for instance in global.asax.cs Application_Start or similar.  If a request comes in before this is
		/// setup, an Exception will be thrown. 
		/// </summary>
		/// <value>	A custom method responsible for returning configuration information for the . </value>
		public static Func<HttpAuthenticationHandler, IHttpAuthenticationConfiguration> Configure
		{
			get { return _configure; }
			set
			{
				if (null != _configure)
				{
					throw new InvalidOperationException("Only one configuration resolver may be set during the lifetime of the HttpAuthenticationHandler");
				}

				_configure = value;
			}
		}

		/// <summary>   
		/// This property is intended to be set only once by an IoC container (or manually in tests). After an initial set, the property becomes
		/// read-only and throws exceptions. 
		/// </summary>
		/// <exception cref="InvalidOperationException">    Thrown when configuration has not been injected, OR when it is injected more than once. </exception>
		/// <value> The configuration defining which inspectors to plugin to the system and how to handle failures. </value>
		public IHttpAuthenticationConfiguration Configuration
		{
			get
			{
				if (configuration == null)
				{
					if (null == _configure)
					{
						throw new NotImplementedException("HttpAuthenticationHandler.Configure must be set in code to enable authentication");
					}

					configuration = _configure(this);

					if (null == configuration)
					{
						throw new InvalidOperationException("Failed to load the header inspector configuration.  HttpAuthenticationHandler.Configure has been set, but does not return a valid IModuleConfiguration instance.");
					}
				}

				return configuration;
			}
		}

		/// <summary>	
		/// Enables processing of HTTP requests through a custom authentication HttpHandler.  This handler enumerates our configured list of
		/// inspectors. The first <see cref="T: EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator"/> that returns a Success
		/// = true status, with a valid non- null IPrincipal wins. Inspectors are executed in the order they are configured.  <see cref="M:
		/// System.Web.HttpContext.Current.User"/>
		/// is set to the non-null IPrincipal returned by an inspector on success.
		/// 
		/// If no suitable IPrincipal can be constructed, the system fails over to the configured failure handler, that has the ability to return
		/// 304 status, request credentials, or any other valid HTTP action. 
		/// </summary>
		/// <remarks>	ebrown, 6/10/2011. </remarks>
		/// <exception cref="ArgumentNullException">	Thrown when one or more required arguments are null. </exception>
		/// <param name="context">	An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects
		/// 						(for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
		public override void ProcessRequest(HttpContextBase context)
		{
			//this shouldn't ever happen
			if (null == context) { throw new ArgumentNullException("context"); }

			//prevent double execution
			if (!context.Items.Contains(RequestProcessedKey))
			{
				context.Items[RequestProcessedKey] = true;
				HttpContextRequestProcessor.Process(context, Configuration);
			}
		}		
	}
}