using System;
using EPS.Text;

namespace EPS.Web
{
    /// <summary>   Http method definition names (i.e verbs) as defined in RFC2616 <a href="http://tools.ietf.org/html/rfc2616#section-9" />. </summary>
    /// <remarks>   Used when manually baking up HTTP request / responses. </remarks>
    [Flags]
    public enum HttpMethodNames
    {
        /// <summary> The GET method means retrieve whatever information (in the form of an entity) is identified by the Request-URI </summary>
        [EnumValue("GET")]
        Get = 1,
        
        /// <summary> The HEAD method is identical to GET except that the server MUST NOT return a message-body in the response. </summary>
        [EnumValue("HEAD")]
        Header = (1 << 1),
        
        /// <summary> The PUT method requests that the enclosed entity be stored under the supplied Request-URI. </summary>
        [EnumValue("PUT")]
        Put = (1 << 2),
        
        /// <summary> The POST method is used to request that the origin server accept the entity enclosed in the request as a new subordinate of the resource identified by the Request-URI in the Request-Line. </summary>
        [EnumValue("POST")]
        Post = (1 << 3),
        
        /// <summary> The DELETE method requests that the origin server delete the resource identified by the Request-URI. </summary>
        [EnumValue("DELETE")]
        Delete = (1 << 4),
        
        /// <summary> The TRACE method is used to invoke a remote, application-layer loop-back of the request message. </summary>
        [EnumValue("TRACE")]
        Trace = (1 << 5),
        
        /// <summary> The OPTIONS method represents a request for information about the communication options available on the request response chain identified by the Request-URI </summary>
        [EnumValue("OPTIONS")]
        Options = (1 << 6)
    }
}
