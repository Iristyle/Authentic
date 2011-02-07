using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using EPS.Utility;
using EPS.Web.Routing;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class HttpHandlerExtensionsTest
    {
        class HttpHandlerTest : IHttpHandler
        {
            public bool IsReusable
            {
                get { return false; }
            }

            public void ProcessRequest(HttpContext context)
            {
                return;
            }
        }

        private static Route GetRouteInsertedToCollection(Action<RouteCollection> insert) //where T: IHttpHandler
        {
            var routes = new RouteCollection();
            insert(routes);            
            var insertedRoute = (Route)routes[0];
            return insertedRoute;
        }

        private GenericEqualityComparer<Route> routeComparer = new GenericEqualityComparer<Route>((route1, route2) =>
                {
                    return route1.Url == route2.Url && 
                        route1.Defaults.SequenceEqual(route2.Defaults) && 
                        route1.Constraints.SequenceEqual(route2.Constraints);
                });

        [Fact]
        public void MapHttpHandler_Throws_OnEmptyUrl()
        {
            var routes = new RouteCollection();
            Assert.Throws<ArgumentException>(() => routes.MapHttpHandler<HttpHandlerTest>(string.Empty));
        }

        [Fact]
        public void MapHttpHandler_NewRouteHandler_IsCorrectType()
        {
            var insertedRoute = GetRouteInsertedToCollection(routes => routes.MapHttpHandler<HttpHandlerTest>("route"));
            Assert.IsType<HttpHandlerRouteHandler<HttpHandlerTest>>(insertedRoute.RouteHandler);
        }

        [Fact]
        public void MapHttpHandler_NewRouteHandler_IsCorrectType_Overload()
        {
            var insertedRoute = GetRouteInsertedToCollection(routes => routes.MapHttpHandler<HttpHandlerTest>("name", "route-url", null, null));
            Assert.IsType<HttpHandlerRouteHandler<HttpHandlerTest>>(insertedRoute.RouteHandler);
        }

        [Fact]
        public void MapHttpHandler_NewRouteHandler_UrlMatches()
        {
            var url = "url";
            var insertedRoute = GetRouteInsertedToCollection(routes => routes.MapHttpHandler<HttpHandlerTest>(url));
            Assert.True(insertedRoute.Url == url);
        }

        [Fact]
        public void MapHttpHandler_NewRouteHandler_DefaultsAndConstraintsNotRequired()
        {
            var routes = new RouteCollection();
            routes.MapHttpHandler<HttpHandlerTest>("name", "route", null, null);
            Assert.True(routes.Count == 1);
        }

        [Fact]
        public void MapHttpHandler_NewRouteHandler_UrlMatches_Overload()
        {
            var routeUrl = "route-url";
            var insertedRoute = GetRouteInsertedToCollection(routes => routes.MapHttpHandler<HttpHandlerTest>("name", routeUrl, null, null));
            Assert.True(insertedRoute.Url == routeUrl);
        }

        [Fact]
        public void MapHttpHandler_NewRouteHandler_DefaultsAndConstraintsMatch()
        {
            var routes = new RouteCollection();
            routes.MapHttpHandler<HttpHandlerTest>("name", "url/{id}", new { id = 12 }, new { id = @"\d+" });
            var insertedRoute = (Route)routes[0];

            var matchingRoute = new Route("url/{id}", new RouteValueDictionary(new { id = 12 }), new RouteValueDictionary(new { id = @"\d+" }), new HttpHandlerRouteHandler<HttpHandlerTest>());

            Assert.Equal(insertedRoute, matchingRoute, routeComparer);
        }

        [Fact]
        public void MapHttpHandler_ThrowsOnNullRoutes()
        {
            Assert.Throws<ArgumentNullException>(() => (null as RouteCollection).MapHttpHandler<HttpHandlerTest>("test"));
        }

        [Fact]
        public void MapHttpHandler_ThrowsOnNullUrl()
        {
            Assert.Throws<ArgumentNullException>(() => new RouteCollection().MapHttpHandler<HttpHandlerTest>(null));
        }

        [Fact]
        public void MapHttpHandler_ThrowsOnNullRoutes2()
        {
            Assert.Throws<ArgumentNullException>(() => (null as RouteCollection).MapHttpHandler<HttpHandlerTest>("test", "url", null, null));
        }

        [Fact]
        public void MapHttpHandler_ThrowsOnNullUrl2()
        {
            Assert.Throws<ArgumentNullException>(() => new RouteCollection().MapHttpHandler<HttpHandlerTest>("name", null, null, null));
        }
    }
}