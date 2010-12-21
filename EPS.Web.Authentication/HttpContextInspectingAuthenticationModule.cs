using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;
using log4net;

namespace EPS.Web.Authentication
{
    //using a unit test friendly base class
    //http://weblogs.asp.net/rashid/archive/2009/03/12/unit-testable-httpmodule-and-httphandler.aspx
    
    public class HttpContextInspectingAuthenticationModule : HttpModuleBase
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HttpContextInspectingAuthenticationModuleSection configuration;

        protected HttpContextInspectingAuthenticationModuleSection Configuration
        {
            get
            {                
                if (configuration == null)
                    throw new ConfigurationErrorsException("Failed to load the header inspector configuration section");

                return configuration;
            }
            //this property should be inject by our IoC container
            set
            {
                if (null != configuration)
                    throw new InvalidOperationException("configuration values may only be set once");
            	
                configuration = value;
            }
        }

        /// <summary>   Executes the authenticate request action. </summary>
        /// <remarks>   ebrown, 12/21/2010. </remarks>
        /// <param name="context">  The context. </param>
        public override void OnAuthenticateRequest(HttpContextBase context)
        {            
            // check if module is enabled
            if (!Configuration.Enabled)
                return;

            log.Info("AuthenticateRequest - processing HTTP headers for authentication information");

            Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectors = 
                Configuration.Inspectors.GetInspectors().ToDictionary(i => i, i => default(InspectorAuthenticationResult));

            if (inspectors.Count > 0 && inspectors.All(i => i.Key.Configuration.RequireSSL) && !context.Request.IsSecureConnection)
            {
                log.Error("All inspector configurations require SSL, but request is not secure");
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.ApplicationInstance.CompleteRequest();
            }
            
            foreach (var inspector in inspectors.Keys.ToList())
            {
                var status = new InspectorAuthenticationResult() { Principal = null, Success = false };

                // if SSL is required by configuration but not enabled, skip to the next inspector
                if (inspector.Configuration.RequireSSL && !context.Request.IsSecureConnection)
                {
                    inspectors[inspector] = status;
                    continue;
                }

                //denies always take precedence over allows                
                try
                {
                    status = inspector.Authenticate(context);
                }
                catch (Exception ex)
                {
                    log.Error(String.Format("Unexpected error authenticating with inspector [{0}] of type [{1}]", inspector.Name, inspector.GetType().Name), ex);
                }

                inspectors[inspector] = status;
                //TODO: 9-16-2008 -- determine how we should handle this IsAuthenticated bit in OnyxPrincipal -- may have to be careful b/c of the 'denied users' ability
                if (!status.Success)
                //null == principal || !principal.Identity.IsAuthenticated)
                {
                    log.InfoFormat("{0} found nothing to validate in current HTTP header", inspector.Name);
                    continue;
                }

                context.User = status.Principal;

                return;                
            }

            //since we've dropped down this far, it means we have a problem -- nothing authenticated -- so we look at 
            //our config to determine how to respond to the failure 
            IHttpContextInspectingAuthenticationFailureHandler failureHandler = Configuration.GetFailureHandler();
            if (null == failureHandler)
                return; 

            if (failureHandler.Configuration.RequireSSL && !context.Request.IsSecureConnection)
            {
                log.Error("Inspector configuration requires SSL, but request is not secure");
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
