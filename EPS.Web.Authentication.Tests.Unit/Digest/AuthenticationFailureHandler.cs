using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;
using FakeItEasy;
using Xunit;
using System.Collections.Specialized;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class AuthenticationFailureHandlerTest
    {
        [Fake] HttpContextBase context;
        [Fake] IAuthenticationFailureHandlerConfigurationSection configuration;
        string privateKey = "MyPrivateKey";
        string realm = "test@test.com";
        string ipAddress = "127.0.0.1";
        PrivateHashEncoder privateHashEncoder;
        AuthenticationFailureHandler failureHandler;

        Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult> inspectorResults 
            = new Dictionary<IHttpContextInspectingAuthenticator, InspectorAuthenticationResult>();

        public AuthenticationFailureHandlerTest()
        {
            Fake.InitializeFixture(this);
            //our classes guard against a null .ApplicationInstance
            A.CallTo(() => context.ApplicationInstance).Returns(null);
            A.CallTo(() => configuration.Realm).Returns(realm);
            A.CallTo(() => configuration.PrivateKey).Returns(privateKey);
            A.CallTo(() => configuration.NonceValidDuration).Returns(TimeSpan.FromSeconds(3));
            privateHashEncoder = new PrivateHashEncoder(privateKey);
            failureHandler = new AuthenticationFailureHandler(configuration);
        }

        private void FreezeNonceClock()
        {
            //freeze the clock 
            DateTime now = DateTime.UtcNow;
            NonceManager.Now = () => now;
        }

        private void ThawNonceClock()
        {
            NonceManager.Now = () => { return DateTime.UtcNow; };
        }


        [Fact]
        public void OnAuthenticationFailure_ThrowsOnNullContext()
        {
            var failureHandler = new AuthenticationFailureHandler(configuration);
            Assert.Throws<ArgumentNullException>(() => failureHandler.OnAuthenticationFailure(null, inspectorResults));
        }

        [Fact]
        public void OnAuthenticationFailure_GeneratesCorrectHeaderForNewRequest()
        {
            FreezeNonceClock();            
            A.CallTo(() => context.Request.HttpMethod).Returns("GET");
            A.CallTo(() => context.Request.UserHostAddress).Returns(ipAddress);

            //record the values from the AddHeader call and make sure they match our expectations
            string headerName = string.Empty, headerValue = string.Empty;
            A.CallTo(() => context.Response.AddHeader(A<string>.Ignored, A<string>.Ignored))
                .Invokes(call => { headerName = (string)call.Arguments[0]; headerValue = (string)call.Arguments[1]; });

            failureHandler.OnAuthenticationFailure(context, inspectorResults);

            string expectedHeader = String.Format(CultureInfo.InvariantCulture,
                "Digest realm=\"{0}\", nonce=\"{1}\", opaque=\"{2}\", stale=FALSE, algorithm=MD5, qop=\"auth\"",
                realm, NonceManager.Generate(ipAddress, privateHashEncoder), Opaque.Current());

            ThawNonceClock();            

            Assert.Equal(headerName, "WWW-Authenticate");
            Assert.Equal(expectedHeader, headerValue);
        }

        [Fact]
        public void OnAuthenticationFailure_RecognizesAndReportsStaleNonce()
        {
            string nonce = NonceManager.Generate(ipAddress, privateHashEncoder);
            A.CallTo(() => context.Request.HttpMethod).Returns("GET");
            A.CallTo(() => context.Request.UserHostAddress).Returns(ipAddress);

            var headers = new NameValueCollection() { { "Authorization", string.Format(
@"Digest username=""Mufasa"",realm=""testrealm@host.com"",
                     nonce=""{0}"",
                     uri=""/dir/index.html"",qop=auth,nc=00000001,cnonce=""0a4f113b"",
                     response=""6629fae49393a05397450978507c4ef1"",
                     opaque=""5ccc069c403ebaf9f0171e9517f40e41""", nonce) } };

            A.CallTo(() => context.Request.Headers).Returns(headers);

            //jump ahead just outside the reach of our configuration
            DateTime now = DateTime.UtcNow;
            NonceManager.Now = () => now + configuration.NonceValidDuration.Add(TimeSpan.FromSeconds(1));

            //record the values from the AddHeader call and make sure they match our expectations
            string headerName = string.Empty, headerValue = string.Empty;
            A.CallTo(() => context.Response.AddHeader(A<string>.Ignored, A<string>.Ignored))
                .Invokes(call => { headerName = (string)call.Arguments[0]; headerValue = (string)call.Arguments[1]; });

            failureHandler.OnAuthenticationFailure(context, inspectorResults);

            string expectedHeader = String.Format(CultureInfo.InvariantCulture,
                "Digest realm=\"{0}\", nonce=\"{1}\", opaque=\"{2}\", stale=TRUE, algorithm=MD5, qop=\"auth\"",
                realm, NonceManager.Generate(ipAddress, privateHashEncoder), Opaque.Current());

            ThawNonceClock();

            Assert.Equal(headerName, "WWW-Authenticate");
            Assert.Equal(expectedHeader, headerValue);
        }

        [Fact(Skip = "Simulate some other missing headers and make sure we handle properly")]
        public void OnAuthenticationFailure_HandlesBadHeaders()
        {
        }
    }
}
