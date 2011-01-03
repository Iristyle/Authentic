using System;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Basic.Configuration;

namespace EPS.Web.Authentication.Basic
{
    /// <summary>   Interface that defines the operations of a basic authorization principal builder. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IBasicAuthPrincipalBuilder
    {
        /// <summary>   Gets the human-friendly name. </summary>
        /// <value> The name. </value>
        string Name { get; }

        /// <summary>   
        /// Gets the configuration defining the <see cref="T:System.Web.Security.MembershipProvider"/> and <see cref="T:
        /// EPS.Web.Authentication.Basic.IBasicAuthPrincipalBuilderFactory"/> to use. 
        /// </summary>
        /// <value> The configuration. </value>
        BasicAuthenticationHeaderInspectorConfigurationElement Configuration { get; }
        
        /// <summary>   Construct an IPrincipal given a username and password (context through in for good measure). </summary>
        /// <param name="context">  The context. </param>
        /// <param name="userName"> The username. </param>
        /// <param name="password"> The password. </param>
        /// <returns>   An IPrincipal if the given credentials could be authenticated, otherwise null. </returns>
        IPrincipal ConstructPrincipal(HttpContextBase context, string userName, string password);
    }
}
