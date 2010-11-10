using System;
using System.Web;
using System.Web.Routing;

namespace EPS.Web.Routing
{
    /// <summary>   A set of extension methods that sit atop <see cref="T:System.Web.Routing.RouteCollection"/>
    /// 			Inspired by Haacked.
    /// 			<a href="http://haacked.com/archive/2008/12/15/redirect-routes-and-other-fun-with-routing-and-lambdas.aspx" />
    /// 			</summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class RouteCollectionExtensions
    {
        /// <summary>   A RouteCollection extension method that we need to redirect permanently. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="routes">       The routes to act on. </param>
        /// <param name="url">          The base URL to route from. </param>
        /// <param name="targetUrl">    The target URL to route to. </param>
        /// <returns>   A new Route representative of redirecting the routes. </returns>
        public static Route RedirectPermanently(this RouteCollection routes, string url, string targetUrl)
        {
            Route route = new Route(url, new DelegateRouteHandler(context => GetRedirectHandler(context, targetUrl, true)));
            routes.Add(route);
            return route;
        }

        private static IHttpHandler GetRedirectHandler(RequestContext context, string targetUrl, bool permanently)
        {
            if (targetUrl.StartsWith("~/"))
            {
                Route route = new Route(targetUrl.Substring(2), null);
                var vpd = route.GetVirtualPath(context, context.RouteData.Values);
                if (vpd != null)
                    targetUrl = "~/" + vpd.VirtualPath;
            }
            else if (targetUrl.StartsWith("/"))
            {
                Route route = new Route(targetUrl.Substring(1), null);
                var vpd = route.GetVirtualPath(context, context.RouteData.Values);
                if (null != vpd)
                    targetUrl = "/" + vpd.VirtualPath;
            }

            return new DelegateHttpHandler(httpContext =>
                {                    
                    if (permanently)
                    {
                        httpContext.Response.Status = "301 Moved Permanently";
                        httpContext.Response.StatusCode = 301;
                        httpContext.Response.AddHeader("Location", targetUrl);
                    }
                    else
                        httpContext.Response.Redirect(targetUrl, false);
                }, false);
        }
    }
}
