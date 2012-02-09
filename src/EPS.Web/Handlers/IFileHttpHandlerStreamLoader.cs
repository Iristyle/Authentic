using System;
using System.Web;

namespace EPS.Web.Handlers
{
    /// <summary>   
    /// Interface that defines the basic behavior of a class responsible for loading a local FileStream or cloud Uri based on an incoming
    /// request context. 
    /// </summary>
    /// <remarks>   ebrown, 2/14/2011. </remarks>
    public interface IFileHttpHandlerStreamLoader : IDisposable
    {
        /// <summary>   Parse a HttpRequestBase instance for details required to load a relevant FileStreamDetails instance. </summary>
        /// <param name="request">  The request. </param>
        /// <returns>   
        /// A FileStreamDetails instance noting whether or not the load was a success.  When successful, the Stream will be accessible OR an
        /// alternate Uri should be available in the CloudLocation member.  When unsuccessful, the Status member should have an error status
        /// provided. 
        /// </returns>
        StreamLoaderResult ParseRequest(HttpRequestBase request);
    }
}