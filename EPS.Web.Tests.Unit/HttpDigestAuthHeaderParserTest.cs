using System;
using System.Collections.Generic;
using EPS.Utility;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class HttpDigestAuthHeaderParserTest
    {
        private IEqualityComparer<DigestHeader> comparer = new GenericEqualityComparer<DigestHeader>
            ((x, y) => PropertyComparer.Equal(x, y));

        [Fact]
        public void TryExtractDigestHeader_ReturnsFalseWithNullDigestHeaderInstance_OnNullString()
        {
            DigestHeader digestHeader;
            bool status = HttpDigestAuthHeaderParser.TryExtractDigestHeader(null, out digestHeader);
            Assert.True(status == false && null == digestHeader);
        }

        [Fact]
        public void TryExtractDigestHeader_ReturnsFalseWithNullDigestHeaderInstance_OnEmptyString()
        {
            DigestHeader digestHeader;
            bool status = HttpDigestAuthHeaderParser.TryExtractDigestHeader(string.Empty, out digestHeader);
            Assert.True(status == false && null == digestHeader);
        }

        [Fact]
        public void ExtractDigestHeader_ThrowsOnNullString()
        {
            Assert.Throws<ArgumentNullException>(() => HttpDigestAuthHeaderParser.ExtractDigestHeader(null));
        }

        [Fact]
        public void ExtractDigestHeader_ThrowsOnEmptyString()
        {
            Assert.Throws<ArgumentException>(() => HttpDigestAuthHeaderParser.ExtractDigestHeader(string.Empty));
        }

        [Fact]
        public void ExtractDigestHeader_ThrowsOnBasicAuthTypeString()
        {
            Assert.Throws<ArgumentException>(() => HttpDigestAuthHeaderParser.ExtractDigestHeader(@"Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ=="));
        }

        [Fact]
        public void ExtractDigestHeader_ParsesCompleteHeaderCorrectly()
        {
            string header = @"Digest username=""Mufasa"",
                     realm=""testrealm@host.com"",
                     nonce=""dcd98b7102dd2f0e8b11d0f600bfb0c093"",
                     uri=""/dir/index.html"",
                     qop=auth,
                     nc=00000001,
                     cnonce=""0a4f113b"",
                     response=""6629fae49393a05397450978507c4ef1"",
                     opaque=""5ccc069c403ebaf9f0171e9517f40e41""";

            DigestHeader expectedHeader = new DigestHeader()
            {
                ClientNonce = "0a4f113b",
                Nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093",
                Opaque = "5ccc069c403ebaf9f0171e9517f40e41",
                QualityOfProtection = DigestQualityOfProtectionType.Authentication,
                Realm = "testrealm@host.com",
                RequestCounter = 1,
                Response = "6629fae49393a05397450978507c4ef1",
                Uri = "/dir/index.html",
                UserName = "Mufasa"
            };

            Assert.Equal(expectedHeader, HttpDigestAuthHeaderParser.ExtractDigestHeader(header), comparer);
        }

        [Fact(Skip = "Need to write some more advanced parsing tests as the above example is entirely insufficient")]
        public void ExtractDigestHeader_AdvancedTests()
        {
        }
    }
}