using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace EPS.Web.Abstractions
{
    /// <summary>   A test-friendly abstract base class that implements <see cref="T:System.Web.IHttpAsyncHandler"/>. 
    /// 			Based on code from:
    /// 			<a href="http://weblogs.asp.net/rashid/archive/2009/03/12/unit-testable-httpmodule-and-httphandler.aspx" />
    /// </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public abstract class HttpAsyncHandlerBase : IHttpAsyncHandler
    {
        /// <summary>   Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance. </summary>
        /// <value> true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false. Default is false.</value>
        /// <returns>   true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false. Default is false.</returns>
        public virtual bool IsReusable
        {
            get { return false; }
        }

        /// <summary>   Process the request described by context. </summary>
        /// <param name="context">  The context. </param>
        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }
        /// <summary>   Process the request described by context. </summary>
        /// <param name="context">  The context. </param>
        public abstract void ProcessRequest(HttpContextBase context);

        /// <summary>   Initiates an asynchronous call to the HTTP handler. </summary>
        /// <param name="context">      An <see cref="T:System.Web.HttpContext" /> object that provides references to intrinsic server objects
        ///                             (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        /// <param name="cb">           is null, the delegate is not called. </param>
        /// <param name="extraData">    Any extra data needed to process the request. </param>
        /// <returns>   An <see cref="T:System.IAsyncResult" /> that contains information about the status of the process. </returns>
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return BeginProcessRequest(new HttpContextWrapper(context), cb, extraData);
        }

        /// <summary>   Initiates an asynchronous call to the HTTP handler. </summary>
        /// <param name="context">      An <see cref="T:System.Web.HttpContext" /> object that provides references to intrinsic server objects
        ///                             (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        /// <param name="cb">           The cb. </param>
        /// <param name="extraData">    Any extra data needed to process the request. </param>
        /// <returns>   An <see cref="T:System.IAsyncResult" /> that contains information about the status of the process. </returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cb", Justification = "Following IHttpAsyncHandler interface spec")]
        public abstract IAsyncResult BeginProcessRequest(HttpContextBase context, AsyncCallback cb, object extraData);
        
        /// <summary>   Provides an asynchronous process End method when the process ends. </summary>
        /// <param name="result">   An <see cref="T:System.IAsyncResult" /> that contains information about the status of the process. </param>
        public abstract void EndProcessRequest(IAsyncResult result);
    }
}