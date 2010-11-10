using System;

namespace EPS.Web.Management
{
    /// <summary>   Credential cache hit event. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class CredentialCacheHitEvent : CacheEvents
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="sender">   Source of the event. </param>
        /// <param name="username"> The username. </param>
        public CredentialCacheHitEvent(object sender, string username)
            : base("Cache hit for: " + username, sender, EventCodes.CacheHit)
        { }
    }
}
