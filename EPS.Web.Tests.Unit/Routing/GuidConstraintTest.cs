using System;
using System.Web;
using System.Web.Routing;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Routing.Tests.Unit
{
    public class GuidConstraintTest
    {
        [Fact]
        public void Match_TrueOnValidGuid()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid());
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        private static RouteValueDictionary GetGuidStringDictionary(string format)
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString(format));
            return dictionary;
        }

        [Fact]
        public void Match_TrueOnValidGuidAsNFormattedString()
        {
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", GetGuidStringDictionary("N"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsDFormattedString()
        {
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", GetGuidStringDictionary("D"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsBFormattedString()
        {
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", GetGuidStringDictionary("B"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsPFormattedString()
        {
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", GetGuidStringDictionary("P"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsXFormattedString()
        {
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", GetGuidStringDictionary("X"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_FalseOnEmptyGuid()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.Empty);
            Assert.False(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_FalseOnEmptyGuidAsString()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.Empty.ToString());
            Assert.False(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_FalseOnNullParameter()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", null);
            Assert.False(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_FalseOnBadlyFormedGuidParameter()
        {
            var dictionary = new RouteValueDictionary();
            //looks like a Guid, but GH is invalid
            dictionary.Add("parameter", "{ABCDEFGH-1234-1234-1234-ABCDEF123456}");
            Assert.False(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_FalseOnNonGuidParameter()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", new object());
            Assert.False(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_FalseOnEmptyDictionary()
        {
            var dictionary = new RouteValueDictionary();
            Assert.False(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnNullHttpContext()
        {
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(null, A.Dummy<Route>(), "parameter", GetGuidStringDictionary("D"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnNullRoute()
        {
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), null, "parameter", GetGuidStringDictionary("D"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnNullParameterName()
        {
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), null, GetGuidStringDictionary("D"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnWhiteSpaceParameterName()
        {
            Assert.Throws<ArgumentException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "\t", GetGuidStringDictionary("D"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnEmptyParameterName()
        {
            Assert.Throws<ArgumentException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), string.Empty, GetGuidStringDictionary("D"), RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnNullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", null, RouteDirection.IncomingRequest));
        }
    }
}