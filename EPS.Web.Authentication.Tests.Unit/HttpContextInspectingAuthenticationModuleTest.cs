using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeItEasy;
using Xunit;
using System.Web;

namespace EPS.Web.Authentication.Tests.Unit
{
    public class HttpContextInspectingAuthenticationModuleTest
    {
        private readonly HttpContextBase httpContext = A.Fake<HttpContextBase>();
        private readonly HttpRequestBase httpRequest = A.Fake<HttpRequestBase>();
        private readonly HttpResponseBase httpResponse = A.Fake<HttpResponseBase>();

        public HttpContextInspectingAuthenticationModuleTest()
        {
            A.CallTo(() => httpContext.Request).Returns(httpRequest);
            A.CallTo(() => httpContext.Response).Returns(httpResponse);
        }

        [Fact(Skip = "Ensure that request context was processed")]
        public void ReadsContext()
        {            
            //TODO: configure module appropriately
            var module = new HttpContextInspectingAuthenticationModule();
            module.OnAuthenticateRequest(httpContext);

            //test that stuff was read out, etc
        }

        [Fact(Skip = "Setup a test with a chain of context handlers")]
        public void CallsConfiguredHandlers()
        {
        }
    }
}
