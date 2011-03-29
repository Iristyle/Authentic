using System;
using System.Web.Security;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Utility.Tests.Unit
{
    public class MembershipProviderLocatorTest : IDisposable
    {
        private readonly MembershipProvider fakeMembershipProvider = A.Fake<MembershipProvider>();

        public MembershipProviderLocatorTest()
        {
            A.CallTo(() => fakeMembershipProvider.Name).Returns("Fake");
            Membership.Providers.AddMembershipProvider(fakeMembershipProvider);            
        }

        public void Dispose()
        {
            Membership.Providers.RemoveMembershipProvider("Fake");
        }

        [Fact]
        public void GetProvider_FindsRegisteredProviderByName()
        {
            Assert.Same(fakeMembershipProvider, MembershipProviderLocator.GetProvider("Fake"));
        }

        [Fact]
        public void GetProvider_ThrowsOnUnregisteredProviderName()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MembershipProviderLocator.GetProvider("MissingProvider"));
        }

        [Fact]
        public void GetProvider_DefaultStringMatchesDefaultRegisteredProvider()
        {
            Assert.Same(Membership.Provider, MembershipProviderLocator.GetProvider("default"));
        }

        [Fact]
        public void GetProvider_ReturnsNullOnNullProviderName()
        {
            Assert.Null(MembershipProviderLocator.GetProvider(null));
        }

        [Fact]
        public void GetProvider_ReturnsNullOnEmptyProviderName()
        {
            Assert.Null(MembershipProviderLocator.GetProvider(string.Empty));
        }
    }
}
