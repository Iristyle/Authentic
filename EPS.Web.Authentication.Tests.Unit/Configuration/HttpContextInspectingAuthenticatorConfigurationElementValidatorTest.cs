using Xunit;

namespace EPS.Web.Authentication.Configuration.Tests.Unit
{
    public class HttpContextInspectingAuthenticatorConfigurationElementValidatorTest
    {
        private readonly HttpContextInspectingAuthenticatorConfigurationElementValidator validator 
            = new HttpContextInspectingAuthenticatorConfigurationElementValidator();

        class MockConfiguration : IHttpContextInspectingAuthenticatorConfigurationElement
        {
            public string RoleProviderName { get; set; }
            public string Name { get; set; }
            public string Factory { get; set; }
            public bool RequireSsl { get; set; }
            public string CustomConfigurationSectionName { get; set; }
            public string ProviderName { get; set; }
            public string PrincipalBuilderFactory { get; set; }
        }

        [Fact(Skip = "Write suite of tests that have bad data, etc")]
        public void Validation_FailsOnXXX()
        {
            var config = new MockConfiguration();            
            var result = validator.Validate(config);
        }
    }
}
