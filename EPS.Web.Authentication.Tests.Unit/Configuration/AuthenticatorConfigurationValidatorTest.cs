using Xunit;

namespace EPS.Web.Authentication.Configuration.Tests.Unit
{
    public class AuthenticatorConfigurationValidatorTest
    {
        private readonly AuthenticatorConfigurationValidator validator 
            = new AuthenticatorConfigurationValidator();

        class MockConfiguration : IAuthenticatorConfiguration
        {
            public string RoleProviderName { get; set; }
            public string Name { get; set; }
            public Abstractions.IAuthenticator Authenticator { get; private set; }
            public bool RequireSsl { get; set; }
            public string ProviderName { get; set; }
            public Web.Abstractions.IPrincipalBuilder PrincipalBuilder { get; private set; }
        }

        [Fact(Skip = "Write suite of tests that have bad data, etc")]
        public void Validation_FailsOnXXX()
        {
            var config = new MockConfiguration();            
            var result = validator.Validate(config);
        }
    }
}
