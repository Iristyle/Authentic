using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Common.Logging;
using EPS.Collections.Generic;
using EPS.Text;

namespace EPS.Web
{
    /// <summary>   A class that can take a HTTP digest auth header string and parse it into a DigestHeader object. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public static class HttpDigestAuthHeaderParser
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        /// <summary>   Try to extract HTTP digest auth header from a given string. </summary>
        /// <remarks>   ebrown, 3/28/2011. </remarks>
        /// <param name="verb">         The HTTP verb. </param>
        /// <param name="authHeader">   The incoming authorization header. </param>
        /// <param name="header">       [out] The header if it exists, otherwise null. </param>
        /// <returns>   true if it succeeds in extracting a header, false if it fails. </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Intent is to eat any exceptions that may occur")]
        public static bool TryExtractDigestHeader(HttpMethodNames verb, string authHeader, out DigestHeader header)
        {
            header = null;

            try
            {
                header = ExtractDigestHeader(verb, authHeader);
                return true;
            }
            catch (Exception)
            { }

            return false;
        }

        /// <summary>   Extracts a HTTP digest auth header from a given string, assigning the given HTTP verb. </summary>
        /// <remarks>   ebrown, 3/28/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the authHeader argument is null. </exception>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="Exception">                Thrown when exception. </exception>
        /// <param name="verb">         The HTTP verb. </param>
        /// <param name="authHeader">   The incoming authorization header. </param>
        /// <returns>   A new DigestHeader instance containing the relevant values as parsed from the header. </returns>
        public static DigestHeader ExtractDigestHeader(HttpMethodNames verb, string authHeader)
        {
            try
            {
                if (null == authHeader) { throw new ArgumentNullException("authHeader"); }
                if (!Enum.IsDefined(typeof(HttpMethodNames), verb)) { throw new ArgumentException("The verb specified is not valid", "verb"); }
                
                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Digest", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("AuthHeader cannot be null or empty OR does not start with Digest", "authHeader");
                }

                //lop off the "Digest " and then separate by commas per the spec
                //http://en.wikipedia.org/wiki/Digest_access_authentication
                // http://tools.ietf.org/html/rfc2617
                string keyValuePairs = authHeader.Substring(7);
                if (string.IsNullOrEmpty(keyValuePairs))
                {
                    throw new ArgumentException("Authorization header did not contain any data other than Digest");
                }

                var headerDictionary = keyValuePairs.Split(',')
                    .Select(pair =>
                    {
                        var splits = pair.Split('=');
                        return new { Key = splits[0].Trim().Trim('\"'), Value = splits[1].Trim().Trim('\"') };
                    }).ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.CurrentCultureIgnoreCase);

                //parse the values, supplying defaults as necessary
                var header = new DigestHeader()
                    {
                        Verb = verb,
                        ClientNonce = headerDictionary.GetValueOrDefault("cnonce", string.Empty),
                        Nonce = headerDictionary.GetValueOrDefault("nonce", string.Empty),
                        Opaque = headerDictionary.GetValueOrDefault("opaque", string.Empty),
                        QualityOfProtection = headerDictionary.GetValueOrDefault("qop", string.Empty).ToEnumFromEnumValue<DigestQualityOfProtectionType>(),
                        Realm = headerDictionary.GetValueOrDefault("realm", string.Empty),
                        RequestCounter = headerDictionary.ContainsKey("nc") ? (int?)Convert.ToInt32(headerDictionary["nc"], 16) : null,
                        Response = headerDictionary.GetValueOrDefault("response", string.Empty),
                        Uri = headerDictionary.GetValueOrDefault("uri", string.Empty),
                        UserName = headerDictionary.GetValueOrDefault("username", string.Empty)
                    };

                string empty = "* EMPTY *";
                log.Info(String.Format(CultureInfo.InvariantCulture,
                    "Authorization header contains username [{1}] / realm [{2}]{0}" +
                    "uri [{3}] / nonce [{4}] / nc (nonce request counter) [{5}] / cnonce (client nonce) [{6}]{0}" +
                    "qop [{7}] / response [{8}] / opaque [{9}]",
                    Environment.NewLine, header.UserName ?? empty, header.Realm ?? empty, header.Uri ?? empty,
                    header.Nonce ?? empty, header.RequestCounter, header.ClientNonce ?? empty,
                    header.QualityOfProtection, header.Response ?? empty, header.Opaque ?? empty));

                return header;
            }
            catch (Exception ex)
            {
                log.Warn("Unexpected error parsing header string", ex);
                throw;
            }
        }
    }
}