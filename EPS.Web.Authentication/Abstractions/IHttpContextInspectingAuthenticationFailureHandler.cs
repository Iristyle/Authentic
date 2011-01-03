using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Abstractions
{
    /// <summary>   Interface for http context inspecting authentication failure handler. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IHttpContextInspectingAuthenticationFailureHandler
    {
        /// <summary>   Executes the authentication failure action. </summary>
        /// <param name="context">          The context. </param>
        /// <param name="inspectorResults"> The set of failed inspector results. </param>
        /// <returns>   An IPrincipal to use should all authentication attempts fail. </returns>
        IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults);
        
        /// <summary>   Gets the configuration. </summary>
        /// <value> The configuration. </value>
        HttpContextInspectingAuthenticationFailureConfigurationSection Configuration { get; }
    }

    /// <summary>   
    /// Generic version of the IHttpContextInspectingAuthenticationFailureHandler interface that allows specialization of configuration. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IHttpContextInspectingAuthenticationFailureHandler<T>
        : IHttpContextInspectingAuthenticationFailureHandler
        where T : HttpContextInspectingAuthenticationFailureConfigurationSection
    {
        /// <summary>   Executes the authentication failure action. </summary>
        /// <param name="context">          The context. </param>
        /// <param name="inspectorResults"> The set of failed inspector results. </param>
        /// <returns>   An IPrincipal to use should all authentication attempts fail. </returns>
        new IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults);
        
        /// <summary>   Gets the configuration. </summary>
        /// <value> The configuration. </value>
        new T Configuration { get; }
    }
}