using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace EPS.Web.Authentication.Digest.Tests.Unit
{
    public class NonceTimeStampParserTest
    {
        [Fact]
        public void Parse_CorrectlyParsesNonceTimeStamp()
        {
            DateTime parsedDateTime = NonceTimeStampParser.Parse("63412971825005");
            Assert.Equal(parsedDateTime, new DateTime(2010, 06, 24, 10, 23, 45, 5));
        }

        [Fact]
        public void Parse_ThrowsOnNullString()
        {
            Assert.Throws<ArgumentNullException>(() => NonceTimeStampParser.Parse(null));
        }

        [Fact]
        public void Parse_ThrowsOnEmptyString()
        {
            Assert.Throws<ArgumentException>(() => NonceTimeStampParser.Parse(string.Empty));
        }

        [Fact]
        public void Parse_ThrowsOnInvalidString()
        {
            Assert.Throws<ArgumentException>(() => NonceTimeStampParser.Parse("blah"));
        }
    }
}
