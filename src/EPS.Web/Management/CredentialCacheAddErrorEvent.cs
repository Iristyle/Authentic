using System;
using System.Web.Management;

namespace EPS.Web.Management
{
    /// <summary>   Credential cache add error event. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class CredentialCacheAddErrorEvent : WebErrorEvent
    {
        private readonly Exception _exception;

        /// <summary>   Constructor that calls the base constructor with EventCodes.CacheAdd. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="sender">       Source of the event. </param>
        /// <param name="userName">     The username. </param>
        /// <param name="exception">    The exception. </param>
        public CredentialCacheAddErrorEvent(object sender, string userName, Exception exception)
            : base(EPS.Web.Properties.ManagementStrings.FailedToAddCredentialIdentifierFor + userName, sender, EventCodes.CacheAdd, exception)
        {
            this._exception = exception;
        }

        /// <summary>   Format custom event details.  Calls the base method and then adds the exception on a new line. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="formatter">    The formatter. </param>
        public override void FormatCustomEventDetails(WebEventFormatter formatter)
        {
            if (null == formatter) { throw new ArgumentNullException("formatter"); }

            base.FormatCustomEventDetails(formatter);
            formatter.AppendLine(_exception.ToString());
        }
    }
}
