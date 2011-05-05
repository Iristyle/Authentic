using System;
using EPS.Web.Authentication.Basic.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Basic.Tests.Unit
{
	public class BasicAuthenticatorTest
	{
		private IBasicAuthenticatorConfiguration GetConfig()
		{
			return null;
		}

		[Fact]
		public void Authenticate_ThrowsOnNullContext()
		{
			Assert.Throws<ArgumentNullException>(() => new BasicAuthenticator(A.Fake<IBasicAuthenticatorConfiguration>())
				.Authenticate(null));
		}

		[Fact(Skip = "Finish test suite")]
		public void Authenticate_DoesStuff()
		{
		}
	}
}
