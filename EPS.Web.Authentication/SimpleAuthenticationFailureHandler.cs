using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
    /// <summary>   A very simple authentication failure handler that allows for clients to simply hook a failure event. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class SimpleAuthenticationFailureHandler :
               HttpContextInspectingAuthenticationFailureHandlerBase<SimpleAuthenticationFailureHandlerConfigurationSection>
    {
        /// <summary>   Constructor an instance of the failure handler given the configuration. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration (empty). </param>
        public SimpleAuthenticationFailureHandler(SimpleAuthenticationFailureHandlerConfigurationSection config)
            : base(config) { }

        /// <summary> The authentication failure handler -- implementors must set the IPrincipal here to something
        /// </summary>   
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible", Justification = "Add/remove is thread-safe for events in .NET.")]
        public static EventHandler<SimpleAuthenticationFailureEventArgs> AuthenticationFailure;
        //TODO: 8-9-2010 -- prevent against multiple hooks here

        #region IHttpHeaderInspectingAuthenticationFailureHandler Members
        /// <summary>   Executes the authentication failure action. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="context">          The incoming HttpContextBase. </param>
        /// <param name="inspectorResults"> The set of failed inspector results. </param>
        /// <returns>   An IPrincipal instance as returned by the failure event handler. </returns>
        public override IPrincipal OnAuthenticationFailure(HttpContextBase context,
            Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults)
        {
            var eventArgs = new SimpleAuthenticationFailureEventArgs(Configuration, context, inspectorResults);

            if (null != AuthenticationFailure)
                AuthenticationFailure(this, eventArgs);

            return eventArgs.IPrincipal;
        }
        #endregion
    }
}