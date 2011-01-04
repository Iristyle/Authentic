using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    /// <summary>   Factory interface for constructing a specific type of http context inspecting authenticator. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IHttpContextInspectingAuthenticatorFactory
    {
        /// <summary>   Constructs the http context inspecting authenticator. </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An authenticator instance. </returns>
        IHttpContextInspectingAuthenticator Construct(HttpContextInspectingAuthenticatorConfigurationElement config);
    }

    /// <summary>   
    /// Generic factory interface for constructing a specific type of http context inspecting authenticator, given a more specialized
    /// configuration type. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IHttpContextInspectingAuthenticatorFactory<T> : IHttpContextInspectingAuthenticatorFactory
        where T : HttpContextInspectingAuthenticatorConfigurationElement
    {
        /// <summary>   Constructs the http context inspecting authenticator. </summary>
        /// <param name="config">   The specialized configuration as specified in the class definition. </param>
        /// <returns>   An authenticator instance. </returns>
        IHttpContextInspectingAuthenticator<T> Construct(T config);
    }
}
