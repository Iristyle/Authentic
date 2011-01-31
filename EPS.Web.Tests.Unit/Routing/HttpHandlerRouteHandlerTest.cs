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
                get { throw new NotImplementedException(); }
            }

            public void ProcessRequest(HttpContext context)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void GetHttpHandler_CreatesInstance()
        {
            var routeHandler = new HttpHandlerRouteHandler<TestHandler>();
            var handler = routeHandler.GetHttpHandler(A.Dummy<RequestContext>());

            Assert.True(null != handler && handler.GetType() == typeof(TestHandler));
        }

        [Fact]
        public void GetHttpHandler_ThrowsOnNullRequestContext()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpHandlerRouteHandler<TestHandler>().GetHttpHandler(null));
        }
    }
}
