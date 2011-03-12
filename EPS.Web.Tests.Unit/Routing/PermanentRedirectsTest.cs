using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using EPS.Web.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Routing.Tests.Unit
{
    //some testing concepts taken from
    //http://prashantbrall.wordpress.com/2010/08/18/mocking-routes/
    public class PermanentRedirectsTest
    {
        private RouteCollection routes = new RouteCollection();
        
        public PermanentRedirectsTest()
        {            
            IRoutingConfiguration config = A.Fake<IRoutingConfiguration>();
            A.CallTo(() => config.Enabled).Returns(true);
            var redirects = new Dictionary<string, RoutingRedirectConfigurationElement>()
            {
                { "FooBar", new RoutingRedirectConfigurationElement() { SourceUrl = "Foo", TargetUrl = "Bar"} },
                { "FooBar2", new RoutingRedirectConfigurationElement() { SourceUrl = "Foo2", TargetUrl = "~/Bar2"} },
                { "FooBar3", new RoutingRedirectConfigurationElement() { SourceUrl = "Foo3", TargetUrl = "/Bar3"} }
            };
            A.CallTo(() => config.PermanentRedirects).Returns(redirects);

            PermanentRedirects.Register(routes, config);
        }

        private static RequestContext FakeRequestContext(string url)
        {
            var requestContext = A.Fake<RequestContext>();
            var httpContextBase = A.Fake<HttpContextBase>();
            A.CallTo(() => requestContext.HttpContext).Returns(httpContextBase);
            var httpRequestBase = A.Fake<HttpRequestBase>();
            A.CallTo(() => httpContextBase.Request).Returns(httpRequestBase);
            A.CallTo(() => httpRequestBase.AppRelativeCurrentExecutionFilePath).Returns(url);
            var httpResponseBase = A.Fake<HttpResponseBase>();
            A.CallTo(() => httpContextBase.Response).Returns(httpResponseBase);
            return requestContext;
        }

        [Fact]
        public void PermanentRedirect_Registered_UsesDelegateRouteHandler()
        {
            //this doesn't prove anything about the right redirect location, just that we are using DelegateRouteHandler
            RouteData routeData = GetRouteDataForFakeRequest("~/Foo");
            Assert.IsType<DelegateRouteHandler>(routeData.RouteHandler);
        }

        [Fact]
        public void PermanentRedirect_Registered_UsesDelegateHttpHandlerInsideRouteHandler()
        {
            //this doesn't prove anything about the right redirect location, just that we are using DelegateHttpHandler
            RequestContext requestContext = FakeRequestContext("~/Foo");
            RouteData routeData = routes.GetRouteData(requestContext.HttpContext);
            
            Assert.IsType<DelegateHttpHandler>(routeData.RouteHandler.GetHttpHandler(requestContext));
        }

        private RouteData GetRouteDataForFakeRequest(string url)
        {
            RequestContext requestContext = FakeRequestContext(url);
            RouteData routeData = routes.GetRouteData(requestContext.HttpContext);
            return routeData;
        }

        private RequestContext ProcessFakeRequest(string url)
        {
            RequestContext requestContext = FakeRequestContext(url);
            RouteData routeData = routes.GetRouteData(requestContext.HttpContext);

            //we already knew we're using a DelegateHttpHandler, which is test friendly and can use HttpContextBase
            DelegateHttpHandler testHandler = (DelegateHttpHandler)routeData.RouteHandler.GetHttpHandler(requestContext);
            testHandler.ProcessRequest(requestContext.HttpContext);
            return requestContext;
        }

        [Fact]
        public void PermanentRedirect_Registered_WritesLocationToHeader()
        {
            RequestContext requestContext = ProcessFakeRequest("~/Foo");
            A.CallTo(() => requestContext.HttpContext.Response.AddHeader("Location", "Bar")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void PermanentRedirect_Registered_Writes301StatusCode()
        {
            RequestContext requestContext = ProcessFakeRequest("~/Foo");
            Assert.Equal(301, requestContext.HttpContext.Response.StatusCode);
        }

        [Fact]
        public void PermanentRedirect_Registered_Writes301MovePermanentlyStatus()
        {
            RequestContext requestContext = ProcessFakeRequest("~/Foo");
            Assert.Same("301 Moved Permanently", requestContext.HttpContext.Response.Status);
        }

        [Fact]
        public void PermanentRedirect_Registered_WritesLocationToHeaderForAppRelativeTarget()
        {
            //TODO: this is a bug - Location should not have a ~/ -- site relative urls only work in IIS, not a regular browser
            RequestContext requestContext = ProcessFakeRequest("~/Foo2");
            A.CallTo(() => requestContext.HttpContext.Response.AddHeader("Location", "/Bar2")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void PermanentRedirect_Registered_WritesLocationToHeaderForSiteRelativeTarget()
        {
            RequestContext requestContext = ProcessFakeRequest("~/Foo3");
            A.CallTo(() => requestContext.HttpContext.Response.AddHeader("Location", "/Bar3")).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void PermanentRedirect_NotRegistered()
        {
            RouteData routeData = GetRouteDataForFakeRequest("~/NotMapped");
            Assert.Null(routeData);
        }
    }
}
