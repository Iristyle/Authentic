using System;
using System.Collections.Generic;
using System.Linq;

namespace EPS.Web.Handlers
{
    /// <summary>   A helper class responsible for examining HTTP request headers, mostly extensions on HttpRequestBase. </summary>
    /// <remarks>   ebrown, 2/10/2011. </remarks>
    public static class RangeRequestHelpers
    {
        /// <summary>   Determines if the set of specified RangeRequests represents a partial range or multiple range requests. </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when rangeRequests are null. </exception>
        /// <param name="rangeRequests">    The set of range requests (typically returned by TryParseRangeRequests). </param>
        /// <param name="fileLength">       Total length of the file to which the requests apply. </param>
        /// <returns>   true if a set of range requests or the single range request is for a partial range, false if not. </returns>
        public static bool IsPartialOrMultipleRangeRequests(IEnumerable<RangeRequest> rangeRequests, long fileLength)
        {
            if (null == rangeRequests) { throw new ArgumentNullException("rangeRequests"); }

            var first = rangeRequests.FirstOrDefault();
            return ((rangeRequests.Take(2).Count() > 1) || (0 != first.Start) || (fileLength != (first.End + 1)));
        }

        /// <summary>   Determines if the set of RangeRequests is representative of more than one range. </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the rangeRequests are null. </exception>
        /// <param name="rangeRequests">    The set of range requests (typically returned by TryParseRangeRequests). </param>
        /// <returns>   true if multipart request, false if not. </returns>
        public static bool IsMultipartRequest(IEnumerable<RangeRequest> rangeRequests)
        {
            if (null == rangeRequests) { throw new ArgumentNullException("rangeRequests"); }
            return (rangeRequests.Take(2).Count() > 1);
        }
    }
}