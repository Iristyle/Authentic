using System;

namespace EPS.Web.Management
{
    /// <summary>   Credential cache miss event. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class CredentialCacheMissEvent : CacheEvents
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="sender">   Source of the event. </param>
        /// <param name="userName"> The username. </param>
        public CredentialCacheMissEvent(object sender, string userName)
            : base(Properties.ManagementStrings.CacheMissFor + userName, sender, EventCodes.CacheHit)
        { }
    }
}
