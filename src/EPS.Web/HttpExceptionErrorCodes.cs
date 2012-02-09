using System;
using System.Globalization;

namespace EPS.Web
{
    /// <summary>   Http exception error codes used internally. </summary>
    /// <remarks>   ebrown, 2/9/2011. </remarks>
    internal static class HttpExceptionErrorCodes
    {
        /// <summary> The connection aborted </summary>
        public static readonly int ConnectionAborted = -2147014842; //equivalent of hex 80072746
    }
}
