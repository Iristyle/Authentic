using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using EPS.Web.Abstractions;
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
				get { throw new InvalidOperationException(); }
			}

			public Configuration.IAuthenticatorConfiguration Configuration
			{
				get { throw new InvalidOperationException(); }
			}

			public IPrincipal ConstructPrincipal(HttpContextBase context, MembershipUser membershipUser, string userName, string password)
			{
				throw new InvalidOperationException();
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
