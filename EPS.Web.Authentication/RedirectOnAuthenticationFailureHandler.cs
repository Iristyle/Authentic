using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Abstractions;


namespace EPS.Web.Authentication
{
    public class RedirectOnAuthenticationFailureHandler :
           HttpContextInspectingAuthenticationFailureHandlerBase<RedirectOnAuthenticationFailureHandlerConfigurationSection>
    {
        internal RedirectOnAuthenticationFailureHandler(RedirectOnAuthenticationFailureHandlerConfigurationSection config)
            : base(config) { }

        #region IHttpHeaderInspectingAuthenticationFailureHandler Members
        public override IPrincipal OnAuthenticationFailure(HttpContextBase context, 
            Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults)
        {
            context.Response.Redirect(Configuration.RedirectUri.ToUrl(), true);
            return null;
        }
        #endregion
    }
}
