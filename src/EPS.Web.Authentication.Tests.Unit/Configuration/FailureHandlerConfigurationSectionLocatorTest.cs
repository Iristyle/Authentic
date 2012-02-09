using System;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Configuration.Tests.Unit
{
    public class FailureHandlerConfigurationSectionLocatorTest
    {
        class MockFailureSection : IFailureHandlerConfiguration
        {
            public bool RequireSsl { get; set; }
        }

        [Fact]
        public void Register_ThrowsOnNullName()
        {
            Assert.Throws<ArgumentNullException>(() => 
                FailureHandlerConfigurationSectionLocator.Register(null, A.Fake<IFailureHandlerConfiguration>()));
        }

        [Fact]
        public void Register_ThrowsOnEmptyName()
        {
            Assert.Throws<ArgumentException>(() =>
                FailureHandlerConfigurationSectionLocator.Register(string.Empty, A.Fake<IFailureHandlerConfiguration>()));
        }

        [Fact]
        public void Register_ThrowsOnNullConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() =>
                FailureHandlerConfigurationSectionLocator.Register("Test", null));
        }

        [Fact]
        public void Register_PreventsDuplicateRegistrationsByName()
        {
            FailureHandlerConfigurationSectionLocator.Register("Duplicate", A.Fake<IFailureHandlerConfiguration>());
            Assert.Throws<ArgumentException>(() => FailureHandlerConfigurationSectionLocator.Register("Duplicate", A.Fake<IFailureHandlerConfiguration>()));
        }

        [Fact]
        public void Resolve_ReturnsNullOnNullConfigurationSectionName()
        {
            Assert.Null(FailureHandlerConfigurationSectionLocator.Resolve(null));
        }

        [Fact]
        public void Resolve_FindsManuallyRegistered()
        {
            string sectionName = "ManualRegistration";
            var manualSection = new MockFailureSection();
            FailureHandlerConfigurationSectionLocator.Register(sectionName, manualSection);

            Assert.Same(manualSection, FailureHandlerConfigurationSectionLocator.Resolve(sectionName));
        }

        [Fact]
        public void Resolve_ThrowsOnMissingConfigurationSection()
        {
            Assert.Throws<ArgumentException>(() => FailureHandlerConfigurationSectionLocator.Resolve("NonExistantSection"));
        }

        [Fact(Skip = "This one should get implemented as an integration test")]
        public void Resolve_ReturnsValidConfigurationSection()
        {
        }
    }
}
