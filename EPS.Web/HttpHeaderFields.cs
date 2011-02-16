using System;
using System.Diagnostics.CodeAnalysis;
using EPS.Text;

namespace EPS.Web
{
	/// <summary>   
	/// HTTP header field definitions that can be used when manually baking up HTTP responses.  Defined in RFC2616 <a href="http:
	/// //tools.ietf.org/html/rfc2616#section-14"/>.
	/// </summary>
	/// <remarks>   
	/// The values are separated out to non-overlapping integers, so that methods accepting HttpHeaderFields as a parameter, can either allow
	/// multiples, or enforce that only one well-defined value is passed. 
	/// </remarks>
	[Flags]
	[SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Values are intended to be entirely non-overlapping, and there are more than can be stored in a 32-bit int")]
	public enum HttpHeaderFields : long
	{   
		/// <summary> The Accept request-header field can be used to specify certain media types which are acceptable for the response </summary>
		[EnumValue("Accept")]
		Accept = 1,

		/// <summary> The Accept-Charset request-header field can be used to indicate what character sets are acceptable for the response. </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Charset", Justification = "Charset is legit")]
		[EnumValue("Accept-Charset")]
		AcceptCharset = (1L << 1),

		/// <summary> The Accept-Encoding request-header field is similar to Accept, but restricts the content-codings (section 3.5) that are acceptable in the response. </summary>
		[EnumValue("Accept-Encoding")]
		AcceptEncoding = (1L << 2),

		/// <summary> The AcceptCharset-Language request-header field is similar to Accept, but restricts the set of natural languages that are preferred as a response to the request </summary>
		[EnumValue("Accept-Language")]
		AcceptLanguage = (1L << 3),

		/// <summary>  The AcceptCharset-Ranges response-header field allows the server to indicate its acceptance of range requests for a resource </summary>
		[EnumValue("Accept-Ranges")]
		AcceptRanges = (1L << 4),

		/// <summary> The The Age response-header field conveys the sender's estimate of the amount of time since the response (or its revalidation) was generated at the origin server.  </summary>
		[EnumValue("Age")]
		Age = (1L << 5),
		
		/// <summary> The Allow entity-header field lists the set of methods supported by the resource identified by the Request-URI. </summary>
		[EnumValue("Allow")]
		Allow = (1L << 6),

		/// <summary> A user agent that wishes to authenticate itself with a server--usually, but not necessarily, after receiving a 401 response--does so by including an Authorization request-header field with the request. </summary>
		[EnumValue("Authorization")]
		Authorization = (1L << 7),

		/// <summary> The Cache-Control general-header field is used to specify directives that MUST be obeyed by all caching mechanisms along the request/response chain. </summary>
		[EnumValue("Cache-Control")]
		CacheControl = (1L << 8),

		/// <summary> The Connection general-header field allows the sender to specify options that are desired for that particular connection and MUST NOT be communicated by proxies over further connections. </summary>
		[EnumValue("Connection")]
		Connection = (1L << 9),

		/// <summary> The Content-Encoding entity-header field is used as a modifier to the media-type.  </summary>
		[EnumValue("Content-Encoding")]
		ContentEncoding = (1L << 10),

		/// <summary> The Content-Language entity-header field describes the natural language(s) of the intended audience for the enclosed entity.  </summary>
		[EnumValue("Content-Language")]
		ContentLanguage = (1L << 11),

		/// <summary> The Content-Length entity-header field indicates the size of the entity-body, in decimal number of OCTETs, sent to the recipient or, in the case of the HEAD method, 
		/// 		  the size of the entity-body that would have been sent had the request been a GET. </summary>
		[EnumValue("Content-Length")]
		ContentLength = (1L << 12),

		/// <summary> The ContentLength-Location entity-header field MAY be used to supply the resource location for the entity enclosed in the message when that entity is accessible from a location separate from the requested resource's URI. </summary>
		[EnumValue("Content-Location")]
		ContentLocation = (1L << 13),

		/// <summary> The Content-MD5 entity-header field, as defined in RFC 1864 [23], is an MD5 digest of the entity-body for the purpose of providing an end-to-end message integrity check (MIC) of the entity-body. </summary>
		[EnumValue("Content-MD5")]
		ContentMD5 = (1L << 14),

		/// <summary> The Content-Range entity-header is sent with a partial entity-body to specify where in the full entity-body the partial body should be applied. </summary>
		[EnumValue("Content-Range")]
		ContentRange = (1L << 15),

		/// <summary> The Content-Type entity-header field indicates the media type of the entity-body sent to the recipient or, in the case of the HEAD method, the media type that would have been sent had the request been a GET. </summary>
		[EnumValue("Content-Type")]
		ContentType = (1L << 16),

		/// <summary> The Date general-header field represents the date and time at which the message was originated, having the same semantics as orig-date in RFC 822. The field value is an HTTP-date, as described in section 3.3.1; it MUST be sent in RFC 1123 [8]-date format. </summary>
		[EnumValue("Date")]
		Date = (1L << 17),
		
		/// <summary> The ETag response-header field provides the current value of the entity tag for the requested variant. </summary>
		[EnumValue("ETag")]
		EntityTag = (1L << 18),

		/// <summary> The Expect request-header field is used to indicate that particular server behaviors are required by the client. </summary>
		[EnumValue("Expect")]
		Expect = (1L << 19),
		
		/// <summary> The Expires entity-header field gives the date/time after which the response is considered stale. </summary>
		[EnumValue("Expires")]
		Expires = (1L << 20),

		/// <summary> The From request-header field, if given, SHOULD contain an Internet e-mail address for the human user who controls the requesting user agent. </summary>
		[EnumValue("From")]
		From = (1L << 21),
		
		/// <summary> The Host request-header field specifies the Internet host and port number of the resource being requested, as obtained from the original URI given 
		/// 		  by the user or referring resource (generally an HTTP URL, as described in section 3.2.2). </summary>
		[EnumValue("Host")]
		Host = (1L << 22),

		/// <summary> The If-Match request-header field is used with a method to make it conditional. </summary>
		[EnumValue("If-Match")]
		IfMatch = (1L << 23),
		
		/// <summary> The If-Modified-Since request-header field is used with a method to make it conditional: if the requested variant has not been modified since the time 
		/// 		  specified in this field, an entity will not be returned from the server; instead, a 304 (not modified) response will
		/// 		  be returned without any message-body. </summary>
		[EnumValue("If-Modified-Since")]
		IfModifiedSince = (1L << 24),

		/// <summary>  The If-None-Match request-header field is used with a method to make it conditional. </summary>
		[EnumValue("If-None-Match")]
		IfNoneMatch = (1L << 25),

		/// <summary> If a client has a partial copy of an entity in its cache, and wishes to have an up-to-date copy of the entire entity in its cache, it
		/// could use the Range request-header with a conditional GET (using either or both of If-Unmodified-Since and If-Match.)</summary>
		[EnumValue("If-Range")]
		IfRange = (1L << 26),

		/// <summary> The If-Unmodified-Since request-header field is used with a method to make it conditional. </summary>
		[EnumValue("If-Unmodified-Since")]
		IfUnmodifiedSince = (1L << 27),

		/// <summary> When the Keep-Alive connection-token has been transmitted with a request or a response, a Keep-Alive header field MAY also be included.  </summary>
		[EnumValue("Keep-Alive")]
		KeepAlive = (1L << 28),

		/// <summary> The Last-Modified entity-header field indicates the date and time at which the origin server believes the variant was last modified. </summary>
		[EnumValue("Last-Modified")]
		LastModified = (1L << 29),

		/// <summary> The Location response-header field is used to redirect the recipient to a location other than the Request-URI for completion of the
		/// request or identification of a new resource. </summary>
		[EnumValue("Location")]
		Location = (1L << 30),

		/// <summary> The Max-Forwards request-header field provides a mechanism with the TRACE (section 9.8) and OPTIONS (section 9.2) methods to limit the
		/// 		  number of proxies or gateways that can forward the request to the next inbound server </summary>
		[EnumValue("Max-Forwards")]
		MaxForwards = (1L << 31),

		/// <summary> The Pragma general-header field is used to include implementation- specific directives that might apply to any recipient along the
		/// request/response chain </summary>
		[EnumValue("Pragma")]
		Pragma = (1L << 32),

		/// <summary> The Proxy-Authenticate response-header field MUST be included as part of a 407 (Proxy Authentication Required) response.</summary>
		[EnumValue("Proxy-Authenticate")]
		ProxyAuthenticate = (1L << 33),

		/// <summary> The Proxy-Authorization request-header field allows the client to identify itself (or its user) to a proxy which requires authentication. </summary>
		[EnumValue("Proxy-Authorization")]
		ProxyAuthorization = (1L << 34),

		/// <summary> Since all HTTP entities are represented in HTTP messages as sequences of bytes, the concept of a byte range is meaningful for any HTTP
		/// entity. </summary>
		[EnumValue("Range")]
		Range = (1L << 35),

		/// <summary> The Referer[sic] request-header field allows the client to specify,for the server's benefit, the address (URI) of the resource from which the Request-URI was obtained (the "referrer", although the header field is misspelled.)  </summary>
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Referer", Justification  = "Misspelled in spec")]
		[EnumValue("Referer")]
		Referer = (1L << 36),

		/// <summary> The Retry-After response-header field can be used with a 503 (Service Unavailable) response to indicate how long the service is expected to be unavailable to the requesting client. </summary>
		[EnumValue("Retry-After")]
		RetryAfter = (1L << 37),

		/// <summary> The Server response-header field contains information about the software used by the origin server to handle the request. The field
		/// can contain multiple product tokens (section 3.8) and comments identifying the server and any significant subproducts. </summary>
		[EnumValue("Server")]
		Server = (1L << 38),

		/// <summary>  The TE request-header field indicates what extension transfer-codings it is willing to accept in the response and whether or not it is
		/// willing to accept trailer fields in a chunked transfer-coding. </summary>
		[EnumValue("TE")]
		TE = (1L << 39),

		/// <summary> The Trailer general field value indicates that the given set of header fields is present in the trailer of a message encoded with
		/// chunked transfer-coding. </summary>
		[EnumValue("Trailer")]
		Trailer = (1L << 40),

		/// <summary> The Transfer-Encoding general-header field indicates what (if any) type of transformation has been applied to the message body in order
		/// to safely transfer it between the sender and the recipient. This differs from the content-coding in that the transfer-coding is a
		/// property of the message, not of the entity. </summary>
		[EnumValue("Transfer-Encoding")]
		TransferEncoding = (1L << 41),

		//TODO: this was previously defined -- but not sure why since its not in the W3C spec
		/// <summary> This header is not defined in the W3C spec</summary>
		[EnumValue("Unless-Modified-Since")]
		UnlessModifiedSince = (1L << 42),

		/// <summary> The Upgrade general-header allows the client to specify what additional communication protocols it supports and would like to use
		/// if the server finds it appropriate to switch protocols. </summary>
		[EnumValue("Upgrade")]
		Upgrade = (1L << 43),

		/// <summary>  The User-Agent request-header field contains information about the user agent originating the request. </summary>
		[EnumValue("User-Agent")]
		UserAgent = (1L << 44),

		/// <summary> The Vary field value indicates the set of request-header fields that fully determines, while the response is fresh, whether a cache is
		/// permitted to use the response to reply to a subsequent request without revalidation.  </summary>
		[EnumValue("Vary")]
		Vary = (1L << 45),
		
		/// <summary> The Via general-header field MUST be used by gateways and proxies to indicate the intermediate protocols and recipients between the user
		/// agent and the server on requests, and between the origin server and the client on responses. </summary>
		[EnumValue("Via")]
		Via= (1L << 46),

		/// <summary> The Warning general-header field is used to carry additional information about the status or transformation of a message which might not be reflected in the message. </summary>
		[EnumValue("Warning")]
		Warning = (1L << 47),

		/// <summary> The WWW-Authenticate response-header field MUST be included in 401 (Unauthorized) response messages. </summary>
		[EnumValue("WWW-Authenticate")]
		WwwAuthenticate = (1L << 48),

		// /// <summary> The accept ranges in bytes </summary>
		//public static readonly string AcceptRangesBytes = "bytes";
   }
}