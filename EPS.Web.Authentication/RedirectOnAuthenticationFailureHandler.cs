using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    /// <summary>   A very simple authentication failure handler that redirects to a preconfigured url. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class RedirectOnAuthenticationFailureHandler :
           HttpContextInspectingAuthenticationFailureHandlerBase<IRedirectOnAuthenticationFailureHandlerConfigurationSection>
    {
        /// <summary>   Constructs an instance of a RedirectOnAuthenticationFailureHandler. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        public RedirectOnAuthenticationFailureHandler(IRedirectOnAuthenticationFailureHandlerConfigurationSection config)
            : base(config) { }

        #region IHttpHeaderInspectingAuthenticationFailureHandler Members
        /// <summary>   Implements the authentication failure action, redirecting to a specified Uri. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="context">          The incoming request context. </param>
        /// <param name="inspectorResults"> The failed inspector results. </param>
        /// <returns>   Always returns a null IPrincipal. </returns>
        public override IPrincipal OnAuthenticationFailure(HttpContextBase context, 
            Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults)
        {
            //this shouldn't ever happen
            if (null == context) { throw new ArgumentNullException("context"); }

            context.Response.Redirect(Configuration.RedirectUri.ToUrl(), true);
            return null;
        }
        #endregion
    }
}