using System.Web;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.IIS.Tests.Unit
{
	public class HttpAuthenticationModuleTest
	{
		[Fake]
		private readonly HttpContextBase httpContext = A.Fake<HttpContextBase>();

		public HttpAuthenticationModuleTest()
		{
			Fake.InitializeFixture(this);
		}

		[Fact(Skip = "Ensure that request context was processed")]
		public void ReadsContext()
		{
			//TODO: configure module appropriately
			var module = new HttpAuthenticationModule();
			module.OnAuthenticateRequest(httpContext);

			//test that stuff was read out, etc
		}

		[Fact(Skip = "Setup a test with a chain of context handlers")]
		public void CallsConfiguredHandlers()
		{
		}

		[Fact(Skip = "Verify that the Configure handler is fired and that the specified config is used properly")]
		public void Configure_CalledToSupportCodeBasedConfiguration()
		{

		}

		[Fact(Skip = "Ensures that the configuration ordering is followed")]
		public void Configure_CalledAfterConfigFileLoaded()
		{
		}
	}
}
