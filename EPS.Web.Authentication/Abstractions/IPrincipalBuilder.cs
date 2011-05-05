using System.Security.Principal;
using System.Web;
using System.Web.Security;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Abstractions
{
    /// <summary>   Interface that defines the operations of a principal builder. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public interface IPrincipalBuilder
    {
        /// <summary>   Gets the human-friendly name. </summary>
        /// <value> The name. </value>
        string Name { get; }

        /// <summary>   
        /// Gets the configuration defining the <see cref="T:System.Web.Security.MembershipProvider"/> and <see cref="T:
        /// EPS.Web.Abstractions.IPrincipalBuilderFactory"/> to use. 
        /// </summary>
        /// <value> The configuration. </value>
        IAuthenticatorConfiguration Configuration { get; }
        
        /// <summary>	Construct an IPrincipal given a username and password / MembershipUser (context thrown in for good measure). </summary>
        /// <param name="context">			The context. </param>
        /// <param name="membershipUser">	The membership user extracted if there was a MembershipProvider specified through configuration.  May be null. </param>
        /// <param name="userName">			The username. </param>
        /// <param name="password">			The password. </param>
        /// <returns>	An IPrincipal if the given credentials could be authenticated, otherwise null. </returns>
        IPrincipal ConstructPrincipal(HttpContextBase context, MembershipUser membershipUser, string userName, string password);
    }
}