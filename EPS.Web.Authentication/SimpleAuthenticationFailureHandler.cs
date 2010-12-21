using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication
{
    public class SimpleAuthenticationFailureHandler :
               HttpContextInspectingAuthenticationFailureHandlerBase<SimpleAuthenticationFailureHandlerConfigurationSection>
    {
        internal SimpleAuthenticationFailureHandler(SimpleAuthenticationFailureHandlerConfigurationSection config)
            : base(config) { }

        /// <summary>
        /// Implementors must set the IPrincipal here to something
        /// </summary>
        //TODO: 8-9-2010 -- prevent against multiple hooks here
        public static EventHandler<SimpleAuthenticationFailureEventArgs> AuthenticationFailure;

        #region IHttpHeaderInspectingAuthenticationFailureHandler Members
        public override IPrincipal OnAuthenticationFailure(HttpContextBase context,
            Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults)
        {
            var eventArgs = new SimpleAuthenticationFailureEventArgs(config, context, inspectorResults);

            if (null != AuthenticationFailure)
                AuthenticationFailure(this, eventArgs);

            return eventArgs.IPrincipal;
        }
        #endregion
    }
}
