using System;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
	public class PrivateHashEncoderTest
	{
		private string privateKey = "MyPrivateKey";
		private string address = "127.0.0.1";
		private string milliseconds = "134213";
		private PrivateHashEncoder standardEncoder;

		public PrivateHashEncoderTest()
		{
			standardEncoder = new PrivateHashEncoder(privateKey);
		}

		[Fact]
		public void Encode_ProperlyHashesFromGivenKeyAndAddress()
		{
			//result provided by http://md5-hash-online.waraxe.us/
			Assert.Equal("6d65215c55ae31e224cd5c853103e6ee", standardEncoder.Encode(milliseconds, address));
		}

		[Fact]
		public void Constructor_ThrowsWhenPrivateKeyNull()
		{
			Assert.Throws<ArgumentNullException>(() => new PrivateHashEncoder(null));
		}

		[Fact]
		public void Encode_ThrowsWhenMillisecondsNull()
		{
			Assert.Throws<ArgumentNullException>(() => standardEncoder.Encode(null, address));
		}

		[Fact]
		public void Encode_ThrowsWhenMillisecondsEmpty()
		{
			Assert.Throws<ArgumentException>(() => standardEncoder.Encode(string.Empty, address));
		}

		[Fact]
		public void Encode_ThrowsWhenAddressNull()
		{
			Assert.Throws<ArgumentNullException>(() => standardEncoder.Encode(milliseconds, null));
		}

		[Fact]
		public void Encode_ThrowsWhenAddressEmpty()
		{
			Assert.Throws<ArgumentException>(() => standardEncoder.Encode(milliseconds, string.Empty));
		}

		[Fact(Skip = "Provide some tests that show that all the values are used in the resultant hash")]
		public void Encode_UsesAllSuppliedValues()
		{
		}
	}
}
