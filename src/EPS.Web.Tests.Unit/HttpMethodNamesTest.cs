using System;
using EPS.Annotations;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class HttpMethodNamesTest
    {
        [Fact]
        public void Get_HasProperEnumValue()
        {
            Assert.Equal("GET", HttpMethodNames.Get.ToEnumValueString());
        }

        [Fact]
        public void Header_HasProperEnumValue()
        {
            Assert.Equal("HEAD", HttpMethodNames.Header.ToEnumValueString());
        }

        [Fact]
        public void Put_HasProperEnumValue()
        {
            Assert.Equal("PUT", HttpMethodNames.Put.ToEnumValueString());
        }

        [Fact]
        public void Post_HasProperEnumValue()
        {
            Assert.Equal("POST", HttpMethodNames.Post.ToEnumValueString());
        }

        [Fact]
        public void Delete_HasProperEnumValue()
        {
            Assert.Equal("DELETE", HttpMethodNames.Delete.ToEnumValueString());
        }

        [Fact]
        public void Trace_HasProperEnumValue()
        {
            Assert.Equal("TRACE", HttpMethodNames.Trace.ToEnumValueString());
        }

        [Fact]
        public void Options_HasProperEnumValue()
        {
            Assert.Equal("OPTIONS", HttpMethodNames.Options.ToEnumValueString());
        }
    }
}
