using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    public class SimpleAuthenticationFailureEventArgs : EventArgs
    {
        public SimpleAuthenticationFailureHandlerConfigurationSection Config { get; private set; }
        public HttpContextBase HttpContextBase { get; private set; }
        public Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> InspectorResults { get; private set; }
        public IPrincipal IPrincipal { get; set; }

        /// <summary>
        /// Initializes a new instance of the AuthenticationFailureGenericEventArgs class.
        /// </summary>
        public SimpleAuthenticationFailureEventArgs(SimpleAuthenticationFailureHandlerConfigurationSection config, HttpContextBase httpContext, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults)
        {
            Config = config;
            HttpContextBase = httpContext;
            InspectorResults = inspectorResults;
        }

    }
}
