using System;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class RangeRequestTest
    {
        [Fact]
        public void Constructor_StartGreaterThanEnd_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RangeRequest(1, 0, 1));
        }

        [Fact]
        public void Constructor_StartLessThanZero_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RangeRequest(-1, 1, 5));
        }

        [Fact]
        public void Constructor_RangeGreaterThanAvailable_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new RangeRequest(0, 20, 20));
        }

        [Fact]
        public void Constructor_PartialRange_Valid()
        {
            var range = new RangeRequest(0, 4, 20);
            Assert.True(range.Start == 0 && range.End == 4 && range.Maximum == 20);
        }

        [Fact]
        public void Constructor_CompleteRange_Valid()
        {
            int max = 10;
            var range = new RangeRequest(0, max - 1, max);
            Assert.True(range.Start == 0 && range.End == max - 1 && range.Maximum == max);
        }

        [Fact]
        public void GetMultipartIntermediateHeader_Valid()
        {
            var rangeRequest = new RangeRequest(0, 511, 512);

            var expected = @"--0000_1111_Multipart_Boundary_1111_0000
Content-Type: Test
Content-Range: bytes 0-511/512

";
            Assert.Equal(expected, rangeRequest.GetMultipartIntermediateHeader("Test"));
        }

        [Fact]
        public void GetMultipartIntermediateHeader_ThrowsOnNullContentType()
        {
            var rangeRequest = new RangeRequest(0, 1, 2);
            Assert.Throws<ArgumentNullException>(() => rangeRequest.GetMultipartIntermediateHeader(null));
        }

        [Fact]
        public void GetMultipartIntermediateHeader_ThrowsOnWhiteSpaceContentType()
        {
            var rangeRequest = new RangeRequest(0, 1, 2);
            Assert.Throws<ArgumentException>(() => rangeRequest.GetMultipartIntermediateHeader(string.Empty));
        }
    }
}