using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Configuration;
using log4net;

namespace EPS.Web.Authentication.Abstractions
{
    public abstract class HttpContextInspectingAuthenticationFailureHandlerBase<T> :
            IHttpContextInspectingAuthenticationFailureHandler<T>
            where T : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected T config;

        private HttpContextInspectingAuthenticationFailureHandlerBase()
        { }

        protected HttpContextInspectingAuthenticationFailureHandlerBase(T config)
        {
            this.config = config;
        }

        HttpContextInspectingAuthenticationFailureConfigurationSection IHttpContextInspectingAuthenticationFailureHandler.Configuration
        {
            get { return config; }
        }

        public T Configuration
        {
            get { return config; }
        }

        public abstract IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults);
    }
}
