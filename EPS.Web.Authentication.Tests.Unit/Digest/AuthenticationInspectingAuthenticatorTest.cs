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

        public AuthenticationInspectingAuthenticatorTest()
        {
            Fake.InitializeFixture(this);

            A.CallTo(() => fakeUser.GetPassword()).Returns("Circle Of Life");
            A.CallTo(() => fakeMembershipProvider.Name).Returns("Fake");
            A.CallTo(() => fakeMembershipProvider.GetUser("Mufasa", A<Boolean>.Ignored)).Returns(fakeUser);
            Membership.Providers.AddMembershipProvider(fakeMembershipProvider);

            A.CallTo(() => configuration.Factory).Returns(typeof(AuthenticationInspectingAuthenticatorFactory).FullName);
            A.CallTo(() => configuration.ProviderName).Returns(fakeMembershipProvider.Name);
            A.CallTo(() => configuration.GetPrincipalBuilder()).Returns(principalBuilder);
            A.CallTo(() => principalBuilder.ConstructPrincipal(A<HttpContextBase>.Ignored, A<string>.Ignored, A<string>.Ignored))
                .ReturnsLazily(call => new GenericPrincipal(new GenericIdentity(call.GetArgument<string>("userName")), new[] { "roles" }));
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
            A.CallTo(() => configuration.Realm).Returns("testrealm@host.com");

            var headers = new NameValueCollection() { { "Authorization", 
@"Digest username=""Mufasa"",realm=""testrealm@host.com"",
                     nonce=""dcd98b7102dd2f0e8b11d0f600bfb0c093"",
                     uri=""/dir/index.html"",qop=auth,nc=00000001,cnonce=""0a4f113b"",
                     response=""6629fae49393a05397450978507c4ef1"",
                     opaque=""5ccc069c403ebaf9f0171e9517f40e41""" } };

            A.CallTo(() => context.Request.Headers).Returns(headers);
            A.CallTo(() => context.Request.HttpMethod).Returns("GET");
            Opaque.Current = () => "5ccc069c403ebaf9f0171e9517f40e41";
            NonceCache.Current = () => "dcd98b7102dd2f0e8b11d0f600bfb0c093";

            var inspector = new AuthenticationInspectingAuthenticator(configuration);
            var result = inspector.Authenticate(context);
            //TODO: validate that we have a GenericPrincipal / GenericIdentity
            Assert.True(result.Success && result.Principal.Identity.Name == "Mufasa");
        }

        [Fact]
        public void Authenticate_FalseOnMismatchedRealm()
        {
            A.CallTo(() => configuration.Realm).Returns("testrealm@testhost.com");

            var headers = new NameValueCollection() { { "Authorization", 
@"Digest username=""Mufasa"",realm=""testrealm@host.com"",
                     nonce=""dcd98b7102dd2f0e8b11d0f600bfb0c093"",
                     uri=""/dir/index.html"",qop=auth,nc=00000001,cnonce=""0a4f113b"",
                     response=""6629fae49393a05397450978507c4ef1"",
                     opaque=""5ccc069c403ebaf9f0171e9517f40e41""" } };

            A.CallTo(() => context.Request.Headers).Returns(headers);
            A.CallTo(() => context.Request.HttpMethod).Returns("GET");
            Opaque.Current = () => "5ccc069c403ebaf9f0171e9517f40e41";
            NonceCache.Current = () => "dcd98b7102dd2f0e8b11d0f600bfb0c093";

            var inspector = new AuthenticationInspectingAuthenticator(configuration);
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