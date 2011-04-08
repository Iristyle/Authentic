using System;
using EPS.Web.Authentication.Digest.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class DigestAuthenticatorFactoryTest
    {
        [Fact]
        public void Construct_ThrowsOnImproperConfiguration()
        {
            DigestAuthenticatorFactory factory = new DigestAuthenticatorFactory();
            Assert.Throws<ArgumentNullException>(() => factory.Construct(null));
        }

        [Fact]
        public static void Construct_ReturnsAuthenticationFailureHandler()
        {
            DigestAuthenticatorFactory factory = new DigestAuthenticatorFactory();
            IDigestAuthenticatorConfiguration configuration = A.Fake<IDigestAuthenticatorConfiguration>();
            A.CallTo(() => configuration.PrivateKey).Returns("12345678");
            A.CallTo(() => configuration.Realm).Returns("realm");
            A.CallTo(() => configuration.NonceValidDuration).Returns(TimeSpan.FromSeconds(30));

            Assert.IsType<DigestAuthenticator>(factory.Construct(configuration));
        }
    }
}
