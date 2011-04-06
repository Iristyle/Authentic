using System;

namespace EPS.Web.Authentication.Digest
{
    /// <summary>   
    /// This is an opaque value that is passed from server to client and back again. Implementing something random that is unique to a client
    /// is on a load balanced server farm is quite a difficult task, so we punt on that and use something simple.
    /// </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public static class Opaque
    {
        //TODO: 4-6-2011 opaque utilization / tracking is not currently implemented
        /// <summary> A user-replaceable opaque value used during digest authentication. </summary>
        public static Func<string> Current = () => "1234123412345678";
    }
}
