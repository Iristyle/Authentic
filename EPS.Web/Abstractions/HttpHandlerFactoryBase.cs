using System;
using System.Web;

namespace EPS.Web.Abstractions
{
    /// <summary>   A unit-testing friendly base class that implements <see cref="T:System.Web.IHttpHandlerFactory"/>.
    /// 			Based on concepts from:
    /// 			<a href="http://weblogs.asp.net/rashid/archive/2009/03/12/unit-testable-httpmodule-and-httphandler.aspx" />
    /// </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public abstract class HttpHandlerFactoryBase : IHttpHandlerFactory
    {
        /// <summary>   Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler" /> interface. </summary>
        /// <param name="context">          An instance of the <see cref="T:System.Web.HttpContext" /> class that provides references to
        ///                                 intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP
        ///                                 requests. </param>
        /// <param name="requestType">      The HTTP data transfer method (GET or POST) that the client uses. </param>
        /// <param name="url">              The <see cref="P:System.Web.HttpRequest.RawUrl" /> of the requested resource. </param>
        /// <param name="pathTranslated">   The <see cref="P:System.Web.HttpRequest.PhysicalApplicationPath" /> to the requested resource. </param>
        /// <returns>   A new <see cref="T:System.Web.IHttpHandler" /> object that processes the request. </returns>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return GetHandler(new HttpContextWrapper(context), requestType, url, pathTranslated);
        }

        /// <summary>   Returns an instance of a class that implements the <see cref="T:System.Web.IHttpHandler" /> interface. </summary>
        /// <param name="context">          An instance of the <see cref="T:System.Web.HttpContext" /> class that provides references to
        ///                                 intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP
        ///                                 requests. </param>
        /// <param name="requestType">      The HTTP data transfer method (GET or POST) that the client uses. </param>
        /// <param name="url">              The <see cref="P:System.Web.HttpRequest.RawUrl" /> of the requested resource. </param>
        /// <param name="pathTranslated">   The <see cref="P:System.Web.HttpRequest.PhysicalApplicationPath" /> to the requested resource. </param>
        /// <returns>   A new <see cref="T:System.Web.IHttpHandler" /> object that processes the request. </returns>
        public abstract IHttpHandler GetHandler(HttpContextBase context, string requestType, string url, string pathTranslated);

        /// <summary>   Enables a factory to reuse an existing handler instance. </summary>
        /// <param name="handler">  The <see cref="T:System.Web.IHttpHandler" /> object to reuse. </param>
        public abstract void ReleaseHandler(IHttpHandler handler);
    }
}
