using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Routing;
using EPS.Web.Abstractions;

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
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <param name="routes">       The routes to act on. </param>
		/// <param name="url">          The base URL to route from. </param>
		/// <param name="targetUrl">    The target URL to route to. </param>
		/// <returns>   A new Route representative of redirecting the routes. </returns>
		[SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~")]
		[SuppressMessage("Gendarme.Rules.Maintainability", "AvoidUnnecessarySpecializationRule", Justification = "We want to enforce usage against a RouteCollection rather than an ICollection<Route>")]
		public static Route RedirectPermanently(this RouteCollection routes, string url, string targetUrl)
		{
			if (null == routes) { throw new ArgumentNullException("routes"); }

			Route route = new Route(url, new DelegateRouteHandler(context => context.GetRedirectHandler(targetUrl, true)));
			routes.Add(route);
			return route;
		}

		private static HttpHandlerBase GetRedirectHandler(this RequestContext context, string targetUrl, bool permanently)
		{
			return new DelegateHttpHandler(httpContext =>
				{
					if (permanently)
					{
						httpContext.Response.Status = "301 Moved Permanently";
						httpContext.Response.StatusCode = 301;
						httpContext.Response.AddHeader("Location", context.GenerateTargetUrl(targetUrl, true));
					}
					else
					{
						httpContext.Response.Redirect(context.GenerateTargetUrl(targetUrl, false), false);
					}
				}, false);
		}

		private static string GenerateTargetUrl(this RequestContext context, string targetUrl, bool permanent)
		{
			if (targetUrl.StartsWith("~/", StringComparison.Ordinal))
			{
				Route route = new Route(targetUrl.Substring(2), null);
				var vpd = route.GetVirtualPath(context, context.RouteData.Values);
				if (vpd != null)
					return string.Format("{0}/{1}", permanent ? string.Empty : "~", vpd.VirtualPath);
			}
			else if (targetUrl.StartsWith("/", StringComparison.Ordinal))
			{
				Route route = new Route(targetUrl.Substring(1), null);
				var vpd = route.GetVirtualPath(context, context.RouteData.Values);
				if (null != vpd)
					return "/" + vpd.VirtualPath;
			}

			return targetUrl;
		}
	}
}