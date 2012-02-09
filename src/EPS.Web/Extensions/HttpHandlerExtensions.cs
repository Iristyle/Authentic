using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Routing;

namespace EPS.Web.Routing
{
	/// <summary>   A set of extensions on RouteCollection that allows mapping a IHttpHandler to a given route. </summary>
	/// <remarks>   Shamelessly stolen from Haacked. <a href="http://haacked.com/archive/2009/11/04/routehandler-for-http-handlers.aspx" /> </remarks>
	public static class HttpHandlerExtensions
	{
		/// <summary>   
		/// A RouteCollection extension method that maps a url route to an IHttpHandler implementation.  If using <see cref="T:
		/// System.Web.Mvc.DependencyResolver"/>, prefer the MapHttpHandlerWithDependencyResolver in the EPS.Web.Mvc assembly as it allows using
		/// parameterized constructors. 
		/// </summary>
		/// <remarks>   ebrown, 1/28/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when routes or the specified url are null. </exception>
		/// <exception cref="ArgumentException">        Thrown when the url is empty or whitespace. </exception>
		/// <typeparam name="THandler"> Type of the handler implementing IHttpHandler. </typeparam>
		/// <param name="routes">   The routes collection to add to. </param>
		/// <param name="url">      URL to route from to the IHttpHandler. </param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is the API we want"),
		SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Following convention established by .NET framework with System.Web.Routing.Route")]
		public static void MapHttpHandler<THandler>(this RouteCollection routes, string url)
			where THandler : IHttpHandler, new()
		{
			if (null == routes) { throw new ArgumentNullException("routes"); }
			if (null == url) { throw new ArgumentNullException("url"); }
			if (string.IsNullOrWhiteSpace(url)) { throw new ArgumentException("must not be empty or whitespace", "url"); }

			MapHttpHandler<THandler>(routes, null, url, null, null);
		}

		/// <summary>   
		/// A RouteCollection extension method that maps a url route to an IHttpHandler implementation.  If using <see cref="T:
		/// System.Web.Mvc.DependencyResolver"/>, prefer the MapHttpHandlerWithDependencyResolver in the EPS.Web.Mvc assembly as it allows using
		/// parameterized constructors. 
		/// </summary>
		/// <remarks>   ebrown, 1/28/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when the routes or the specified url are null. </exception>
		/// <exception cref="ArgumentException">        Thrown when the url is empty or whitespace. </exception>
		/// <typeparam name="THandler"> Type of the handler implementing IHttpHandler. </typeparam>
		/// <param name="routes">       The routes collection to add to. </param>
		/// <param name="name">         The name, which can be null. </param>
		/// <param name="url">          URL to route from to the IHttpHandler. </param>
		/// <param name="defaults">     The defaults, which can be null. </param>
		/// <param name="constraints">  The constraints, which can be null. </param>
		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This is the API we want"),
		SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Justification = "Following convention established by .NET framework with System.Web.Routing.Route")]
		public static void MapHttpHandler<THandler>(this RouteCollection routes, string name, string url, object defaults, object constraints)
			where THandler : IHttpHandler, new()
		{
			if (null == routes) { throw new ArgumentNullException("routes"); }
			if (null == url) { throw new ArgumentNullException("url"); }
			if (string.IsNullOrWhiteSpace(url)) { throw new ArgumentException("must not be empty or whitespace", "url"); }

			var route = new Route(url, new HttpHandlerRouteHandler<THandler>())
			{
				Defaults = new RouteValueDictionary(defaults),
				Constraints = new RouteValueDictionary(constraints)
			};
			routes.Add(name, route);
		}
	}
}