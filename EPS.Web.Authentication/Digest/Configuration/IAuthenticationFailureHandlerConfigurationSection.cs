using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Digest.Configuration
{
    /// <summary>   Interface for authentication failure handler configuration section. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public interface IAuthenticationFailureHandlerConfigurationSection : 
        IHttpContextInspectingAuthenticationFailureConfigurationSection
    {
        /// <summary>   Gets or sets the realm of the cookie on an outgoing cookie request. </summary>
        /// <value> The realm. </value>
        string Realm { get; set; }

        /// <summary>   Gets or sets the Private Key value used when generating nonce values. </summary>
        /// <value> The private key. </value>
        string PrivateKey { get; set; }

        /// <summary>   Gets or sets the timespan that a nonce is valid. </summary>
        /// <value> The duration that the nonce is valid for. </value>
        TimeSpan NonceValidDuration { get; set; }
    }
}
