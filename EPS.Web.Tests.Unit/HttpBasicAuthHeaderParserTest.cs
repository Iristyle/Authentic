using System;
using System.Collections.Generic;
using System.Net;
using EPS.Utility;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class HttpBasicAuthHeaderParserTest
    {
        private IEqualityComparer<NetworkCredential> comparer = new GenericEqualityComparer<NetworkCredential>
            ((x, y) => MemberComparer.Equal(x, y));

        [Fact]
        public void TryExtractCredentialsFromHeader_ReturnsFalseWithNullNetworkCredentialInstance_OnNullString()
        {
            NetworkCredential networkCredential;
            bool status = HttpBasicAuthHeaderParser.TryExtractCredentialsFromHeader(null, out networkCredential);
            Assert.True(status == false && null == networkCredential);
        }

        [Fact]
        public void TryExtractCredentialsFromHeader_ReturnsFalseWithNullNetworkCredentialInstance_OnEmptyString()
        {
            NetworkCredential networkCredential;
            bool status = HttpBasicAuthHeaderParser.TryExtractCredentialsFromHeader(string.Empty, out networkCredential);
            Assert.True(status == false && null == networkCredential);
        }

        [Fact]
        public void TryExtractCredentialsFromHeader_ParsesCompleteHeaderCorrectly()
        {
            string header = @"Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==";
            NetworkCredential networkCredential,
                expectedCredentials = new NetworkCredential("Aladdin", "open sesame");
    
            bool status = HttpBasicAuthHeaderParser.TryExtractCredentialsFromHeader(header, out networkCredential);
            Assert.True(status == true && comparer.Equals(networkCredential, expectedCredentials));
        }

        [Fact]
        public void ExtractCredentialsFromHeader_ThrowsOnNullString()
        {
            Assert.Throws<ArgumentNullException>(() => HttpBasicAuthHeaderParser.ExtractCredentialsFromHeader(null));
        }

        [Fact]
        public void ExtractCredentialsFromHeader_ThrowsOnEmptyString()
        {
            Assert.Throws<ArgumentException>(() => HttpBasicAuthHeaderParser.ExtractCredentialsFromHeader(string.Empty));
        }

        [Fact]
        public void ExtractCredentialsFromHeader_ThrowsOnNonBasicAuthHeaderString()
        {
            Assert.Throws<ArgumentException>(() => HttpBasicAuthHeaderParser.ExtractCredentialsFromHeader("Digest stuff"));
        }

        [Fact]
        public void ExtractCredentialsFromHeader_ParsesCompleteHeaderCorrectly()
        {
            string header = @"Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==";
            NetworkCredential expectedCredentials = new NetworkCredential("Aladdin", "open sesame");

            Assert.Equal(expectedCredentials, HttpBasicAuthHeaderParser.ExtractCredentialsFromHeader(header), comparer);
        }
    }
}