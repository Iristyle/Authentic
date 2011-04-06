using System;
using System.Globalization;
using System.Security.Cryptography;
using EPS.Security.Cryptography;

namespace EPS.Web.Authentication.Digest
{
    /// <summary>   
    /// The nonce that is returned by the server contains a public part and a private part. This class is responsible for creating the
    /// private part of the nonce. The private part is based on a timestamp, the ip-address of the client and a private key. 
    /// </summary>
    /// <remarks>   ebrown, 4/6/2011. </remarks>
    public class PrivateHashEncoder
    {
        private readonly string privateKey;
        private readonly HashAlgorithm algorithm = MD5.Create();

        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 4/6/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="privateKey">   The private key to be used in hash generation. </param>
        public PrivateHashEncoder(string privateKey)
        {
            if (null == privateKey) { throw new ArgumentNullException("privateKey"); }
            if (string.IsNullOrWhiteSpace(privateKey)) { throw new ArgumentException("privateKey must be non-empty", "privateKey"); }
            this.privateKey = privateKey;
        }

        /// <summary>   Encodes a millisecond value and ip address along with the given private key to be used in a nonce. </summary>
        /// <remarks>   ebrown, 4/6/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="dateTimeInMilliSecondsString"> The milliseconds as a string. </param>
        /// <param name="ipAddress">                    The ip address. </param>
        /// <returns>   A MD5 hashed string containing the above values. </returns>
        public string Encode(string dateTimeInMilliSecondsString, string ipAddress)
        {
            if (null == ipAddress) { throw new ArgumentNullException("ipAddress"); }
            if (string.IsNullOrWhiteSpace(ipAddress)) { throw new ArgumentException("value must be non-whitespace", "ipAddress"); }

            if (null == dateTimeInMilliSecondsString) { throw new ArgumentNullException("dateTimeInMilliSecondsString"); }
            if (string.IsNullOrWhiteSpace(dateTimeInMilliSecondsString)) { throw new ArgumentException("value must be non-whitespace", "dateTimeInMilliSecondsString"); }

            string stringToEncode = string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}", 
                dateTimeInMilliSecondsString, ipAddress, privateKey);
            
            return HashHelpers.SafeHash(algorithm, stringToEncode);
        }
    }
}
