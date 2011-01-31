using System;

namespace EPS.Web
{
    /// <summary>   Http method definition names (i.e verbs) as defined in RFC2616 <a href="http://tools.ietf.org/html/rfc2616#section-9" />. </summary>
    /// <remarks>   Used when manually baking up HTTP request / responses. </remarks>
    public static class HttpMethodNames
    {
        /// <summary> The GET method means retrieve whatever information (in the form of an entity) is identified by the Request-URI </summary>
        public static readonly string Get = "GET";
        
        /// <summary> The HEAD method is identical to GET except that the server MUST NOT return a message-body in the response. </summary>
        public static readonly string Header = "HEAD";
        
        /// <summary> The PUT method requests that the enclosed entity be stored under the supplied Request-URI. </summary>
        public static readonly string Put = "PUT";
        
        /// <summary> The POST method is used to request that the origin server accept the entity enclosed in the request as a new subordinate of the resource identified by the Request-URI in the Request-Line. </summary>
        public static readonly string Post = "POST";
        
        /// <summary> The DELETE method requests that the origin server delete the resource identified by the Request-URI. </summary>
        public static readonly string Delete = "DELETE";
        
        /// <summary> The TRACE method is used to invoke a remote, application-layer loop-back of the request message. </summary>
        public static readonly string Trace = "TRACE";
        
        /// <summary> The OPTIONS method represents a request for information about the communication options available on the request response chain identified by the Request-URI </summary>
        public static readonly string Options = "OPTIONS";
    }
}
