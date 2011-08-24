using System;
using System.Collections.Generic;
using EPS.Utility;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Tests.Unit
{
	public class HttpDigestAuthHeaderParserTest
	{
		private IEqualityComparer<DigestHeader> comparer = new GenericEqualityComparer<DigestHeader>
			((x, y) => MemberComparer.Equal(x, y));

		[Fact]
		public void TryExtractDigestHeader_ReturnsFalseWithNullDigestHeaderInstance_OnNullString()
		{
			DigestHeader digestHeader;
			bool status = HttpDigestAuthHeaderParser.TryExtractDigestHeader("GET", null, out digestHeader);
			Assert.True(status == false && null == digestHeader);
		}

		[Fact]
		public void TryExtractDigestHeader_ReturnsFalseWithNullDigestHeaderInstance_OnEmptyString()
		{
			DigestHeader digestHeader;
			bool status = HttpDigestAuthHeaderParser.TryExtractDigestHeader("GET", string.Empty, out digestHeader);
			Assert.True(status == false && null == digestHeader);
		}

		[Fact]
		public void ExtractDigestHeader_ThrowsOnNullString()
		{
			Assert.Throws<ArgumentNullException>(() => HttpDigestAuthHeaderParser.ExtractDigestHeader("GET", null));
		}

		[Fact]
		public void ExtractDigestHeader_ThrowsOnInvalidHttpMethodName()
		{
			Assert.Throws<ArgumentException>(() => HttpDigestAuthHeaderParser.ExtractDigestHeader("GOO", string.Empty));
		}

		[Fact]
		public void ExtractDigestHeader_ThrowsOnEmptyString()
		{
			Assert.Throws<ArgumentException>(() => HttpDigestAuthHeaderParser.ExtractDigestHeader("GET", string.Empty));
		}

		[Fact]
		public void ExtractDigestHeader_ThrowsOnBasicAuthTypeString()
		{
			Assert.Throws<ArgumentException>(() => HttpDigestAuthHeaderParser.ExtractDigestHeader("GET", @"Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ=="));
		}

		[Fact]
		[SuppressMessage("Gendarme.Rules.Portability", "DoNotHardcodePathsRule", Justification = "The path is part of a Uri and this usage is acceptable")]
		public void ExtractDigestHeader_ParsesCompleteHeaderCorrectly()
		{
			string verb = "GET";
			string header = @"Digest username=""Mufasa"",
realm=""testrealm@host.com"",
nonce=""QWxhZGRpbjpvcGVuIHNlc2FtZQ=="",
uri=""/dir/index.html"",
qop=auth,
nc=00000001,
cnonce=""0a4f113b"",
response=""6629fae49393a05397450978507c4ef1"",
opaque=""5ccc069c403ebaf9f0171e9517f40e41""";

			DigestHeader expectedHeader = new DigestHeader()
			{
				Verb = verb,
				ClientNonce = "0a4f113b",
				Nonce = "QWxhZGRpbjpvcGVuIHNlc2FtZQ==",
				Opaque = "5ccc069c403ebaf9f0171e9517f40e41",
				QualityOfProtection = DigestQualityOfProtectionType.Authentication,
				Realm = "testrealm@host.com",
				RequestCounter = 1,
				Response = "6629fae49393a05397450978507c4ef1",
				Uri = "/dir/index.html",
				UserName = "Mufasa"
			};

			Assert.Equal(expectedHeader, HttpDigestAuthHeaderParser.ExtractDigestHeader(verb, header), comparer);
		}

		[Fact(Skip = "Need to write some more advanced parsing tests as the above example is entirely insufficient")]
		public void ExtractDigestHeader_AdvancedTests()
		{
		}
	}
}
