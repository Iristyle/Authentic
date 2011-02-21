using System;
using System.Collections.Specialized;
using System.Web;
using EPS.Text;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Extensions.Tests.Unit
{
    public class HttpRequestBaseExtensionsTest
    {
        [Fact(Skip = "Tests should be easy against extension methods")]
        public void IsMobileDeviceOrConfiguredMobile_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void GetCookies_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void HasRangeHeaders_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void TryParseRangeRequests_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void IfRangeHeaderIsValid_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void IfMatchHeaderIsValid_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void IfNoneMatchHeaderIsWildcard_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void IfNoneMatchHeaderHasMatchingETagSpecified_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void IfModifiedSinceHeaderIsBeforeGiven_Test()
        {
        }

        [Fact(Skip = "Tests should be easy against extension methods")]
        public void IfUnmodifiedHeaderIsAfterGivenDate_Test()
        {
        }

        [Fact]
        public void RetrieveHeader_ThrowsOnNullRequest()
        {
            HttpRequestBase request = null;
            Assert.Throws<ArgumentNullException>(() => request.RetrieveHeader(HttpHeaderFields.WwwAuthenticate));
        }

        [Fact]
        public void RetrieveHeader_ThrowsOnBadEnum()
        {
            HttpRequestBase request = A.Fake<HttpRequestBase>();
            Assert.Throws<ArgumentException>(() => request.RetrieveHeader((HttpHeaderFields)3));
        }

        [Fact]
        public void RetrieveHeader_ThrowsOnEnumBitmask()
        {
            HttpRequestBase request = A.Fake<HttpRequestBase>();
            Assert.Throws<ArgumentException>(() => request.RetrieveHeader(HttpHeaderFields.Warning | HttpHeaderFields.TE));
        }

        [Fact]
        public void RetrieveHeader_ReturnsEmptyStringOnMissingHeader()
        {
            HttpRequestBase request = A.Fake<HttpRequestBase>();
            Assert.Equal(string.Empty, request.RetrieveHeader(HttpHeaderFields.From));
        }

        [Fact]
        public void RetrieveHeader_ReturnsCorrectHeaderText()
        {
            HttpRequestBase request = A.Fake<HttpRequestBase>();
            NameValueCollection values = new NameValueCollection();
            foreach (HttpHeaderFields headerField in Enum.GetValues(typeof(HttpHeaderFields)))
                values.Add(headerField.ToEnumValueString(), headerField.ToValueString());

            A.CallTo(() => request.Headers).Returns(values);

            //pick one at random to verify
            Assert.Equal(HttpHeaderFields.EntityTag.ToValueString(), request.RetrieveHeader(HttpHeaderFields.EntityTag));
        }

        [Fact]
        public void RetrieveHeader_StripsQuotesFromHeaderText()
        {
            HttpRequestBase request = A.Fake<HttpRequestBase>();
            NameValueCollection values = new NameValueCollection();
            values.Add(HttpHeaderFields.Authorization.ToEnumValueString(), @"""Test ""Data""");
            A.CallTo(() => request.Headers).Returns(values);

            //pick one at random to verify
            Assert.Equal(@"Test Data", request.RetrieveHeader(HttpHeaderFields.Authorization));
        }
    }
}
