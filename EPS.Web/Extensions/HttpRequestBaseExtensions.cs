using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using EPS.Conversions;
using EPS.Text;
using EPS.Web.Configuration;
using log4net;

namespace EPS.Web
{
    /// <summary>
    /// A simple set of extensions on top of the <see cref="T:System.Web.Abstractions.HttpRequestBase"/> class
    /// </summary>
    public static class HttpRequestBaseExtensions
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>   
        /// Determines whether the current browser is a mobile device (by built-in User-Agent sniffing) OR whether it has been configured as
        /// mobile indicated by the presence of a cookie. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="request">          The incoming request. </param>
        /// <param name="configuration">    An IMobileConfigurationSection to read settings from.  For testing purposes, instances may be faked,
        ///                                 otherwise use the appropriate xml config and register MobileConfigurationSection from new
        ///                                 ConfigurationManagerWrapper().GetSection&lt;MobileConfigurationSection&gt; in an IoC container. </param>
        /// <returns>   <c>true</c> if the device is mobile or has been configured as mobile with a cookie; otherwise, <c>false</c>. </returns>
        public static bool IsMobileDeviceOrConfiguredMobile(this HttpRequestBase request, IMobileConfiguration configuration)
        {
            if (null == request) { throw new ArgumentNullException("request"); }
            if (null == configuration) { throw new ArgumentNullException("configuration"); }

            var mobileCookie = request.Cookies.Get(configuration.OverrideCookie);
            return null == mobileCookie ? request.Browser.IsMobileDevice
                : mobileCookie.Value.ToBoolean(false);
        }

        /// <summary>   
        /// A HttpRequestBase extension method that takes extracts a list of HttpCookies from the request and converts them to a list of Cookie. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="request">  The request to act on. </param>
        /// <returns>   The cookies. </returns>
        public static IEnumerable<Cookie> GetCookies(this HttpRequestBase request)
        {
            if (null == request) { throw new ArgumentNullException("request"); }

            return request.Cookies.OfType<HttpCookie>().Select(c => c.ConvertToCookie());
        }

        /// <summary>   A HttpRequestBase extension method to query if range headers have been specified. </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <param name="request">  The request. </param>
        /// <returns>   true if range headers, false if not. </returns>
        public static bool HasRangeHeaders(this HttpRequestBase request)
        {
            return !string.IsNullOrWhiteSpace(request.RetrieveHeader(HttpHeaderFields.Range));
        }

        /// <summary>   A HttpRequestBase extension method that tries to a request for range requests. </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <param name="request">          The request. </param>
        /// <param name="fileSize">         Size of the file. </param>
        /// <param name="rangeRequests">    [out] The set of range requests (typically returned by IsRangeRequestValid). </param>
        /// <returns>   true if the headers are valid and parseable OR if there are not range requests specified, false if the range requests specified are invalid and cannot be satisfied. </returns>
        public static bool TryParseRangeRequests(this HttpRequestBase request, long fileSize, out IEnumerable<RangeRequest> rangeRequests)
        {
            string allRangeHeaders = request.RetrieveHeader(HttpHeaderFields.Range);

            //no range specified -- return entire file range
            if (string.IsNullOrWhiteSpace(allRangeHeaders))
            {
                rangeRequests = new[] { new RangeRequest(0, fileSize - 1, fileSize) };
                return true;
            }

            rangeRequests = allRangeHeaders
                .Replace("bytes=", string.Empty)
                .Split(new char[] { ',' })
                .Select(rangeHeader =>
                {
                    //split by a dash to get the requested start and end
                    int splitter = rangeHeader.IndexOf('-');
                    string startRange = rangeHeader.Substring(0, splitter), endRange = rangeHeader.Substring(splitter + 1);
                    long start = -1, end = -1;

                    //if no end, then file size -1 / otherwise, read it the end value
                    if (string.IsNullOrEmpty(endRange))
                    {
                        end = fileSize - 1;
                    }
                    else if (!long.TryParse(endRange, out end))
                    {
                        log.InfoFormat(CultureInfo.CurrentCulture, "Range Request End Value Invalid -- Specified : End [{0}] // File Size [{1}]", endRange, fileSize);
                        return null;
                    }

                    //no start, so end value indicates return the first n bytes of the file
                    if (string.IsNullOrEmpty(startRange))
                    {
                        start = fileSize - end - 1;
                        end = fileSize - 1;
                    }
                    //normal range value was indicated
                    else if (!long.TryParse(startRange, out start))
                    {
                        log.InfoFormat(CultureInfo.CurrentCulture, "Range Request Start Value Invalid -- Specified : Start [{0}] / End [{1}] // File Size [{2}]", startRange, end, fileSize);
                        return null;
                    }

                    /*
                    * NOTE:
                    * Do not clean invalid values up by fitting them into valid parameters using Math.Min and Math.Max, because
                    * some download clients (like Go!Zilla) might send invalid (e.g. too large) range requests to determine the file limits!
                    */

                    //can't start before, the file, or end after the file, or have an end before a start
                    if ((start < 0) || (end > (fileSize - 1)) || (end < start))
                    {
                        return null;
                    }

                    return new RangeRequest(start, end, fileSize);

                }).ToList();

            //if any of the constructed requests was not valid, this parsing is a FAIL
            return !rangeRequests.Any(r => r == null);
        }

