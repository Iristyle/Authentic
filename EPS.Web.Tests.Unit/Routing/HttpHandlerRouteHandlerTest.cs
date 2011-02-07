using System;
using System.Web;
using System.Web.Routing;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Routing.Tests.Unit
{
    public class HttpHandlerRouteHandlerTest
    {
        class TestHandler : IHttpHandler
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

        private IHttpHandler CreateHandler()
        {
            var routeHandler = new HttpHandlerRouteHandler<TestHandler>();
            return routeHandler.GetHttpHandler(A.Dummy<RequestContext>());
        }

        [Fact]
        public void GetHttpHandler_CreatesInstanceOfCorrectType()
        {
            var handler = CreateHandler();
            Assert.IsType<TestHandler>(handler);
        }

        [Fact]
        public void GetHttpHandler_CreatesNonNullInstance()
        {
            var handler = CreateHandler();
            Assert.NotNull(handler);
        }

        [Fact]
        public void GetHttpHandler_ThrowsOnNullRequestContext()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpHandlerRouteHandler<TestHandler>().GetHttpHandler(null));
        }
    }
}