using System;

namespace EPS.Web.Handlers
{
    /// <summary>   A HttpResponseType indicates the type of response that a client has requested. </summary>
    /// <remarks>   ebrown, 2/15/2011. </remarks>
    public enum HttpResponseType
    {
        /// <summary> Just an HTTP head should be sent to a client.  </summary>
        HeadOnly,
        /// <summary> A complete response (file) should be sent to the client.  </summary>
        Complete
    }
}
