using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Tests.Unit
{
    public class HttpContextInspectorsLocatorTest
    {
        class MockPrincipal : IPrincipal
        {
            public IIdentity Identity { get { return null; } }
            public bool IsInRole(string role) { return false; }
        }

        class MockAuthenticator :
            IAuthenticator
        {
            private IAuthenticatorConfiguration config;

            public MockAuthenticator(IAuthenticatorConfiguration config)
            {
                this.config = config;
            }

            public string Name { get { return "Mock"; } }

            public AuthenticationResult Authenticate(HttpContextBase context)
            {
                return null;
            }

            public IAuthenticatorConfiguration Configuration
            {
                get { return config; }
            }
        }

        class MockAuthConfigFactory :
            IAuthenticatorFactory
        {
            public IAuthenticator Construct(IAuthenticatorConfiguration config)
            {
                return new MockAuthenticator(config);
            }
        }

        class MockFailureHandler : 
            IFailureHandler
        {
            public IPrincipal  OnAuthenticationFailure(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults)
            {
 	            throw new NotImplementedException();
            }

            public IFailureHandlerConfiguration  Configuration
            {
	            get { throw new NotImplementedException(); }
            }
        }

        class MockFailureHandlerConfiguration :
            IFailureHandlerConfiguration
        {
            private IFailureHandlerConfiguration config;

            public MockFailureHandlerConfiguration(IFailureHandlerConfiguration config)
            {
                this.config = config;
            }

            public IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults)
            {
                return new MockPrincipal();
            }

            public IFailureHandlerConfiguration Configuration
            {
                get { return config; }
            }

            public bool RequireSsl { get; set; }
        }

        class MockConfigFactory : 
            IFailureHandlerFactory
        {
            IFailureHandler IFailureHandlerFactory.Construct(IFailureHandlerConfiguration config)
            {
                return new MockFailureHandler();
            }
        }

        [Fact]
        public void Construct_ThrowsOnNullConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() => HttpContextInspectorsLocator.Construct(null as AuthenticatorConfigurationElement));
        }

        [Fact]
        public void Construct_ThrowsOnNullInspectors()
        {
            Assert.Throws<ArgumentNullException>(() => HttpContextInspectorsLocator.Construct(null as IDictionary<string, AuthenticatorConfigurationElement>));
        }

        [Fact]
        public void Construct_ThrowsOnInvalidConfigurationTypeName()
        {
            var config = new AuthenticatorConfigurationElement() { Factory = "GobbledyGook" };
            Assert.Throws<ArgumentNullException>(() => HttpContextInspectorsLocator.Construct(config));
        }

        [Fact]
        public void Construct_ReturnsCorrectlyTypedInstance()
        {
            var config = new AuthenticatorConfigurationElement() { Factory = typeof(MockAuthConfigFactory).AssemblyQualifiedName };
            Assert.IsType<MockAuthenticator>(HttpContextInspectorsLocator.Construct(config));
        }

        [Fact(Skip = "Need to make sure that these tests execute properly")]
        public void Construct_TestsIEnumerableVersion()
        { }

        [Fact]
        public void GetFailureHandler_ThrowsOnNullConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() => HttpContextInspectorsLocator.GetFailureHandler(null));
        }

        [Fact]
        public void GetFailureHandler_ReturnsNullOnNullConfiguration()
        {
            var config = new HttpAuthenticationModuleConfigurationSection();
            Assert.Null(HttpContextInspectorsLocator.GetFailureHandler(config));
        }

        [Fact]
        public void GetFailureHandler_ThrowsOnInvalidConfigurationTypeName()
        {
            var config = new HttpAuthenticationModuleConfigurationSection() { FailureHandlerFactoryName = "GobbledyGook" };
            Assert.Throws<ArgumentNullException>(() => HttpContextInspectorsLocator.GetFailureHandler(config));
        }

        [Fact]
        public void GetFailureHandler_ReturnsCorrectlyTypedInstance()
        {
            var config = new HttpAuthenticationModuleConfigurationSection() 
                { FailureHandlerFactoryName = typeof(MockConfigFactory).AssemblyQualifiedName };

            Assert.IsType<MockFailureHandler>(HttpContextInspectorsLocator.GetFailureHandler(config));
        }
    }
}