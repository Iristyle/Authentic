using System;
using EPS.Annotations;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class DigestQualityOfProtectionTypesTest
    {
        [Fact]
        public void EnumValue_HasCorrectStringForAuthentication()
        {
            Assert.Equal(DigestQualityOfProtectionType.Authentication, "auth".ToEnumFromEnumValue<DigestQualityOfProtectionType>());
        }

        [Fact]
        public void EnumValue_HasCorrectStringForAuthenticationWithIntegrity()
        {
            Assert.Equal(DigestQualityOfProtectionType.AuthenticationWithIntegrity, "auth-int".ToEnumFromEnumValue<DigestQualityOfProtectionType>());
        }       

        [Fact]
        public void EnumValue_HasCorrectStringForUnspecified()
        {
            Assert.Equal(DigestQualityOfProtectionType.Unspecified, string.Empty.ToEnumFromEnumValue<DigestQualityOfProtectionType>());
        }
    }
}
