using System;
using System.Web;
using System.Web.Routing;

namespace EPS.Web.Routing
{
    /// <summary>   
    /// A custom IRouteHandler class that allows us to plug standard IHttpHandler instances into routing easier.  A similar class exists in
    /// the EPS.Web.Mvc assembly that integrates with <see cref="T:System.Web.Mvc.DependencyResolver"/> 
    /// </summary>
    /// <remarks>   Shamelessly stolen from Haacked. <a href="http://haacked.com/archive/2009/11/04/routehandler-for-http-handlers.aspx" /> </remarks>
    public class HttpHandlerRouteHandler<THandler> 
        : IRouteHandler where THandler : IHttpHandler, new()
    {
        /// <summary>   Returns a new instance of the specified IHttpHandler. </summary>
        /// <remarks>   ebrown, 1/28/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the requestContext is null. </exception>
        /// <param name="requestContext">   Context for the request. Unused, but must not be null. </param>
        /// <returns>   A new instance of the given THandler. </returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            if (null == requestContext) { throw new ArgumentNullException("requestContext"); }

            return new THandler();
        }
    }
}