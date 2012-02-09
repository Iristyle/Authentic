using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
	public class NonceManagerTest
	{
		private static string key = "MyPrivateKey";
		private static string validNonce = "NjM0MzM4ODg3Nzc1MzEuMzpmZDIxNzllOTUzMDY2ODc2YWQyYjY1NmVmZGJkYTc4MQ==";
		private static string ipAddress = "192.168.16.1";
		private PrivateHashEncoder privateHashEncoder;

		public NonceManagerTest()
		{
			privateHashEncoder = new PrivateHashEncoder(key);
		}

		[SuppressMessage("Gendarme.Rules.Concurrency", "WriteStaticFieldFromInstanceMethodRule", Justification = "NonceManager.Now is intended to only be used internally by tests, and as such is OK")]
		private void FreezeNonceClock()
		{
			//freeze the clock 
			DateTime now = DateTime.UtcNow;
			NonceManager.Now = () => now;
		}

		[SuppressMessage("Gendarme.Rules.Concurrency", "WriteStaticFieldFromInstanceMethodRule", Justification = "NonceManager.Now is intended to only be used internally by tests, and as such is OK")]
		private void ThawNonceClock()
		{
			NonceManager.Now = () => { return DateTime.UtcNow; };
		}

		[Fact]
		public void Generate_ThrowsOnNullPrivateHashEncoderNotInitialized()
		{
			Assert.Throws<ArgumentNullException>(() => NonceManager.Generate("127.0.0.1", null));
		}

		[Fact]
		public void Validate_ThrowsOnNullPrivateHashEncoderNotInitialized()
		{
			//this is a valid nonce conforming to our standards
			Assert.Throws<ArgumentNullException>(() => NonceManager.Validate("NjM0MzM4ODg3Nzc1MzEuMzpmZDIxNzllOTUzMDY2ODc2YWQyYjY1NmVmZGJkYTc4MQ==", "127.0.0.1", null));
		}

		[Fact]
		public void Generate_ThrowsOnNullIPAddress()
		{
			Assert.Throws<ArgumentNullException>(() => NonceManager.Generate(null, privateHashEncoder));
		}

		[Fact]
		public void Generate_ThrowsOnEmptyIPAddress()
		{
			Assert.Throws<ArgumentException>(() => NonceManager.Generate(string.Empty, privateHashEncoder));
		}

		private string GenerateNonce()
		{
			return NonceManager.Generate(ipAddress, privateHashEncoder);
		}

		[Fact]
		public void Generate_CanRoundtripThroughValidate()
		{
			FreezeNonceClock();

			string nonce = GenerateNonce();
			bool match = NonceManager.Validate(nonce, ipAddress, privateHashEncoder);

			ThawNonceClock();

			Assert.True(match);
		}

		[Fact]
		public void Generate_ProducesSameNonceAtFrozenTime()
		{
			FreezeNonceClock();

			string firstNonce = GenerateNonce();
			string secondNonce = GenerateNonce();

			ThawNonceClock();
			Assert.Equal(firstNonce, secondNonce);
		}

		[Fact]
		[SuppressMessage("Gendarme.Rules.Concurrency", "WriteStaticFieldFromInstanceMethodRule", Justification = "NonceManager.Now is intended to only be used internally by tests, and as such is OK")]
		public void Generate_ProducesDifferentNonceValuesOverElapsedTime()
		{
			string firstNonce = GenerateNonce();

			NonceManager.Now = () => DateTime.UtcNow.AddHours(1);

			string secondNonce = GenerateNonce();

			ThawNonceClock();

			Assert.NotEqual(firstNonce, secondNonce);
		}

		[Fact]
		public void Validate_ThrowsOnNullIPAddress()
		{
			Assert.Throws<ArgumentNullException>(() => NonceManager.Validate(validNonce, null, privateHashEncoder));
		}

		[Fact]
		public void Validate_ThrowsOnEmptyIPAddress()
		{
			Assert.Throws<ArgumentException>(() => NonceManager.Validate(validNonce, string.Empty, privateHashEncoder));
		}

		[Fact]
		public void Validate_ThrowsOnNullNonce()
		{
			Assert.Throws<ArgumentNullException>(() => NonceManager.Validate(null, ipAddress, privateHashEncoder));
		}

		[Fact]
		public void Validate_ThrowsOnEmptyNonce()
		{
			Assert.Throws<ArgumentException>(() => NonceManager.Validate(string.Empty, ipAddress, privateHashEncoder));
		}

		[Fact]
		public void Validate_MatchesPreCalculatedNonce()
		{
			Assert.True(NonceManager.Validate(validNonce, ipAddress, privateHashEncoder));
		}

		[Fact]
		public void IsStale_ThrowsOnNullNonce()
		{
			Assert.Throws<ArgumentNullException>(() => NonceManager.IsStale(null, TimeSpan.FromSeconds(30)));
		}

		[Fact]
		public void IsStale_ThrowsOnEmptyNonce()
		{
			Assert.Throws<ArgumentException>(() => NonceManager.IsStale(string.Empty, TimeSpan.FromSeconds(30)));
		}

		[Fact]
		public void IsStale_FalseOnCurrentNonce()
		{
			FreezeNonceClock();

			string nonce = GenerateNonce();
			bool stale = NonceManager.IsStale(nonce, TimeSpan.FromSeconds(1));

			ThawNonceClock();

			Assert.False(stale);
		}

		[Fact]
		public void IsStale_TrueOnZeroSeconds()
		{
			string nonce = GenerateNonce();
			Thread.Sleep(2);
			Assert.True(NonceManager.IsStale(nonce, TimeSpan.FromSeconds(0)));
		}

		[Fact]
		public void IsStale_TrueOnOldPreCalculatedNonce()
		{
			Assert.True(NonceManager.IsStale(validNonce, TimeSpan.FromSeconds(0)));
		}
	}
}