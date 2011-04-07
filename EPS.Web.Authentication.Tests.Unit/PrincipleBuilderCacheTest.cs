using System;
using EPS.Web.Abstractions;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Tests.Unit
{
    public class PrincipleBuilderCacheTest
    {
        class MockPrincipalBuilderFactory : IPrincipalBuilderFactory
        {
            public IPrincipalBuilder Construct(Configuration.IHttpContextInspectingAuthenticatorConfigurationElement config)
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

            public Configuration.IHttpContextInspectingAuthenticatorConfigurationElement Configuration
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
            var config = A.Fake<Configuration.IHttpContextInspectingAuthenticatorConfigurationElement>();
            A.CallTo(() => config.PrincipalBuilderFactory).Returns(string.Empty);
            Assert.Null(PrincipalBuilderCache.Resolve(config));
        }
        
        [Fact]
        public void Resolve_ReturnsNullOnNullFactoryName()
        {
            var config = A.Fake<Configuration.IHttpContextInspectingAuthenticatorConfigurationElement>();
            Assert.Null(PrincipalBuilderCache.Resolve(config));
        }

        [Fact]
        public void Resolve_ReturnsExpectedTypeInstance()
        {
            var config = A.Fake<Configuration.IHttpContextInspectingAuthenticatorConfigurationElement>();
            A.CallTo(() => config.PrincipalBuilderFactory).Returns(typeof(MockPrincipalBuilderFactory).AssemblyQualifiedName);
            Assert.IsType(typeof(MockPrincipalBuilder), PrincipalBuilderCache.Resolve(config));
        }
    }
}
