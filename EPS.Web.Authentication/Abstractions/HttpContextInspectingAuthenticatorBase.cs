using System;
using System.Web;
using EPS.Web.Authentication.Configuration;
using log4net;

namespace EPS.Web.Authentication.Abstractions
{
    public abstract class HttpContextInspectingAuthenticatorBase<T> : IHttpContextInspectingAuthenticator<T> where T : HttpContextInspectingAuthenticatorConfigurationElement
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected T config;

        private HttpContextInspectingAuthenticatorBase()
        { }

        internal HttpContextInspectingAuthenticatorBase(T config)
        {
            this.config = config;
        }

        public string Name
        {
            get { return ((HttpContextInspectingAuthenticatorConfigurationElement)config).Name; }
        }

        /*
        HttpContextInspectingAuthenticatorConfigurationElement IHttpContextInspectingAuthenticator<T>.Configuration
        {
            get { return config; }
        }
        */

        HttpContextInspectingAuthenticatorConfigurationElement IHttpContextInspectingAuthenticator.Configuration
        {
            get { return config; }
        }

        public T Configuration
        {
            get { return config; }
        }

        public abstract InspectorAuthenticationResult Authenticate(HttpContextBase context);
    }
}
