using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Digest.Configuration
{
    public class DigestFailureHandlerConfiguration :
        FailureHandlerConfiguration, IDigestFailureHandlerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the FailureHandlerConfiguration class.
        /// </summary>
        public DigestFailureHandlerConfiguration(string realm, string privateKey, TimeSpan nonceValidDuration)
        {
            //TODO: 4-8-2011 -- used appropriate validator class to validate values
            Realm = realm;
            PrivateKey = privateKey;
            NonceValidDuration = nonceValidDuration;
        }

        /// <summary>   Gets or sets the realm of the cookie on an outgoing cookie request. </summary>
        /// <value> The realm. </value>
        public string Realm { get; set; }
        /// <summary>   Gets or sets the Private Key value used when generating nonce values. </summary>
        /// <value> The private key. </value>
        public string PrivateKey { get; set; }
        /// <summary>   Gets or sets the timespan that a nonce is valid. </summary>
        /// <value> The duration that the nonce is valid for. </value>
        public TimeSpan NonceValidDuration { get; set; }
    }
}
