using System;
using System.Globalization;

namespace EPS.Web.Authentication.Digest
{
    /// <summary>   This class is responsible for parsing the date time stamp that is received from the client. </summary>
    /// <remarks>   ebrown, 4/6/2011. </remarks>
    public static class NonceTimestampParser
    {
        /// <summary>   Parses a string based nonce timestamp into a DateTime. </summary>
        /// <remarks>   ebrown, 4/6/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="nonceTimestamp">   The nonce time stamp. </param>
        /// <returns>   A Utc based DateTime represenation of a given nonce string. </returns>
        public static DateTime Parse(string nonceTimestamp)
        {
            if (null == nonceTimestamp) { throw new ArgumentNullException("nonceTimeStamp"); }
            if (string.IsNullOrWhiteSpace(nonceTimestamp)) { throw new ArgumentException("value must be non-whitespace", "nonceTimeStamp"); }

            double nonceTimeStampDouble;
            if (Double.TryParse(nonceTimestamp, NumberStyles.Float, CultureInfo.InvariantCulture, out nonceTimeStampDouble))
            {
                return DateTime.MinValue.AddMilliseconds(nonceTimeStampDouble);
            }

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The given nonce time stamp {0} was not valid", nonceTimestamp));
        }
    }
}
