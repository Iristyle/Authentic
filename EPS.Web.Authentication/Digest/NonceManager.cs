using System;
using System.Globalization;
using System.Text;

namespace EPS.Web.Authentication.Digest
{
    /// <summary>
    /// This class is responsible for generating a unique nonce based on a client IP address and our PrivateHashEncoder implementation.
    /// </summary>
    public static class NonceManager
    {
        private static Encoding encoding = Encoding.ASCII;
        private static Lazy<PrivateHashEncoder> privateHashEncoder = new Lazy<PrivateHashEncoder>(() =>
            {
                if (null == PrivateHashEncoder.Current) { throw new InvalidOperationException("PrivateHashEncoder.Current has not been properly initialized"); }
                var privateHashEncoder = PrivateHashEncoder.Current();
                if (null == privateHashEncoder) { throw new InvalidOperationException("PrivateHashEncoder.Current has not been properly initialized"); }
                return privateHashEncoder;                
            });

        /// <summary>   
        /// Generates a base64 encoded nonce combining the current time, and another hashed value that contains a hash of the time, ip address
        /// and a private key. Milliseconds:PrivateHash where PrivateHash = Milliseconds:IP:PrivateKey
        /// </summary>
        /// <remarks>   ebrown, 4/6/2011. </remarks>
        /// <exception cref="InvalidOperationException">    Thrown when the PrivateHashEncoder.Current has not been properly initialized. </exception>
        /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException">            Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="ipAddress">    The IP address. </param>
        /// <returns>   A base64 nonce value representing time:privateValue. </returns>
        public static string Generate(string ipAddress)
        {
            if (null == ipAddress) { throw new ArgumentNullException("ipRange"); }
            if (string.IsNullOrWhiteSpace(ipAddress)) { throw new ArgumentException("must not be empty", "ipRange"); };

            string dateTimeInMilliSecondsString = Math.Truncate((DateTime.UtcNow - DateTime.MinValue)
                .TotalMilliseconds).ToString(CultureInfo.InvariantCulture);
            string privateHash = privateHashEncoder.Value.Encode(dateTimeInMilliSecondsString, ipAddress);
            return Convert.ToBase64String(encoding.GetBytes(
                string.Format(CultureInfo.InvariantCulture, "{0}:{1}", dateTimeInMilliSecondsString, privateHash)));
        }

        /// <summary>   Given a nonce value, determines if it correctly applies to the given IP address. </summary>
        /// <remarks>   ebrown, 4/6/2011. </remarks>
        /// <exception cref="InvalidOperationException">    Thrown when the PrivateHashEncoder.Current has not been properly initialized. </exception>
        /// <exception cref="ArgumentNullException">        Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException">            Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="nonce">        The nonce. </param>
        /// <param name="ipAddress">    The IP address. </param>
        /// <returns>   true if it succeeds, false if it fails. </returns>
        public static bool Validate(string nonce, string ipAddress)
        {
            if (null == nonce) { throw new ArgumentNullException("nonce"); }
            if (string.IsNullOrWhiteSpace(nonce)) { throw new ArgumentException("must not be empty", "nonce"); };

            if (null == ipAddress) { throw new ArgumentNullException("ipRange"); }
            if (string.IsNullOrWhiteSpace(ipAddress)) { throw new ArgumentException("must not be empty", "ipRange"); };

            string[] decodedParts = GetDecodedParts(nonce);
            string md5EncodedString = privateHashEncoder.Value.Encode(decodedParts[0], ipAddress);
            return string.CompareOrdinal(decodedParts[1], md5EncodedString) == 0;
        }

        /// <summary>   Query if 'nonce' is stale based on an allowed number of seconds. </summary>
        /// <remarks>   ebrown, 4/6/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="nonce">        The nonce. </param>
        /// <param name="staleTimeOut"> The stale time out expressed as a TimeSpan. </param>
        /// <returns>   true if stale, false if not. </returns>
        public static bool IsStale(string nonce, TimeSpan staleTimeOut)
        {
            if (null == nonce) { throw new ArgumentNullException("nonce"); }
            if (string.IsNullOrWhiteSpace(nonce)) { throw new ArgumentException("must not be empty", "nonce"); };

            string[] decodedParts = GetDecodedParts(nonce);
            DateTime dateTimeFromNonce = NonceTimeStampParser.Parse(decodedParts[0]);
            return (dateTimeFromNonce + staleTimeOut) < DateTime.UtcNow;
        }

        private static string[] GetDecodedParts(string nonce)
        {
            return encoding.GetString(Convert.FromBase64String(nonce))
                .Split(':');
        }
    }
}