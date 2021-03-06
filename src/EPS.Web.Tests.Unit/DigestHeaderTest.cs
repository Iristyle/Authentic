﻿using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace EPS.Web.Tests.Unit
{
	public class DigestHeaderTest
	{
		[Fact]
		public void MatchesCredential_ThrowsOnAuthenticationWithIntegrity()
		{
			var header = new DigestHeader() { QualityOfProtection = DigestQualityOfProtectionType.AuthenticationWithIntegrity };

			Assert.Throws<NotImplementedException>(() => { header.MatchesCredentials(string.Empty, string.Empty, string.Empty); });
		}

		[Fact]
		public void MatchesCredentials_ThrowsOnInvalidHttpMethodName()
		{
			var header = new DigestHeader() { Verb = "FOO" };
			Assert.Throws<NotSupportedException>(() => { header.MatchesCredentials(string.Empty, string.Empty, string.Empty); });
		}

		[Fact]
		public void MatchesCredentials_ThrowsOnNullPassword()
		{
			var header = new DigestHeader();
			Assert.Throws<ArgumentNullException>(() => { header.MatchesCredentials(string.Empty, string.Empty, null); });
		}

		[Fact]
		public void MatchesCredentials_ThrowsOnNullRealm()
		{
			var header = new DigestHeader();
			Assert.Throws<ArgumentNullException>(() => { header.MatchesCredentials(null, string.Empty, string.Empty); });
		}

		[Fact]
		[SuppressMessage("Gendarme.Rules.Portability", "DoNotHardcodePathsRule", Justification = "The path is part of a Uri and this usage is acceptable")]
		public void MatchesCredentials_ReturnsFalseOnMismatchedRealm()
		{
			//sample from http://en.wikipedia.org/wiki/Digest_access_authentication
			string opaque = "5ccc069c403ebaf9f0171e9517f40e41";

			var header = new DigestHeader()
			{
				Verb = "GET",
				UserName = "Mufasa",
				Realm = "testrealm@host.com",
				Nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093",
				Uri = "/dir/index.html",
				QualityOfProtection = DigestQualityOfProtectionType.Authentication,
				RequestCounter = 1,
				ClientNonce = "0a4f113b",
				Opaque = opaque,
				Response = "6629fae49393a05397450978507c4ef1"
			};

			//this would work, except for the fact that the realms don't match (and accordingly the responses shouldn't)
			Assert.False(header.MatchesCredentials("bad@test.com", opaque, "Circle Of Life"));
		}

		[Fact]
		[SuppressMessage("Gendarme.Rules.Portability", "DoNotHardcodePathsRule", Justification = "The path is part of a Uri and this usage is acceptable")]
		public void MatchesCredentials_ReturnsTrueOnMatchedResponseWithAuthenticationQualityOfProtection()
		{
			//sample from http://en.wikipedia.org/wiki/Digest_access_authentication
			string realm = "testrealm@host.com",
				opaque = "5ccc069c403ebaf9f0171e9517f40e41";

			var header = new DigestHeader()
			{
				Verb = "GET",
				UserName = "Mufasa",
				Realm = realm,
				Nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093",
				Uri = "/dir/index.html",
				QualityOfProtection = DigestQualityOfProtectionType.Authentication,
				RequestCounter = 1,
				ClientNonce = "0a4f113b",
				Opaque = opaque,
				Response = "6629fae49393a05397450978507c4ef1"
			};

			Assert.True(header.MatchesCredentials(realm, opaque, "Circle Of Life"));
		}

		[Fact]
		[SuppressMessage("Gendarme.Rules.Portability", "DoNotHardcodePathsRule", Justification = "The path is part of a Uri and this usage is acceptable")]
		public void MatchesCredentials_ReturnsTrueOnMatchedResponseWithNoQualityOfProtection()
		{
			//sample from http://en.wikipedia.org/wiki/Digest_access_authentication
			string realm = "testrealm@host.com",
				opaque = "5ccc069c403ebaf9f0171e9517f40e41";

			var header = new DigestHeader()
			{
				Verb = "GET",
				UserName = "Mufasa",
				Realm = realm,
				Nonce = "dcd98b7102dd2f0e8b11d0f600bfb0c093",
				Uri = "/dir/index.html",
				QualityOfProtection = DigestQualityOfProtectionType.Unspecified,
				RequestCounter = 1,
				ClientNonce = "0a4f113b",
				Opaque = opaque,
				//test response for unspecified qop calculated here http://md5-hash-online.waraxe.us/
				Response = "670fd8c2df070c60b045671b8b24ff02"
			};

			Assert.True(header.MatchesCredentials(realm, opaque, "Circle Of Life"));
		}

		[Fact(Skip = "Create some funky strings to test and make sure they pass/fail")]
		public void MatchesCredentials_AdvancedScenarios()
		{ }
	}
}