using System;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web
{
    /// <summary>   HTTP header field definitions that can be used when manually baking up HTTP responses.  Defined in RFC2616 <a href="http://tools.ietf.org/html/rfc2616#section-14"/> </summary>
    /// <remarks>   ebrown, 1/28/2011. </remarks>
    public static class HttpHeaderFields
    {   
        /// <summary> The Accept request-header field can be used to specify certain media types which are acceptable for the response </summary>
        public static readonly string Accept = "Accept";

        /// <summary> The Accept-Charset request-header field can be used to indicate what character sets are acceptable for the response. </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Charset", Justification = "Charset is legit")]
        public static readonly string AcceptCharset = "Accept-Charset";

        /// <summary> The Accept-Encoding request-header field is similar to Accept, but restricts the content-codings (section 3.5) that are acceptable in the response. </summary>
        public static readonly string AcceptEncoding = "Accept-Encoding";

        /// <summary> The Accept-Language request-header field is similar to Accept, but restricts the set of natural languages that are preferred as a response to the request </summary>
        public static readonly string AcceptLanguage = "Accept-Language";

        /// <summary>  The Accept-Ranges response-header field allows the server to indicate its acceptance of range requests for a resource </summary>
        public static readonly string AcceptRanges = "Accept-Ranges";

        /// <summary> The The Age response-header field conveys the sender's estimate of the amount of time since the response (or its revalidation) was generated at the origin server.  </summary>
        public static readonly string Age = "Age";
        
        /// <summary> The Allow entity-header field lists the set of methods supported by the resource identified by the Request-URI. </summary>
        public static readonly string Allow = "Allow";

        /// <summary> A user agent that wishes to authenticate itself with a server--usually, but not necessarily, after receiving a 401 response--does so by including an Authorization request-header field with the request. </summary>
        public static readonly string Authorization = "Authorization";

        /// <summary> The Cache-Control general-header field is used to specify directives that MUST be obeyed by all caching mechanisms along the request/response chain. </summary>
        public static readonly string CacheControl = "Cache-Control";

        /// <summary> The Connection general-header field allows the sender to specify options that are desired for that particular connection and MUST NOT be communicated by proxies over further connections. </summary>
        public static readonly string Connection = "Connection";

        /// <summary> The Content-Encoding entity-header field is used as a modifier to the media-type.  </summary>
        public static readonly string ContentEncoding = "Content-Encoding";

        /// <summary> The Content-Language entity-header field describes the natural language(s) of the intended audience for the enclosed entity.  </summary>
        public static readonly string ContentLanguage = "Content-Language";

        /// <summary> The Content-Length entity-header field indicates the size of the entity-body, in decimal number of OCTETs, sent to the recipient or, in the case of the HEAD method, 
        /// 		  the size of the entity-body that would have been sent had the request been a GET. </summary>
        public static readonly string ContentLength = "Content-Length";

        /// <summary> The Content-Location entity-header field MAY be used to supply the resource location for the entity enclosed in the message when that entity is accessible from a location separate from the requested resource's URI. </summary>
        public static readonly string ContentLocation = "Content-Location";

        /// <summary> The Content-MD5 entity-header field, as defined in RFC 1864 [23], is an MD5 digest of the entity-body for the purpose of providing an end-to-end message integrity check (MIC) of the entity-body. </summary>
        public static readonly string ContentMD5 = "Content-MD5";

        /// <summary> The Content-Range entity-header is sent with a partial entity-body to specify where in the full entity-body the partial body should be applied. </summary>
        public static readonly string ContentRange = "Content-Range";

        /// <summary> The Content-Type entity-header field indicates the media type of the entity-body sent to the recipient or, in the case of the HEAD method, the media type that would have been sent had the request been a GET. </summary>
        public static readonly string ContentType = "Content-Type";

        /// <summary> The Date general-header field represents the date and time at which the message was originated, having the same semantics as orig-date in RFC 822. The field value is an HTTP-date, as described in section 3.3.1; it MUST be sent in RFC 1123 [8]-date format. </summary>
        public static readonly string Date = "Date";
        
        /// <summary> The ETag response-header field provides the current value of the entity tag for the requested variant. </summary>
        public static readonly string EntityTag = "ETag";

        /// <summary> The Expect request-header field is used to indicate that particular server behaviors are required by the client. </summary>
        public static readonly string Expect = "Expect";
        
        /// <summary> The Expires entity-header field gives the date/time after which the response is considered stale. </summary>
        public static readonly string Expires = "Expires";

        /// <summary> The From request-header field, if given, SHOULD contain an Internet e-mail address for the human user who controls the requesting user agent. </summary>
        public static readonly string From = "From";
        
        /// <summary> The Host request-header field specifies the Internet host and port number of the resource being requested, as obtained from the original URI given 
        /// 		  by the user or referring resource (generally an HTTP URL, as described in section 3.2.2). </summary>
        public static readonly string Host = "Host";

        /// <summary> The If-Match request-header field is used with a method to make it conditional. </summary>
        public static readonly string IfMatch = "If-Match";
        
        /// <summary> The If-Modified-Since request-header field is used with a method to make it conditional: if the requested variant has not been modified since the time 
        /// 		  specified in this field, an entity will not be returned from the server; instead, a 304 (not modified) response will
        /// 		  be returned without any message-body. </summary>
        public static readonly string IfModifiedSince = "If-Modified-Since";

        /// <summary>  The If-None-Match request-header field is used with a method to make it conditional. </summary>
        public static readonly string IfNoneMatch = "If-None-Match";

        /// <summary> If a client has a partial copy of an entity in its cache, and wishes to have an up-to-date copy of the entire entity in its cache, it
        /// could use the Range request-header with a conditional GET (using either or both of If-Unmodified-Since and If-Match.)</summary>
        public static readonly string IfRange = "If-Range";

        /// <summary> The If-Unmodified-Since request-header field is used with a method to make it conditional. </summary>
        public static readonly string IfUnmodifiedSince = "If-Unmodified-Since";

        /// <summary> The Last-Modified entity-header field indicates the date and time at which the origin server believes the variant was last modified. </summary>
        public static readonly string LastModified = "Last-Modified";

        /// <summary> The Location response-header field is used to redirect the recipient to a location other than the Request-URI for completion of the
        /// request or identification of a new resource. </summary>
        public static readonly string Location = "Location";

        /// <summary> The Max-Forwards request-header field provides a mechanism with the TRACE (section 9.8) and OPTIONS (section 9.2) methods to limit the
        /// 		  number of proxies or gateways that can forward the request to the next inbound server </summary>
        public static readonly string MaxForwards = "Max-Forwards";

        /// <summary> The Pragma general-header field is used to include implementation- specific directives that might apply to any recipient along the
        /// request/response chain </summary>
        public static readonly string Pragma = "Pragma";

        /// <summary> The Proxy-Authenticate response-header field MUST be included as part of a 407 (Proxy Authentication Required) response.</summary>
        public static readonly string ProxyAuthenticate = "Proxy-Authenticate";

        /// <summary> The Proxy-Authorization request-header field allows the client to identify itself (or its user) to a proxy which requires authentication. </summary>
        public static readonly string ProxyAuthorization = "Proxy-Authorization";

        /// <summary> Since all HTTP entities are represented in HTTP messages as sequences of bytes, the concept of a byte range is meaningful for any HTTP
        /// entity. </summary>
        public static readonly string Range = "Range";

        /// <summary> The Referer[sic] request-header field allows the client to specify,for the server's benefit, the address (URI) of the resource from which the Request-URI was obtained (the "referrer", although the header field is misspelled.)  </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Referer", Justification  = "Misspelled in spec")]
        public static readonly string Referer = "Referer";

        /// <summary> The Retry-After response-header field can be used with a 503 (Service Unavailable) response to indicate how long the service is expected to be unavailable to the requesting client. </summary>
        public static readonly string RetryAfter = "Retry-After";

        /// <summary> The Server response-header field contains information about the software used by the origin server to handle the request. The field
        /// can contain multiple product tokens (section 3.8) and comments identifying the server and any significant subproducts. </summary>
        public static readonly string Server = "Server";

        /// <summary>  The TE request-header field indicates what extension transfer-codings it is willing to accept in the response and whether or not it is
        /// willing to accept trailer fields in a chunked transfer-coding. </summary>
        public static readonly string TE = "TE";

        /// <summary> The Trailer general field value indicates that the given set of header fields is present in the trailer of a message encoded with
        /// chunked transfer-coding. </summary>
        public static readonly string Trailer = "Trailer";

        /// <summary> The Transfer-Encoding general-header field indicates what (if any) type of transformation has been applied to the message body in order
        /// to safely transfer it between the sender and the recipient. This differs from the content-coding in that the transfer-coding is a
        /// property of the message, not of the entity. </summary>
        public static readonly string TransferEncoding = "Transfer-Encoding";

        /// <summary> The Upgrade general-header allows the client to specify what additional communication protocols it supports and would like to use
        /// if the server finds it appropriate to switch protocols. </summary>
        public static readonly string Upgrade = "Upgrade";

        /// <summary>  The User-Agent request-header field contains information about the user agent originating the request. </summary>
        public static readonly string UserAgent = "User-Agent";

        /// <summary> The Vary field value indicates the set of request-header fields that fully determines, while the response is fresh, whether a cache is
        /// permitted to use the response to reply to a subsequent request without revalidation.  </summary>
        public static readonly string Vary = "Vary";
        
        /// <summary> The Via general-header field MUST be used by gateways and proxies to indicate the intermediate protocols and recipients between the user
        /// agent and the server on requests, and between the origin server and the client on responses. </summary>
        public static readonly string Via = "Via";

        /// <summary> The Warning general-header field is used to carry additional information about the status or transformation of a message which might not be reflected in the message. </summary>
        public static readonly string Warning = "Warning";

        /// <summary> The WWW-Authenticate response-header field MUST be included in 401 (Unauthorized) response messages. </summary>
        public static readonly string WwwAuthenticate = "WWW-Authenticate";

        /// <summary> When the Keep-Alive connection-token has been transmitted with a request or a response, a Keep-Alive header field MAY also be included.  </summary>
        public static readonly string KeepAlive = "Keep-Alive";

        //TODO: this was previously defined -- but not sure why since its not in the W3C spec
        //public static readonly string UnlessModifiedSince = "Unless-Modified-Since";        
        // 
        // /// <summary> The accept ranges in bytes </summary>
        //public static readonly string AcceptRangesBytes = "bytes";
   }
}