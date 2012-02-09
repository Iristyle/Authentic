using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
	public class NonceTimestampParserTest
	{
		[Fact]
		public void Parse_CorrectlyParsesNonceTimestamp()
		{
			DateTime parsedDateTime = NonceTimestampParser.Parse("63412971825005");
			Assert.Equal(parsedDateTime, new DateTime(2010, 06, 24, 10, 23, 45, 5));
		}

		[Fact]
		public void Parse_ThrowsOnNullString()
		{
			Assert.Throws<ArgumentNullException>(() => NonceTimestampParser.Parse(null));
		}

		[Fact]
		public void Parse_ThrowsOnEmptyString()
		{
			Assert.Throws<ArgumentException>(() => NonceTimestampParser.Parse(string.Empty));
		}

		[Fact]
		public void Parse_ThrowsOnInvalidString()
		{
			Assert.Throws<ArgumentException>(() => NonceTimestampParser.Parse("blah"));
		}
	}
}
