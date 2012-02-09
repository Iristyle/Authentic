using System;
using System.Web.Management;

namespace EPS.Web.Management
{
    /// <summary>   Authentication success event. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class AuthenticationSuccessEvent : WebAuthenticationSuccessAuditEvent
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="sender">   Source of the event. </param>
        /// <param name="userName"> The username. </param>
        public AuthenticationSuccessEvent(object sender, string userName)
            : base(EPS.Web.Properties.ManagementStrings.AuthenticationSuccess, sender, EventCodes.AuthenticationSuccess, userName)
        { }
    }
}
