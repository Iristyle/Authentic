using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Digest.Configuration;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
	public class DigestFailureHandlerTest
	{
		class MockConfiguration : IDigestFailureHandlerConfiguration
		{
			public string Realm { get; set; }
			public string PrivateKey { get; set; }
			public TimeSpan NonceValidDuration { get; set; }
			public bool RequireSsl { get; set; }
		}

		[Fake]
		HttpContextBase context;
		[Fake]
		IDigestFailureHandlerConfiguration configuration;
		string privateKey = "MyPrivateKey";
		string realm = "test@test.com";
		string ipAddress = "127.0.0.1";
		PrivateHashEncoder privateHashEncoder;
		DigestFailureHandler failureHandler;

		Dictionary<IAuthenticator, AuthenticationResult> inspectorResults
			= new Dictionary<IAuthenticator, AuthenticationResult>();

		public DigestFailureHandlerTest()
		{
			Fake.InitializeFixture(this);
			//our classes guard against a null .ApplicationInstance
			A.CallTo(() => context.ApplicationInstance).Returns(null);
			A.CallTo(() => configuration.Realm).Returns(realm);
			A.CallTo(() => configuration.PrivateKey).Returns(privateKey);
			A.CallTo(() => configuration.NonceValidDuration).Returns(TimeSpan.FromSeconds(30));
			privateHashEncoder = new PrivateHashEncoder(privateKey);
			failureHandler = new DigestFailureHandler(configuration);
		}

		private void FreezeNonceClock()
		{
			//freeze the clock 
			DateTime now = DateTime.UtcNow;
			NonceManager.Now = () => now;
		}

		private void ThawNonceClock()
		{
			NonceManager.Now = () => { return DateTime.UtcNow; };
		}

		[Fact]
		public void Constructor_ThrowsOnNullConfiguration()
		{
			Assert.Throws<ArgumentNullException>(() => new DigestFailureHandler(null));
		}

		[Fact]
		public void Constructor_ThrowsOnNullPrivateKey()
		{
			var configuration = new MockConfiguration() { PrivateKey = null, NonceValidDuration = TimeSpan.FromMinutes(10), Realm = "realm", RequireSsl = false };
			Assert.Throws<ArgumentNullException>(() => new DigestFailureHandler(configuration));
		}

		[Fact]
		public void Constructor_ThrowsOnEmptyPrivateKey()
		{
			var configuration = new MockConfiguration() { PrivateKey = string.Empty, NonceValidDuration = TimeSpan.FromMinutes(10), Realm = "realm", RequireSsl = false };
			Assert.Throws<ArgumentException>(() => new DigestFailureHandler(configuration));
		}

		[Fact]
		public void Constructor_ThrowsOnPrivateKeyTooShort()
		{
			var configuration = new MockConfiguration() { PrivateKey = "1234567", NonceValidDuration = TimeSpan.FromMinutes(10), Realm = "realm", RequireSsl = false };
			Assert.Throws<ArgumentException>(() => new DigestFailureHandler(configuration));
		}

		[Fact]
		public void Constructor_ThrowsOnNullRealm()
		{
			var configuration = new MockConfiguration() { PrivateKey = "12345678", NonceValidDuration = TimeSpan.FromMinutes(10), Realm = null, RequireSsl = false };
			Assert.Throws<ArgumentNullException>(() => new DigestFailureHandler(configuration));
		}

		[Fact]
		public void Constructor_ThrowsOnEmptyRealm()
		{
			var configuration = new MockConfiguration() { PrivateKey = "12345678", NonceValidDuration = TimeSpan.FromMinutes(10), Realm = string.Empty, RequireSsl = false };
			Assert.Throws<ArgumentException>(() => new DigestFailureHandler(configuration));
		}

		[Fact]
		public void Constructor_ThrowsOnTimeSpanTooShort()
		{
			var configuration = new MockConfiguration() { PrivateKey = "12345678", NonceValidDuration = TimeSpan.FromSeconds(19), Realm = "realm", RequireSsl = false };
			Assert.Throws<ArgumentException>(() => new DigestFailureHandler(configuration));
		}

		[Fact]
		public void Constructor_ThrowsOnTimeSpanTooLong()
		{
			var configuration = new MockConfiguration() { PrivateKey = "12345678", NonceValidDuration = TimeSpan.FromMinutes(61), Realm = "realm", RequireSsl = false };
			Assert.Throws<ArgumentException>(() => new DigestFailureHandler(configuration));
		}

		[Fact]
		public void OnAuthenticationFailure_ThrowsOnNullContext()
		{
			var failureHandler = new DigestFailureHandler(configuration);
			Assert.Throws<ArgumentNullException>(() => failureHandler.OnAuthenticationFailure(null, inspectorResults));
		}

		[Fact]
		public void OnAuthenticationFailure_GeneratesCorrectHeaderForNewRequest()
		{
			FreezeNonceClock();
			A.CallTo(() => context.Request.HttpMethod).Returns("GET");
			A.CallTo(() => context.Request.UserHostAddress).Returns(ipAddress);

			//record the values from the AddHeader call and make sure they match our expectations
			string headerName = string.Empty, headerValue = string.Empty;
			A.CallTo(() => context.Response.AddHeader(A<string>.Ignored, A<string>.Ignored))
				.Invokes(call => { headerName = (string)call.Arguments[0]; headerValue = (string)call.Arguments[1]; });

			failureHandler.OnAuthenticationFailure(context, inspectorResults);

			string expectedHeader = String.Format(CultureInfo.InvariantCulture,
				"Digest realm=\"{0}\", nonce=\"{1}\", opaque=\"{2}\", stale=FALSE, algorithm=MD5, qop=\"auth\"",
				realm, NonceManager.Generate(ipAddress, privateHashEncoder), Opaque.Current());

			ThawNonceClock();

			Assert.Equal(headerName, "WWW-Authenticate");
			Assert.Equal(expectedHeader, headerValue);
		}

		[Fact]
		public void OnAuthenticationFailure_RecognizesAndReportsStaleNonce()
		{
			string nonce = NonceManager.Generate(ipAddress, privateHashEncoder);
			A.CallTo(() => context.Request.HttpMethod).Returns("GET");
			A.CallTo(() => context.Request.UserHostAddress).Returns(ipAddress);

			var headers = new NameValueCollection() { { "Authorization", string.Format(
@"Digest username=""Mufasa"",realm=""testrealm@host.com"",
                     nonce=""{0}"",
                     uri=""/dir/index.html"",qop=auth,nc=00000001,cnonce=""0a4f113b"",
                     response=""6629fae49393a05397450978507c4ef1"",
                     opaque=""5ccc069c403ebaf9f0171e9517f40e41""", nonce) } };

			A.CallTo(() => context.Request.Headers).Returns(headers);

			//jump ahead just outside the reach of our configuration
			DateTime now = DateTime.UtcNow;
			NonceManager.Now = () => now + configuration.NonceValidDuration.Add(TimeSpan.FromSeconds(1));

			//record the values from the AddHeader call and make sure they match our expectations
			string headerName = string.Empty, headerValue = string.Empty;
			A.CallTo(() => context.Response.AddHeader(A<string>.Ignored, A<string>.Ignored))
				.Invokes(call => { headerName = (string)call.Arguments[0]; headerValue = (string)call.Arguments[1]; });

			failureHandler.OnAuthenticationFailure(context, inspectorResults);

			string expectedHeader = String.Format(CultureInfo.InvariantCulture,
				"Digest realm=\"{0}\", nonce=\"{1}\", opaque=\"{2}\", stale=TRUE, algorithm=MD5, qop=\"auth\"",
				realm, NonceManager.Generate(ipAddress, privateHashEncoder), Opaque.Current());

			ThawNonceClock();

			Assert.Equal(headerName, "WWW-Authenticate");
			Assert.Equal(expectedHeader, headerValue);
		}

		[Fact(Skip = "Simulate some other missing headers and make sure we handle properly")]
		public void OnAuthenticationFailure_HandlesBadHeaders()
		{
		}
	}
}
