using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class AuthenticationInspectingAuthenticatorTest : IDisposable
    {
        [Fake] readonly MembershipProvider fakeMembershipProvider;
        [Fake] MembershipUser fakeUser;
        [Fake] IAuthenticationHeaderInspectorConfigurationElement configuration;
        [Fake] HttpContextBase context;
        [Fake] IPrincipalBuilder principalBuilder;
        string privateKey = "MyPrivateKey";
        PrivateHashEncoder privateHashEncoder;

        public AuthenticationInspectingAuthenticatorTest()
        {
            Fake.InitializeFixture(this);

            A.CallTo(() => fakeUser.GetPassword()).Returns("Circle Of Life");
            A.CallTo(() => fakeMembershipProvider.Name).Returns("Fake");
            A.CallTo(() => fakeMembershipProvider.GetUser("Mufasa", A<Boolean>.Ignored)).Returns(fakeUser);
            Membership.Providers.AddMembershipProvider(fakeMembershipProvider);

            A.CallTo(() => configuration.Factory).Returns(typeof(AuthenticationInspectingAuthenticatorFactory).FullName);
            A.CallTo(() => configuration.ProviderName).Returns(fakeMembershipProvider.Name);
            A.CallTo(() => configuration.PrivateKey).Returns(privateKey);
            A.CallTo(() => configuration.GetPrincipalBuilder()).Returns(principalBuilder);
            A.CallTo(() => principalBuilder.ConstructPrincipal(A<HttpContextBase>.Ignored, A<string>.Ignored, A<string>.Ignored))
                .ReturnsLazily(call => new GenericPrincipal(new GenericIdentity(call.GetArgument<string>("userName")), new[] { "roles" }));

            privateHashEncoder = new PrivateHashEncoder(privateKey);

            Opaque.Current = () => "5ccc069c403ebaf9f0171e9517f40e41";
        }

        public void Dispose()
        {
            Membership.Providers.RemoveMembershipProvider("Fake");
        }

        [Fact]
        public void Authenticate_ThrowsOnNullContext()
        {
            var inspector = new AuthenticationInspectingAuthenticator(configuration);
            Assert.Throws<ArgumentNullException>(() => inspector.Authenticate(null));
        }

        [Fact]
        public void Authenticate_TrueOnValidMembership()
        {
            var inspector = new AuthenticationInspectingAuthenticator(configuration);

            string ipAddress = "127.0.0.1";
            string realm = "testrealm@host.com";
            
            //the result of MD5 hashing some well known values (either specified in the header below or similar)
            string response = "dc950f2d7c24037a6c775bcc9198b6f8";
            //939e7578ed9e3c518a452acee763bce9:NjM0Mzc3MjI2OTIwMDA6Yjg3ZWZlODM0Mjc1NThjZGVlZWVkYjRjNTI1MzFjMzM=:00000001:0a4f113b:auth:39aff3a2bab6126f332b942af96d3366

            NonceManager.Now = () => DateTime.Parse("4/6/2011 9:38:12 PM");

            string nonce = NonceManager.Generate(ipAddress, privateHashEncoder);
            //this should generate very specific nonce "NjM0Mzc3MjI2OTIwMDA6Yjg3ZWZlODM0Mjc1NThjZGVlZWVkYjRjNTI1MzFjMzM="

            A.CallTo(() => configuration.Realm).Returns(realm);

            var headers = new NameValueCollection() { { "Authorization", string.Format(
@"Digest username=""Mufasa"",realm=""{0}"",
                     nonce=""{1}"",
                     uri=""/dir/index.html"",qop=auth,nc=00000001,cnonce=""0a4f113b"",
                     response=""{2}"",
                     opaque=""{3}""", realm, nonce, response, Opaque.Current())} };

            
            A.CallTo(() => context.Request.Headers).Returns(headers);
            A.CallTo(() => context.Request.HttpMethod).Returns("GET");
            A.CallTo(() => context.Request.UserHostAddress).Returns(ipAddress);
            
            var result = inspector.Authenticate(context);

            NonceManager.Now = () => { return DateTime.UtcNow; };
            Assert.True(result.Success);
            Assert.Equal(result.Principal.Identity.Name, "Mufasa");
        }

        [Fact]
        public void Authenticate_FalseOnMismatchedRealm()
        {
            var inspector = new AuthenticationInspectingAuthenticator(configuration);

            string ipAddress = "127.0.0.1";
            string nonce = NonceManager.Generate(ipAddress, privateHashEncoder);

            A.CallTo(() => configuration.Realm).Returns("testrealm@testhost.com");

            var headers = new NameValueCollection() { { "Authorization", string.Format(
@"Digest username=""Mufasa"",realm=""testrealm@host.com"",
                     nonce=""{0}"",
                     uri=""/dir/index.html"",qop=auth,nc=00000001,cnonce=""0a4f113b"",
                     response=""6629fae49393a05397450978507c4ef1"",
                     opaque=""5ccc069c403ebaf9f0171e9517f40e41""", nonce) } };

            A.CallTo(() => context.Request.Headers).Returns(headers);
            A.CallTo(() => context.Request.HttpMethod).Returns("GET");
            A.CallTo(() => context.Request.UserHostAddress).Returns(ipAddress);
            
            var result = inspector.Authenticate(context);
            //TODO: validate that we have a GenericPrincipal / GenericIdentity
            Assert.False(result.Success);
        }

        [Fact(Skip = "Write some more examples of auth failures")]
        public void Authenticate_FalseOnAuthenticationFailures()
        {
        }
    }
}