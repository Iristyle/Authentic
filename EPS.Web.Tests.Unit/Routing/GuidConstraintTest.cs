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

        [Fact]
        public void Match_TrueOnValidGuidAsNFormattedString()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString("N"));
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsDFormattedString()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString("D"));
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsBFormattedString()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString("B"));
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsPFormattedString()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString("N"));
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_TrueOnValidGuidAsXFormattedString()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString("X"));
            Assert.True(new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
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
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString());
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(null, A.Dummy<Route>(), "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnNullRoute()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString());
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), null, "parameter", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnNullParameterName()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString());
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), null, dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnWhitespaceParameterName()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString());
            Assert.Throws<ArgumentException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "\t", dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnEmptyParameterName()
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("parameter", Guid.NewGuid().ToString());
            Assert.Throws<ArgumentException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), string.Empty, dictionary, RouteDirection.IncomingRequest));
        }

        [Fact]
        public void Match_ThrowsOnNullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() => new GuidConstraint().Match(A.Dummy<HttpContextBase>(), A.Dummy<Route>(), "parameter", null, RouteDirection.IncomingRequest));
        }
    }
}