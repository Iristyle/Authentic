using System;
using EPS.Web.Authentication.Digest.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class AuthenticationInspectingAuthenticatorFactoryTest
    {
        [Fact]
        public void Construct_ThrowsOnImproperConfiguration()
        {
            AuthenticationInspectingAuthenticatorFactory factory = new AuthenticationInspectingAuthenticatorFactory();
            Assert.Throws<ArgumentNullException>(() => factory.Construct(null));
        }

        [Fact]
        public static void Construct_ReturnsAuthenticationFailureHandler()
        {
            AuthenticationInspectingAuthenticatorFactory factory = new AuthenticationInspectingAuthenticatorFactory();
            IAuthenticationHeaderInspectorConfigurationElement configuration = A.Fake<IAuthenticationHeaderInspectorConfigurationElement>();
            A.CallTo(() => configuration.PrivateKey).Returns("12345678");
            A.CallTo(() => configuration.Realm).Returns("realm");
            A.CallTo(() => configuration.NonceValidDuration).Returns(TimeSpan.FromSeconds(30));

            Assert.IsType<AuthenticationInspectingAuthenticator>(factory.Construct(configuration));
        }
    }
}
