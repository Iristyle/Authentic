using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    /// <summary>   
    /// Additional information for simple authentication failure events. Clients are responsible for filling in the specified IPrincipal. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    [SuppressMessage("Gendarme.Rules.Maintainability", "AvoidLackOfCohesionOfMethodsRule", Justification = "This is a simple EventArgs class that should not be difficult to understand")]
    public class SimpleFailureEventArgs : EventArgs
    {
        /// <summary>   Gets the configuration. </summary>
        /// <value> The configuration. </value>
        public ISimpleFailureHandlerConfiguration Config { get; private set; }
        
        /// <summary>   Gets the http context base. </summary>
        /// <value> The http context base. </value>
        public HttpContextBase HttpContextBase { get; private set; }
        
        /// <summary>   Gets the failed inspector results. </summary>
        /// <value> The inspector results. </value>
        public Dictionary<IAuthenticator, AuthenticationResult> InspectorResults { get; private set; }
        
        /// <summary>   Gets or sets the principal.  Event handlers must set this value. </summary>
        /// <value> The IPrincipal. </value>
        public IPrincipal IPrincipal { get; set; }

        /// <summary>   
        /// Initializes a new instance of the AuthenticationFailureGenericEventArgs class that will be to passed to an event client. The client
        /// is responsible for filling in the IPrincipal. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">           The configuration. </param>
        /// <param name="httpContext">      The HttpContext for the given request. </param>
        /// <param name="inspectorResults"> The set of failed inspector results. </param>
        public SimpleFailureEventArgs(ISimpleFailureHandlerConfiguration config, HttpContextBase httpContext, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults)
        {
            Config = config;
            HttpContextBase = httpContext;
            InspectorResults = inspectorResults;
        }
    }
}