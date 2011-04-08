using System;
using EPS.Web.Abstractions;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Configuration.Tests.Unit
{
    public class PrincipalBuilderLocatorTest
    {
        class MockPrincipalBuilderFactory : IPrincipalBuilderFactory
        {
            public IPrincipalBuilder Construct(Configuration.IAuthenticatorConfiguration config)
            {
                return new MockPrincipalBuilder();
            }
        }

        class MockPrincipalBuilder : IPrincipalBuilder
        {
            public string Name
            {
                get { throw new NotImplementedException(); }
            }

            public Configuration.IAuthenticatorConfiguration Configuration
            {
                get { throw new NotImplementedException(); }
            }

            public System.Security.Principal.IPrincipal ConstructPrincipal(System.Web.HttpContextBase context, string userName, string password)
            {
                throw new NotImplementedException();
            }
        }


        [Fact]
        public void Resolve_ReturnsNullOnEmptyFactoryName()
        {
            var config = new AuthenticatorConfigurationElement() { PrincipalBuilderFactory = string.Empty };
            Assert.Null(PrincipalBuilderLocator.Resolve(config));
        }
        
        [Fact]
        public void Resolve_ReturnsNullOnNullFactoryName()
        {
            Assert.Null(PrincipalBuilderLocator.Resolve(new AuthenticatorConfigurationElement()));
        }

        [Fact]
        public void Resolve_ReturnsExpectedTypeInstance()
        {
            var config = new AuthenticatorConfigurationElement() { PrincipalBuilderFactory = typeof(MockPrincipalBuilderFactory).AssemblyQualifiedName };
            Assert.IsType(typeof(MockPrincipalBuilder), PrincipalBuilderLocator.Resolve(config));
        }
    }
}
