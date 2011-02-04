using System;
using System.Web;
using System.Web.Routing;
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
                get { throw new NotImplementedException(); }
            }

            public void ProcessRequest(HttpContext context)
            {
                throw new NotImplementedException();
            }
        }

        private static Route GetRouteInsertedToCollection(Action<RouteCollection> insert) //where T: IHttpHandler
        {
            var routes = new RouteCollection();
            insert(routes);            
            var insertedRoute = (Route)routes[0];
            return insertedRoute;
        }
        [Fact]

        public void MapHttpHandler_AddsNewRouteToHandler()
        {
            var insertedRoute = GetRouteInsertedToCollection(routes => routes.MapHttpHandler<HttpHandlerTest>("url"));
            Assert.True(insertedRoute.RouteHandler.GetType() == typeof(HttpHandlerRouteHandler<HttpHandlerTest>) 
                && insertedRoute.Url == "url");
        }

        [Fact]
        public void MapHttpHandler_AddsNewRouteToHandlerWithNullDefaultsAndConstraints()
        {
            var insertedRoute = GetRouteInsertedToCollection(routes => routes.MapHttpHandler<HttpHandlerTest>("name", "route-url", null, null));
            Assert.True(insertedRoute.RouteHandler.GetType() == typeof(HttpHandlerRouteHandler<HttpHandlerTest>)
                && insertedRoute.Url == "route-url");
        }

        [Fact]
        public void MapHttpHandler_AddsNewRouteToHandlerWithCorrectDefaultsAndConstraints()
        {
            var routes = new RouteCollection();
            routes.MapHttpHandler<HttpHandlerTest>("name", "url/{id}", new { id = 12 }, new { id = @"\d+" });
            var insertedRoute = (Route)routes[0];
            Assert.True(insertedRoute.RouteHandler.GetType() == typeof(HttpHandlerRouteHandler<HttpHandlerTest>)
                && insertedRoute.Url == "url/{id}" && (int)insertedRoute.Defaults["id"] == 12 && (string)insertedRoute.Constraints["id"] == @"\d+" );
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