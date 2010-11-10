using System;
using System.Web.Management;

namespace EPS.Web.Management
{
    /// <summary>   Credential cache add error event. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class CredentialCacheAddErrorEvent : WebErrorEvent
    {
        private Exception exception;

        /// <summary>   Constructor that calls the base constructor with EventCodes.CacheAdd. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="sender">       Source of the event. </param>
        /// <param name="username">     The username. </param>
        /// <param name="exception">    The exception. </param>
        public CredentialCacheAddErrorEvent(object sender, string username, Exception exception)
            : base("Failed to add credential identifier for: " + username, sender, EventCodes.CacheAdd, exception)
        {
            this.exception = exception;
        }

        /// <summary>   Format custom event details.  Calls the base method and then adds the exception on a new line.</summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="formatter">    The formatter. </param>
        public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            base.FormatCustomEventDetails(formatter);
            formatter.AppendLine(exception.ToString());
        }
    }
}
