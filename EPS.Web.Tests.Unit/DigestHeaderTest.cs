using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class DigestHeaderTest
    {
        [Fact]
        public void MatchesCredential_ThrowsOnAuthenticationWithIntegrity()
        {
            var header = new DigestHeader() { QualityOfProtection = DigestQualityOfProtectionType.AuthenticationWithIntegrity };

            Assert.Throws<NotImplementedException>(() => { header.MatchesCredentials(string.Empty, string.Empty, string.Empty, string.Empty); });
        }

        [Fact]
        public void MatchesCredentials_ThrowsOnInvalidHttpMethodName()
        {
            var header = new DigestHeader() { Verb = (HttpMethodNames)57 };
            Assert.Throws<ArgumentOutOfRangeException>(() => { header.MatchesCredentials(string.Empty, string.Empty, string.Empty, string.Empty); });
        }

        [Fact]
        public void MatchesCredentials_ThrowsOnNullPassword()
        {
            var header = new DigestHeader();
            Assert.Throws<ArgumentNullException>(() => { header.MatchesCredentials(string.Empty, string.Empty, string.Empty, null); });
        }

        [Fact]
        public void MatchesCredentials_ThrowsOnNullRealm()
        {
            var header = new DigestHeader();
            Assert.Throws<ArgumentNullException>(() => { header.MatchesCredentials(null, string.Empty, string.Empty, string.Empty); });
        }

        [Fact]
        public void MatchesCredentials_ThrowsOnNullNonce()
        {
            var header = new DigestHeader();

            Assert.Throws<ArgumentNullException>(() => { header.MatchesCredentials(string.Empty, null, string.Empty, string.Empty); });
        }

        [Fact]
        public void MatchesCredentials_ReturnsFalseOnMismatchedNonce()
        {
            var header = new DigestHeader() { Nonce = "123", Verb = HttpMethodNames.Get };
            Assert.False(header.MatchesCredentials(string.Empty, "345", string.Empty, string.Empty));
        }

        [Fact]
        public void MatchesCredentials_ReturnsFalseOnMismatchedRealm()
        {
            //sample from http://en.wikipedia.org/wiki/Digest_access_authentication
            string nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093",
                opaque = "5ccc069c403ebaf9f0171e9517f40e41";

            var header = new DigestHeader()
            {
                Verb = HttpMethodNames.Get,
                UserName = "Mufasa",
                Realm = "testrealm@host.com",
                Nonce = nonce,
                Uri = "/dir/index.html",
                QualityOfProtection = DigestQualityOfProtectionType.Authentication,
                RequestCounter = 1,
                ClientNonce = "0a4f113b",
                Opaque = opaque,
                Response = "6629fae49393a05397450978507c4ef1"
            };

            //this would work, except for the fact that the realms don't match (and accordingly the responses shouldn't)
            Assert.False(header.MatchesCredentials("bad@test.com", nonce, opaque, "Circle Of Life"));
        }

        [Fact]
        public void MatchesCredentials_ReturnsTrueOnMatchedResponseWithAuthenticationQualityOfProtection()
        {
            //sample from http://en.wikipedia.org/wiki/Digest_access_authentication
            string realm = "testrealm@host.com",
                nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093",
                opaque = "5ccc069c403ebaf9f0171e9517f40e41";

            var header = new DigestHeader()            
            {
                Verb = HttpMethodNames.Get,
                UserName = "Mufasa",
                Realm = realm,
                Nonce = nonce,
                Uri = "/dir/index.html",
                QualityOfProtection = DigestQualityOfProtectionType.Authentication,
                RequestCounter = 1,
                ClientNonce = "0a4f113b",
                Opaque = opaque,
                Response = "6629fae49393a05397450978507c4ef1"
            };

            Assert.True(header.MatchesCredentials(realm, nonce, opaque, "Circle Of Life"));
        }

        [Fact]
        public void MatchesCredentials_ReturnsTrueOnMatchedResponseWithNoQualityOfProtection()
        {
            //sample from http://en.wikipedia.org/wiki/Digest_access_authentication
            string realm = "testrealm@host.com",
                nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093",
                opaque = "5ccc069c403ebaf9f0171e9517f40e41";

            var header = new DigestHeader()
            {
                Verb = HttpMethodNames.Get,
                UserName = "Mufasa",
                Realm = realm,
                Nonce = nonce,
                Uri = "/dir/index.html",
                QualityOfProtection = DigestQualityOfProtectionType.Unspecified,
                RequestCounter = 1,
                ClientNonce = "0a4f113b",
                Opaque = opaque,
                //test response for unspecified qop calculated here http://md5-hash-online.waraxe.us/
                Response = "670fd8c2df070c60b045671b8b24ff02"
            };

            Assert.True(header.MatchesCredentials(realm, nonce, opaque, "Circle Of Life"));
        }

        [Fact(Skip = "Create some funky strings to test and make sure they pass/fail")]
        public void MatchesCredentials_AdvancedScenarios()
        { }
    }
}