using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;
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
			private IAuthenticatorConfiguration _config;

			public MockAuthenticator(IAuthenticatorConfiguration config)
			{
				this._config = config;
			}

			public string Name { get { return "Mock"; } }

			public AuthenticationResult Authenticate(HttpContextBase context)
			{
				return null;
			}

			public IAuthenticatorConfiguration Configuration
			{
				get { return _config; }
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
			public IPrincipal OnAuthenticationFailure(HttpContextBase context, Dictionary<IAuthenticator, AuthenticationResult> inspectorResults)
			{
				throw new InvalidOperationException();
			}

			public IFailureHandlerConfiguration Configuration
			{
				get { throw new InvalidOperationException(); }
			}
		}

		class MockFailureHandlerConfiguration :
			IFailureHandlerConfiguration
		{
			private IFailureHandlerConfiguration _config;

			public MockFailureHandlerConfiguration(IFailureHandlerConfiguration config)
			{
				this._config = config;
			}

			public IFailureHandlerConfiguration Configuration
			{
				get { return _config; }
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
			var config = new HttpAuthenticationConfigurationSection();
			Assert.Null(HttpContextInspectorsLocator.GetFailureHandler(config));
		}

		[Fact]
		public void GetFailureHandler_ThrowsOnInvalidConfigurationTypeName()
		{
			var config = new HttpAuthenticationConfigurationSection() { FailureHandlerFactoryName = "GobbledyGook" };
			Assert.Throws<ArgumentNullException>(() => HttpContextInspectorsLocator.GetFailureHandler(config));
		}

		[Fact]
		public void GetFailureHandler_ReturnsCorrectlyTypedInstance()
		{
			var config = new HttpAuthenticationConfigurationSection() { FailureHandlerFactoryName = typeof(MockConfigFactory).AssemblyQualifiedName };

			Assert.IsType<MockFailureHandler>(HttpContextInspectorsLocator.GetFailureHandler(config));
		}
	}
}