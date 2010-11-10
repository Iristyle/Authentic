using System;
using System.Web;

namespace EPS.Web.Abstractions
{
    /// <summary>   A unit-test friendly base class that implements <see cref="T:System.Web.IHttpHandler"/>
    /// Based on code from:
    /// <a href="http://weblogs.asp.net/rashid/archive/2009/03/12/unit-testable-httpmodule-and-httphandler.aspx" />
    /// </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public abstract class HttpHandlerBase : IHttpHandler
    {
        /// <summary>   Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance. </summary>
        /// <value> true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.  Default of false. </value>
        /// <returns>   true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false. Default of false.</returns>
        public virtual bool IsReusable
        {
            get { return false; }
        }

        /// <summary>   
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" />
        /// interface. 
        /// </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects
        ///                         (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        /// <summary>   
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"
        /// />interface. 
        /// </summary>
        /// <param name="context">  An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects
        ///                         (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public abstract void ProcessRequest(HttpContextBase context);
    }
}
