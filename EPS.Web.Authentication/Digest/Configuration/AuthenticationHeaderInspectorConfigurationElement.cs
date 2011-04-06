using System;
using System.Configuration;
using EPS.Web.Authentication.Configuration;
using EPS.Web.Authentication.Utility;

namespace EPS.Web.Authentication.Digest.Configuration
{
    /// <summary>   
    /// Digest authentication header inspector configuration element that defines settings specific to digest authentication. 
    /// </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public class AuthenticationHeaderInspectorConfigurationElement : 
        HttpContextInspectingAuthenticatorConfigurationElement, 
        IAuthenticationHeaderInspectorConfigurationElement
    {
        protected override void PostDeserialize()
        {
            base.PostDeserialize();

            //simple verification of info supplied in config -- not used for anything (yet)
            var provider = MembershipProviderLocator.GetProvider(this.ProviderName);
            if (null != provider && !provider.EnablePasswordRetrieval)
            {
                throw new ConfigurationErrorsException("Provider " + this.ProviderName + " must support password retrieval to be used with Digest authentication");
            }
        }

        /// <summary>   Gets or sets the realm of the digest response on an outgoing 401 challenge. </summary>
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
        [TimeSpanValidator(MinValueString="00:00:20", MaxValueString="00:60:00")]
        public TimeSpan NonceValidDuration
        {
            get { return (TimeSpan)this["nonceValidDuration"]; }
            set { base["nonceValidDuration"] = value; }
        }
    }
}