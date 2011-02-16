using System;
using System.Globalization;
using EPS.Text;

namespace EPS.Web
{
    /// <summary>   
    /// Represents a HTTP range request made by a client.  The client has specified a start and an end, within the context of a maximum
    /// possible range. 
    /// </summary>
    /// <remarks>   ebrown, 2/9/2011. </remarks>
    public class RangeRequest
    {
        private string _multipartIntermediateHeader = null;

        /// <summary>   Constructs an instance of a RangeRequest. </summary>
        /// <remarks>   ebrown, 2/9/2011. </remarks>
        /// <param name="start">       The position at which to start the range request. </param>
        /// <param name="end">         The position at which to end the range request. </param>
        /// <param name="maximum">    The maximum size of the range thats available. </param>
        public RangeRequest(long start, long end, long maximum)
        {
            this.Start = start;
            this.End = end;
            this.Maximum = maximum;

            if (start > end) { throw new ArgumentOutOfRangeException("start", "start must be less than end"); }
            if (start < 0) { throw new ArgumentOutOfRangeException("start", "start must be 0 or greater"); }
            if (end >= maximum) { throw new ArgumentOutOfRangeException("end", "end must be less than maximum size of the range"); }
        }
        
        /// <summary> Gets the position at which to start the range request. </summary>
        public long Start { get; private set; }

        /// <summary> Gets the position at which to end the range request. </summary>
        public long End { get; private set; }

        /// <summary>   Gets the size of the complete range to which this range request is a sub-rangeRequest. </summary>
        public long Maximum { get; private set; }

        /// <summary>   Gets or builds a multipart header from the range request that can used in HTTP responses. </summary>
        /// <remarks>   ebrown, 2/14/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when contentType is null. </exception>
        /// <exception cref="ArgumentException">        Thrown when contentType is whitespace only. </exception>
        /// <param name="contentType">  Mime type of the content - can be matched from a file extension with the MimeTypes class. </param>
        /// <returns>   A string representing a multipart intermediate header. </returns>
        public string GetMultipartIntermediateHeader(string contentType)
        {
            if (null == contentType) { throw new ArgumentNullException("contentType"); }
            if (string.IsNullOrWhiteSpace(contentType)) { throw new ArgumentException("A contentType cannot be empty", "contentType"); }

            if (null == this._multipartIntermediateHeader)
            {
                this._multipartIntermediateHeader = String.Format(CultureInfo.InvariantCulture, "--{0}{1}", MultipartNames.MultipartBoundary, Environment.NewLine)
                    + String.Format(CultureInfo.InvariantCulture, "{0}: {1}{2}", HttpHeaderFields.ContentType.ToEnumValueString(), contentType, Environment.NewLine)
                    + String.Format(CultureInfo.InvariantCulture, "{0}: bytes {1}-{2}/{3}{4}{4}", HttpHeaderFields.ContentRange.ToEnumValueString(), Start, End, Maximum, Environment.NewLine);
            }

            return this._multipartIntermediateHeader;
        }
    }
}