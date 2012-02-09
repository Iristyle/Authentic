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
        /// <param name="userName"> The username. </param>
        public CredentialCacheHitEvent(object sender, string userName)
            : base(Properties.ManagementStrings.CacheHitFor + userName, sender, EventCodes.CacheHit)
        { }
    }
}