        /// <summary>   
        /// A HttpRequestBase extension method that queries if a given request has a valid If-Range HTTP header or no If-Range HTTP header.  To
        /// be valid, the ETag must match an expected MD5 hash. 
        /// </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the request is null. </exception>
        /// <param name="request">  The request. </param>
        /// <param name="md5Hash">  The expected MD5 hash. </param>
        /// <returns>   true if the ETag / MD5 matches or there is no If-Range header, otherwise false . </returns>
        public static bool IfRangeHeaderIsValid(this HttpRequestBase request, string md5Hash)
        {
            if (null == request) { throw new ArgumentNullException("request"); }

            string header = request.RetrieveHeader(HttpHeaderFields.IfRange);
            return (string.IsNullOrWhiteSpace(header) || string.Equals(md5Hash, header.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>   
        /// A HttpRequestBase extension method that queries if a given request has a valid If-Match HTTP header or no If-Match HTTP header.  To
        /// be valid, the ETag must match an expected MD5 hash, or must be '*'. 
        /// </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the request is null. </exception>
        /// <param name="request">  The request. </param>
        /// <param name="md5Hash">  The expected MD5 hash. </param>
        /// <returns>   true if the ETag / MD5 matches or there is no If-Match header, otherwise false. </returns>
        public static bool IfMatchHeaderIsValid(this HttpRequestBase request, string md5Hash)
        {
            if (null == request) { throw new ArgumentNullException("request"); }

            //Retrieve If-Match Header value from Request (*, meaning any, if none is indicated)
            string requestHeaderIfMatch = request.RetrieveHeader(HttpHeaderFields.IfMatch);

            //The server may perform the request as if the If-Match header does not exists...
            if (string.IsNullOrWhiteSpace(requestHeaderIfMatch) || "*" == requestHeaderIfMatch) { return true; }

            //One or more Match Ids where sent by the client software...
            string[] entityIds = requestHeaderIfMatch.Replace("bytes=", string.Empty).Split(',');

            //Loop through all entity Ids, finding one which matches the current file's Id will be enough to satisfy the If-Match
            return entityIds.Any(entityId => string.Equals(md5Hash, entityId.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>   
        /// A HttpRequestBase extension method that queries if a given request has "*" specified for the If-None-Match HTTP header or no If-None-
        /// Match HTTP header is specified.  
        /// </summary>
        /// <remarks>   Clients should have . </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the request is null. </exception>
        /// <param name="request">  The request. </param>
        /// <returns>   true if the If-None-Match ETag / MD5 matches or there is no If-Match header, otherwise false. </returns>
        public static bool IfNoneMatchHeaderIsWildcard(this HttpRequestBase request)
        {
            if (null == request) { throw new ArgumentNullException("request"); }

            string requestHeaderIfNoneMatch = request.RetrieveHeader(HttpHeaderFields.IfNoneMatch);

            if (string.IsNullOrWhiteSpace(requestHeaderIfNoneMatch)) { return false; }

            //don't perform this request
            if ("*" == requestHeaderIfNoneMatch) { return true; }

            return false;
        }

        /// <summary>   A HttpRequestBase extension method that queries if a given request has an ETag specified that matches the given MD5 hash. </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the given request is null. </exception>
        /// <param name="request">  The request. </param>
        /// <param name="md5Hash">  The MD5 hash. </param>
        /// <returns>   true if the If-None-Match header has specified the given ETag OR if there is no If-None-Match header, otherwise false. </returns>
        public static bool IfNoneMatchHeaderHasMatchingETagSpecified(this HttpRequestBase request, string md5Hash)
        {
            if (null == request) { throw new ArgumentNullException("request"); }

            string requestHeaderIfNoneMatch = request.RetrieveHeader(HttpHeaderFields.IfNoneMatch);

            if (string.IsNullOrWhiteSpace(requestHeaderIfNoneMatch)) { return false; }

            //http://tools.ietf.org/html/rfc2068#section-14.26
            //One or more Match Ids where sent by the client software...
            return requestHeaderIfNoneMatch
                .Replace("bytes=", string.Empty)
                .Split(',')
                //Finding an entityId If-None-Match
                .Any(entityId => entityId == md5Hash);
        }

        /// <summary>   
        /// A HttpRequestBase extension method that queries if an If-Modified-Since HTTP header is specified and the given date is after the one
        /// found in the HTTP header (RFC 1123 format).
        /// </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <param name="request">          The request. </param>
        /// <param name="comparisonDate">   A Date/Time to compare against. </param>
        /// <returns>   true if the given date is after the one specified in the header OR there was no header, otherwise false. </returns>
        public static bool IfModifiedSinceHeaderIsBeforeGiven(this HttpRequestBase request, DateTime comparisonDate)
        {
            string headerDate = request.RetrieveHeader(HttpHeaderFields.IfModifiedSince);
            if (string.IsNullOrWhiteSpace(headerDate))
                return true;

            DateTime parsedDate;
            if (DateTime.TryParse(headerDate, out parsedDate))
            {
                return (comparisonDate >= parsedDate);
            }

            return false;
        }

        /// <summary>   
        /// A HttpRequestBase extension method that queries if the If-Unmodified-Since or the (unsupported?) Unless-Modified-Since header has a
        /// specified date greater than a particular given date (RFC 1123 format). 
        /// </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <param name="request">          The request. </param>
        /// <param name="comparisonDate">   A Date/Time to compare against. </param>
        /// <returns>   true if the given date is before the one specified in the header OR there was no header, otherwise false. </returns>
        public static bool IfUnmodifiedHeaderIsAfterGivenDate(this HttpRequestBase request, DateTime comparisonDate)
        {
            // Retrieve If-Unmodified-Since or Unless-Modified-Since, as appropriate
            string headerDate = request.RetrieveHeader(HttpHeaderFields.IfUnmodifiedSince);
            if (string.IsNullOrWhiteSpace(headerDate))
                headerDate = request.RetrieveHeader(HttpHeaderFields.UnlessModifiedSince);

            //no date, so we're fine
            if (string.IsNullOrWhiteSpace(headerDate)) { return true; }

            DateTime parsedDate;
            if (DateTime.TryParse(headerDate, out parsedDate))
            {
                //return true if the file was not modified since the indicated date... don't pull from the database, b/c the file stamp is the important thing here (a corrupt copy could have been replaced with a good one)
                return (comparisonDate < parsedDate);
            }

            //conversion failed
            return false;
        }


        /// <summary>   Retrieves a HTTP header, given a predefined header name from the HttpHeaderFields enumeration. </summary>
        /// <remarks>   ebrown, 2/10/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the given request is null. </exception>
        /// <exception cref="ArgumentException">        Thrown when the enumeration value passed is not defined or is some sort of bitmask. </exception>
        /// <param name="request">      The request. </param>
        /// <param name="headerName">   Name of the header.  Bitmasks are not accepted. </param>
        /// <returns>   The value stored in the given header with any "s totally removed, or string.Empty if null / missing / empty / whitespace. </returns>
        public static string RetrieveHeader(this HttpRequestBase request, HttpHeaderFields headerName)
        {
            if (null == request) { throw new ArgumentNullException("request"); }
            if (!Enum.IsDefined(typeof(HttpHeaderFields), headerName)) { throw new ArgumentException("passed enumeration value must be a defined value and cannot be a mask"); }

            string returnHeader = request.Headers[headerName.ToEnumValueString()];
            if (string.IsNullOrWhiteSpace(returnHeader))
                return string.Empty;

            return returnHeader.Replace("\"", string.Empty);
        }
    }
}
