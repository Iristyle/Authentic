using System;
using System.Web.Management;

namespace EPS.Web.Management
{
    /// <summary>   Authentication failure event. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class AuthenticationFailureEvent : WebAuthenticationFailureAuditEvent
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="sender">   Source of the event. </param>
        /// <param name="username"> The username. </param>
        public AuthenticationFailureEvent(object sender, string username)
            : base("Authentication failure", sender, EventCodes.AuthenticationFailure, username)
        { }
    }
}
