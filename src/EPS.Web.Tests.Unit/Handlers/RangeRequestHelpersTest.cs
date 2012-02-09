using System;
using System.Linq;
using Xunit;

namespace EPS.Web.Handlers.Tests.Unit
{
    public class RangeRequestHelpersTest
    {
        [Fact]
        public void IsPartialOrMultipleRangeRequests_ThrowsOnNullRangeRequests()
        {
            Assert.Throws<ArgumentNullException>(() => RangeRequestHelpers.IsPartialOrMultipleRangeRequests(null, 0));
        }

        [Fact(Skip = "Not implemented")]
        public void IsPartialOrMultipleRangeRequests_True()
        {
        }

        [Fact(Skip = "Not implemented")]
        public void IsPartialOrMultipleRangeRequests_False()
        {
        }


        [Fact]
        public void IsMultipartRequest_ThrowsOnNullRangeRequests()
        {
            Assert.Throws<ArgumentNullException>(() => RangeRequestHelpers.IsMultipartRequest(null));
        }

        [Fact(Skip = "Not implemented")]
        public void IsMultipartRequest_True()
        {
        }
        
        [Fact(Skip = "Not implemented")]
        public void IsMultipartRequest_False()
        {
        }
    }
}