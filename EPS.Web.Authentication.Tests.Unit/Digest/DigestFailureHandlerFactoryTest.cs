using System;
using EPS.Web.Authentication.Digest.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class DigestFailureHandlerFactoryTest
    {
        [Fact]
        public void Construct_ThrowsOnImproperConfiguration()
        {
            DigestFailureHandlerFactory factory = new DigestFailureHandlerFactory();
            Assert.Throws<ArgumentNullException>(() => factory.Construct(null));
        }

        [Fact]
        public static void Construct_ReturnsAuthenticationFailureHandler()
        {
            DigestFailureHandlerFactory factory = new DigestFailureHandlerFactory();
            IDigestFailureHandlerConfiguration configuration = A.Fake<IDigestFailureHandlerConfiguration>();
            A.CallTo(() => configuration.PrivateKey).Returns("12345678");
            A.CallTo(() => configuration.Realm).Returns("realm");
            A.CallTo(() => configuration.NonceValidDuration).Returns(TimeSpan.FromSeconds(30));

            Assert.IsType<DigestFailureHandler>(factory.Construct(configuration));
        }
    }

}
