using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    /// <summary>   
    /// Interface for the http context inspecting authentication failure handler factory.  Constructs a failure handler given configuration
    /// information. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IFailureHandlerFactory
    {
        /// <summary>   
        /// Constructs a <see cref="EPS.Web.Authentication.Abstractions.IFailureHandler"/>
        /// given configuration information. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of a failure handler. </returns>
        IFailureHandler Construct(IFailureHandlerConfiguration config);
    }

    /// <summary>   
    /// Generic version of the <see cref="EPS.Web.Authentication.Abstractions.IFailureHandlerFactory"/>
    /// interface for the http context inspecting authentication failure handler factory.  Constructs a failure handler given specialized
    /// configuration information. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IFailureHandlerFactory<T> : 
        IFailureHandlerFactory
        where T : class, IFailureHandlerConfiguration
    {
        /// <summary>   
        /// Constructs a generic IFailureHandler, which allows passing along the specific type of
        /// configuration information. 
        /// </summary>
        /// <param name="config">   The configuration. </param>
        /// <returns>   An instance of a failure handler. </returns>
        IFailureHandler<T> Construct(T config);
    }
}