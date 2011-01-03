using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    /// <summary>   
    /// Interface for the http context inspecting authentication failure handler factory.  Constructs a failure handler given configuration
    /// information. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IHttpContextInspectingAuthenticationFailureHandlerFactory
    {
        /// <summary>   
        /// Constructs a <see cref="EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandler"/>
        /// given configuration information. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of a failure handler. </returns>
        IHttpContextInspectingAuthenticationFailureHandler Construct(HttpContextInspectingAuthenticationFailureConfigurationSection config);
    }

    /// <summary>   
    /// Generic version of the <see cref="EPS.Web.Authentication.Abstractions.IHttpContextInspectingAuthenticationFailureHandlerFactory"/>
    /// interface for the http context inspecting authentication failure handler factory.  Constructs a failure handler given specialized
    /// configuration information. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IHttpContextInspectingAuthenticationFailureHandlerFactory<T> : 
        IHttpContextInspectingAuthenticationFailureHandlerFactory
        where T : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        /// <summary>   
        /// Constructs a generic IHttpContextInspectingAuthenticationFailureHandler, which allows passing along the specific type of
        /// configuration information. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of a failure handler. </returns>
        IHttpContextInspectingAuthenticationFailureHandler<T> Construct(T config);
    }

}