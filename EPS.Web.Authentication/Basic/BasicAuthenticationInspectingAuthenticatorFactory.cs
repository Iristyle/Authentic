using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   An implementation of a simple Basic authentication inspecting authenticator factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class BasicAuthenticationInspectingAuthenticatorFactory :
        HttpContextInspectingAuthenticatorFactoryBase<BasicAuthenticationHeaderInspectorConfigurationElement>
    {
        #region IHttpHeaderInspectingAuthenticatorFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Basic.BasicAuthenticationInspectingAuthenticator"/>, returning a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator{BasicAuthenticationHeaderInspectorConfigurationElement}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A new authenticator instance. </returns>
        public override IHttpContextInspectingAuthenticator<BasicAuthenticationHeaderInspectorConfigurationElement> Construct(BasicAuthenticationHeaderInspectorConfigurationElement config)
        {
            return new BasicAuthenticationInspectingAuthenticator(config);
        }
        #endregion
    }
}