using System;
using System.Web;
using FakeItEasy;
using Xunit;

namespace EPS.Web.Handlers.Tests.Unit
{
    public class ResponseStreamWriterTest
    {
        [Fact]
        public void ResponseStreamWriter_ThrowsOnNullResponse()
        {
            Assert.Throws<ArgumentNullException>(() => new ResponseStreamWriter(null, 1234));
        }

        [Fact(Skip = "There are a ton of different scenarios here that test cases need to be written for")]
        public void StreamFile_StreamsHeadProperly()
        {
            var response = A.Fake<HttpResponseBase>();
			var responseStreamWriter = new ResponseStreamWriter(response, 512);
			StreamLoaderResult streamLoaderResult = new StreamLoaderResult(StreamLoadStatus.Success, new StreamMetadata("foo.bar", null, null, null), null, null, null);

            responseStreamWriter.StreamFile(HttpResponseType.HeadOnly, streamLoaderResult, null, null, false);
        }
    }
}
