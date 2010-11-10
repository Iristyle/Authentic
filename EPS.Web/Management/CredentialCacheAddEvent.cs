using System;

namespace EPS.Web.Management
{
    /// <summary>   Credential cache add event. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class CredentialCacheAddEvent : CacheEvents
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="sender">   Source of the event. </param>
        /// <param name="username"> The username. </param>
        public CredentialCacheAddEvent(object sender, string username)
            : base("Credential identifier added for: " + username, sender, EventCodes.CacheAdd)
        { }
    }
}
