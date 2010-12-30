using System;
using System.Web.Management;

namespace EPS.Web.Management
{
    /// <summary>   Base class for Cache events. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public abstract class CacheEvents : WebAuditEvent
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="message">      The message. </param>
        /// <param name="sender">       Source of the event. </param>
        /// <param name="eventcode">    The eventcode. </param>
        protected CacheEvents(string message, object sender, int eventcode)
            : base(message, sender, eventcode)
        { }
    }
}
