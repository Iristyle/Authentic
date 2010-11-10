using System;
using System.Web;
using EPS.Web.Abstractions;

namespace EPS.Web
{
    /// <summary>   <see cref="T:System.Web.IHttpHandler"/> implementation that is able to use a simple lambda delegate to process a request.
    /// 			Inspired by Haacked:
    /// 			<a href="http://haacked.com/archive/2008/12/15/redirect-routes-and-other-fun-with-routing-and-lambdas.aspx" />
    /// </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class DelegateHttpHandler : HttpHandlerBase
    {
        /// <summary>   Constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="action">       The action used to implement IHttpHandler. </param>
        /// <param name="isReusable">   true if is reusable. </param>
        public DelegateHttpHandler(Action<HttpContextBase> action, bool isReusable)
        {
            IsReusable = isReusable;
            HttpHandlerAction = action;
        }

        /// <summary>   Gets or sets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance. </summary>
        /// <value> true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.  Default of false. </value>
        /// <returns>   true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false. Default of false. </returns>
        public new bool IsReusable { get; private set; }

        /// <summary>   Gets the delegate repsonsible for handling the request. </summary>
        /// <value> The http handler action. </value>
        public Action<HttpContextBase> HttpHandlerAction { get; private set; }

        /// <summary>   We process the request using the handler passed in to the constructor. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="context">  An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects
        ///                         (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public override void ProcessRequest(HttpContextBase context)
        {
            var action = HttpHandlerAction;
            if (action != null)
                action(context);
        }
    }
}
