using System;

namespace EPS.Web.Authentication
{
    /// <summary>   Provides a hook for code-based configuration of the authentication module. </summary>
    /// <remarks>   ebrown, 4/7/2011. </remarks>
    public class HttpAuthenticationModuleConfigureEventArgs : EventArgs
    {
        /// <summary>   Gets or sets the instance to configure. </summary>
        /// <value> The instance. </value>
        public HttpAuthenticationModule Instance { get; private set; }

        /// <summary>
        /// Initializes a new instance of the HttpContextInspectingAuthenticationModuleConfigureEventArgs class.
        /// </summary>
        public HttpAuthenticationModuleConfigureEventArgs(HttpAuthenticationModule instance)
        {
            Instance = instance;
        }
    }
}
