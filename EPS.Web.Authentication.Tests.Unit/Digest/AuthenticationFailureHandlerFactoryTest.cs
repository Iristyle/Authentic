using System;
using EPS.Web.Authentication.Digest.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class AuthenticationFailureHandlerFactoryTest
    {
        [Fact]
        public void Construct_ThrowsOnImproperConfiguration()
        {
            AuthenticationFailureHandlerFactory factory = new AuthenticationFailureHandlerFactory();
            Assert.Throws<ArgumentNullException>(() => factory.Construct(null));
        }

        [Fact]
        public static void Construct_ReturnsAuthenticationFailureHandler()
        {
            AuthenticationFailureHandlerFactory factory = new AuthenticationFailureHandlerFactory();
            IAuthenticationFailureHandlerConfigurationSection configuration = A.Fake<IAuthenticationFailureHandlerConfigurationSection>();
            A.CallTo(() => configuration.PrivateKey).Returns("12345678");
            A.CallTo(() => configuration.Realm).Returns("realm");
            A.CallTo(() => configuration.NonceValidDuration).Returns(TimeSpan.FromSeconds(30));

            Assert.IsType<AuthenticationFailureHandler>(factory.Construct(configuration));
        }
    }

}
