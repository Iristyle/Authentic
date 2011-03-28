using System;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   An implementation of a simple Basic authentication inspecting authenticator factory. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticationInspectingAuthenticatorFactory :
        HttpContextInspectingAuthenticatorFactoryBase<IAuthenticationHeaderInspectorConfigurationElement>
    {
        #region IHttpHeaderInspectingAuthenticatorFactory Members
        /// <summary>   
        /// Constructs a new instance of a <see cref="T:EPS.Web.Authentication.Basic.AuthenticationInspectingAuthenticator"/>, returning a
        /// <see cref="T:
        /// EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticator{IAuthenticationHeaderInspectorConfigurationElement}"/> . 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <param name="config">   The configuration. </param>
        /// <returns>   A new authenticator instance. </returns>
        public override IHttpContextInspectingAuthenticator<IAuthenticationHeaderInspectorConfigurationElement> Construct(IAuthenticationHeaderInspectorConfigurationElement config)
        {
            return new AuthenticationInspectingAuthenticator(config);
        }
        #endregion
    }
}