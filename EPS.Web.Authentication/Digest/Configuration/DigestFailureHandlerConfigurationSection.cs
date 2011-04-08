using System;
using System.Configuration;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Digest.Configuration
{
    /// <summary>   Digest authentication failure handler configuration section. </summary>
    /// <remarks>   ebrown, 12/30/2010. </remarks>
    public class DigestFailureHandlerConfigurationSection : 
        FailureHandlerConfigurationSection, 
        IFailureHandlerConfiguration
    {
        /// <summary>   Gets or sets the realm of the cookie on an outgoing cookie request. </summary>
        /// <value> The realm. </value>
        [ConfigurationProperty("realm", DefaultValue = "localhost")]
        [StringValidator(MinLength = 1)]
        public string Realm
        {
            get { return (string)this["realm"]; }
            set { base["realm"] = value; }
        }

        /// <summary>   Gets or sets the Private Key value used when generating nonce values.  Minimum length of 8 characters. </summary>
        /// <value> The private key. </value>
        [ConfigurationProperty("privateKey", IsRequired = true, DefaultValue = "privateKey")]
        [StringValidator(MinLength = 8)]
        public string PrivateKey
        {
            get { return (string)this["privateKey"]; }
            set { base["privateKey"] = value; }
        }

        /// <summary>   Gets or sets the timespan that a nonce is valid. </summary>
        /// <value> The duration that the nonce is valid for. </value>
        [ConfigurationProperty("nonceValidDuration", IsRequired = true, DefaultValue = "00:05:00")]
        [TimeSpanValidator(MinValueString = "00:00:20", MaxValueString = "00:60:00")]
        public TimeSpan NonceValidDuration
        {
            get { return (TimeSpan)this["nonceValidDuration"]; }
            set { base["nonceValidDuration"] = value; }
        }
    }
}