using System;
using System.Web;

namespace EPS.Web.Handlers
{
    /// <summary>   An interface that describes how status information may be logged from a FileHttpHandler. </summary>
    /// <remarks>   ebrown, 2/14/2011. </remarks>
    public interface IFileHttpHandlerStatusLog
    {
        
        /// <summary>   Logs status information. </summary>
        /// <param name="context">  The context that may be inspected for . </param>
        /// <param name="status">   The status of a file transfer. </param>
        void Log(HttpContextBase context, StreamWriteStatus status);
        
        /// <summary>   Logs. </summary>
        /// <param name="context">  The context. </param>
        /// <param name="status">   The status of a file transfer. </param>
        /// <param name="message">  Additional text describing the status. </param>
        void Log(HttpContextBase context, StreamWriteStatus status, string message);
    }
}
