using System;
using System.Globalization;
using System.Net;
using System.Text;
using log4net;

namespace EPS.Web
{
    /// <summary>   A utility class for parsing a HTTP basic auth header. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class HttpBasicAuthHeaderParser
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>   Try to extract HTTP basic auth credentials from header. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="authHeader">   The incoming authorization header. </param>
        /// <param name="credentials">  [out] The credentials if they exist, otherwise null. </param>
        /// <returns>   true if it succeeds in extracting credentials, false if it fails. </returns>
        public static bool TryExtractCredentialsFromHeader(string authHeader, out NetworkCredential credentials)
        {
            credentials = null;

            try
            {
                credentials = ExtractCredentialsFromHeader(authHeader);
                return true;
            }
            catch (Exception)
            { }

            return false;
        }

        /// <summary>   Extracts and decodes the username / password credentials from the "Basic" HTTP header (as captured in authHeader). </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="Exception">            Thrown when exception. </exception>
        /// <param name="authHeader">   The incoming authorization header as a string. </param>
        /// <returns>   A NetworkCredential object that wraps up the credentials. </returns>
        public static NetworkCredential ExtractCredentialsFromHeader(string authHeader)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic", StringComparison.InvariantCulture))
                    throw new ArgumentException("AuthHeader cannot be null or empty OR does not start with Basic", "authHeader");
                
                // that's the right encoding -- use the auth header with "basic" stripped out
                string userPass = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(authHeader.Substring(6).Trim()));                
                if (string.IsNullOrEmpty(userPass))
                    throw new ArgumentException("Authorization header did not contain a base 64 encoded user:pass");                    

                string[] credentials = userPass.Split(new char[] { ':' }, 2);
                log.Info(String.Format(CultureInfo.InvariantCulture, "Authorization header contains user [{0}] / pass [{1}] selected", 
                    credentials[0] ?? "* EMPTY *", credentials[1] ?? "* EMPTY *"));
                return new NetworkCredential(credentials[0], credentials[1]);
            }
            catch (Exception ex)
            {
                log.Warn("Unexpected error parsing header string", ex);
                throw;
                //throw new HttpException("Invalid Authentication Header", ex);
            }
        }
    }
}
