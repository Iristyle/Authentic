using System;

namespace EPS.Web
{
    /// <summary>   Multipart content-type as defined in RFC1341 <a href="http://www.w3.org/Protocols/rfc1341/7_2_Multipart.html" />. </summary>
    /// <remarks>   ebrown, 1/28/2011. </remarks>
    public static class MultipartNames
    {
        /// <summary>  The encapsulation boundary is defined as a line consisting entirely of two hyphen characters ("-", decimal code 45) followed by the boundary parameter value from the Content-Type header field. </summary>
        public static readonly string MultipartBoundary = "0000_1111_Multipart_Boundary_1111_0000";
        // The boundary is used in multipart/byteranges responses to separate each ranges content. It should be as unique as possible to avoid confusion with binary content.

        /// <summary> Type of the multipart content </summary>
        public static readonly string MultipartContentType = "multipart/byteranges; boundary=" + MultipartBoundary;
    }
}
