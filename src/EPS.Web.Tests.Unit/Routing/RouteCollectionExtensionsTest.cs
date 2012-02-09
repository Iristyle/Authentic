using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPS.Web.Routing;
using FakeItEasy;
using Xunit;
using System.Web.Routing;

namespace EPS.Web.Routing.Tests.Unit
{
    public class RouteCollectionExtensionsTest
    {
        [Fact(Skip = "Need to figure out how we verify the permanent redirect - might be necessary to integration test")]
        public void RedirectPermanently_ResolvesRedirectCorrectly()
        {
            var routes = new RouteCollection();
            routes.RedirectPermanently("http://www.test.com", "http://www.redirect.com");

            //http://haacked.com/archive/2007/12/17/testing-routes-in-asp.net-mvc.aspx
            
            //not sure that we *can* unit test this stuff
        }
    }
}
