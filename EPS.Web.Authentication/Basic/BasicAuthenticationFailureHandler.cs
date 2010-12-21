using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    public class BasicAuthenticationFailureHandler : 
        HttpContextInspectingAuthenticationFailureHandlerBase<BasicAuthenticationFailureHandlerConfigurationSection>
    {
        internal BasicAuthenticationFailureHandler(BasicAuthenticationFailureHandlerConfigurationSection config)
            : base(config) {}

        #region IHttpHeaderInspectingAuthenticationFailureHandler Members
        public override IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.AddHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", Configuration.Realm));
            context.ApplicationInstance.CompleteRequest();

            return null;
        }

        #endregion
    }

}
