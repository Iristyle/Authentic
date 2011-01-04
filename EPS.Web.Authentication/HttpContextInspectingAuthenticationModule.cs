using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;
using log4net;

namespace EPS.Web.Authentication
{
    /// <summary>   
    /// This is the meat of the authentication system.  This class is the top-level HttpModule responsible for delegating inspection of
    /// HttpContext to any plugins that follow our simple design guidelines. Inspectors are prioritized on a first-come, first-serve basis
    /// into the OnPostAuthenticateRequest handler. If no suitable inspectors (that return a valid IPrincipal) are found, then the system
    /// fails over to the configured failure handler. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class HttpContextInspectingAuthenticationModule : HttpModuleBase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HttpContextInspectingAuthenticationModuleSection configuration;

        /// <summary>   
        /// This property is intended to be set only once by an IoC container (or manually in tests). After an initial set, the property becomes
        /// read-only and throws exceptions. 
        /// </summary>
        /// <exception cref="InvalidOperationException">    Thrown when configuration has not been injected, OR when it is injected more than once. </exception>
        /// <value> The configuration defining which inspectors to plugin to the system and how to handle failures. </value>
        protected HttpContextInspectingAuthenticationModuleSection Configuration
        {
            get
            {
                if (configuration == null)
                {
                    throw new InvalidOperationException("Failed to load the header inspector configuration section, either this dependency has not been injected or set manually before using");
                }

                return configuration;
            }
            //this property should be inject by our IoC container
            set
            {
                if (null != configuration)
                    throw new InvalidOperationException("Configuration values may only be set once");
            	
                configuration = value;
            }
        }

        /// <summary>   
        /// Executes the authenticate request action, by enumerating through our configured list of inspectors. The first <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator"/> that returns a Success = true status, with a valid non-
        /// null IPrincipal wins. Inspectors are executed in the order they are configured.  <see cref="M:System.Web.HttpContext.Current.User"/>
        /// is set to the non-null IPrincipal returned by an inspector on success.
        /// 
        /// If no suitable IPrincipal can be constructed, the system fails over to the configured failure handler, that has the ability to return
        /// 304 status, request credentials, or any other valid HTTP action. 
        /// </summary>
        /// <remarks>   ebrown, 12/21/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="context">  The context. </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", 
            Justification = "Rare instance when this is reasonable, since we're implementing a service locator pattern where first handler to process context successfully first wins, and we eat failing handler errors")]            
        public override void OnAuthenticateRequest(HttpContextBase context)
        {
            //this shouldn't ever happen
            if (null == context) { throw new ArgumentNullException("context"); }

            // check if module is enabled
            if (!Configuration.Enabled)
                return;

            log.InfoFormat(CultureInfo.InvariantCulture, "AuthenticateRequest - processing HTTP headers for authentication information");

            Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectors = 
                HttpContextInspectorsLocator.Construct(Configuration.Inspectors)
                .ToDictionary(i => i, i => default(InspectorAuthenticationResult));

            if (inspectors.Count > 0 && inspectors.All(i => i.Key.Configuration.RequireSsl) && !context.Request.IsSecureConnection)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "All inspector configurations require SSL, but request is not secure");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.ApplicationInstance.CompleteRequest();
            }
            
            foreach (var inspector in inspectors.Keys.ToList())
            {
                // if SSL is required by configuration but not enabled, skip to the next inspector
                if (inspector.Configuration.RequireSsl && !context.Request.IsSecureConnection)
                {
                    inspectors[inspector] = new InspectorAuthenticationResult(false, null, "Inspector requires SSL, but this is not an SSL request");
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
                    inspectors[inspector] = new InspectorAuthenticationResult(false, null, msg);
                }

                //TODO: 9-16-2008 -- determine how we should handle this IsAuthenticated bit in OnyxPrincipal -- may have to be careful b/c of the 'denied users' ability
                if (!inspectors[inspector].Success) //null == principal || !principal.Identity.IsAuthenticated)
                {
                    log.InfoFormat(CultureInfo.InvariantCulture, "{0} found nothing to validate in current HTTP header", inspector.Name);
                    continue;
                }

                context.User = inspectors[inspector].Principal;

                return;                
            }

            //since we've dropped down this far, it means we have a problem -- nothing authenticated -- so we look at 
            //our config to determine how to respond to the failure 
            IHttpContextInspectingAuthenticationFailureHandler failureHandler = HttpContextInspectorsLocator.GetFailureHandler(Configuration);
            if (null == failureHandler)
                return; 

            if (failureHandler.Configuration.RequireSsl && !context.Request.IsSecureConnection)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "Inspector configuration requires SSL, but request is not secure");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.ApplicationInstance.CompleteRequest();
                return;
            }            

            //new AuthenticationFailureEvent(this, (null != inspectors[failureInspector] && null != inspectors[failureInspector].Identity ? inspectors[failureInspector].Identity.Name : string.Empty)).Raise();
            context.User = failureHandler.OnAuthenticationFailure(context, inspectors);             
        }

        /// <summary>   Executes the post authenticate request action. </summary>
        /// <remarks>   ebrown, 12/21/2010. </remarks>
        /// <param name="context">  The context. </param>
        public override void OnPostAuthenticateRequest(HttpContextBase context)
        {
            base.OnPostAuthenticateRequest(context);
        }

        /// <summary>   Executes the authorize request action. </summary>
        /// <remarks>   ebrown, 12/21/2010. </remarks>
        /// <param name="context">  The context. </param>
        public override void OnAuthorizeRequest(HttpContextBase context)
        {
            base.OnAuthorizeRequest(context);
        }

        /// <summary>   Executes the end request action. </summary>
        /// <remarks>   ebrown, 12/21/2010. </remarks>
        /// <param name="context">  The context. </param>
        public override void OnEndRequest(HttpContextBase context)
        {
            base.OnEndRequest(context);
            //if (Configuration.Enabled && (context.Context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)) //401
            //    SendAuthenticationHeader();
        }

        /// <summary>   Executes the post authorize request action. </summary>
        /// <remarks>   ebrown, 12/21/2010. </remarks>
        /// <param name="context">  The context. </param>
        public override void OnPostAuthorizeRequest(HttpContextBase context)
        {
            base.OnPostAuthorizeRequest(context);
        }        
    }
}