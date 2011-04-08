using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    /// <summary>   Factory interface for constructing a specific type of http context inspecting authenticator. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IAuthenticatorFactory
    {
        /// <summary>   Constructs the http context inspecting authenticator. </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An authenticator instance. </returns>
        IAuthenticator Construct(IAuthenticatorConfiguration config);
    }

    /// <summary>   
    /// Generic factory interface for constructing a specific type of http context inspecting authenticator, given a more specialized
    /// configuration type. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IAuthenticatorFactory<T> : 
        IAuthenticatorFactory
            where T : class, IAuthenticatorConfiguration
    {
        /// <summary>   Constructs the http context inspecting authenticator. </summary>
        /// <param name="config">   The specialized configuration as specified in the class definition. </param>
        /// <returns>   An authenticator instance. </returns>
        IAuthenticator<T> Construct(T config);
    }
}
