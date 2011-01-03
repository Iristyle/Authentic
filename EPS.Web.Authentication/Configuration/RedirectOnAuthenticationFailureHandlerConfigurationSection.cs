using System;
using System.Configuration;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   
    /// Redirect on authentication failure handler configuration section that defines the configurable behavior of
    /// <see cref="T:EPS.Web.Authentication.RedirectOnAuthenticationFailureHandler"/>. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class RedirectOnAuthenticationFailureHandlerConfigurationSection : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        /// <summary>   Gets or sets URI for the redirect on a failed request. </summary>
        /// <value> The redirect uri. </value>
        [ConfigurationProperty("redirectUri", IsRequired = true)]
        public Uri RedirectUri
        {
            get { return (Uri)this["redirectUri"]; }
            set { base["redirectUri"] = value; }
        }
    }
}