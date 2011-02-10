using System;
using Xunit;

namespace EPS.Web.Tests.Unit
{
    public class MimeTypesTest
    {
        [Fact]
        public void GetMimeTypeForFileExtension_ReturnsExpectedMimeTypeForHtml()
        {
            Assert.Same("text/HTML", MimeTypes.GetMimeTypeForFileExtension(".htm"));
        }

        [Fact]
        public void GetMimeTypeForFileExtension_ReturnsDefaultOfApplicationOctetStreamForUnregisteredMimeType()
        {
            Assert.Same("application/octet-stream", MimeTypes.GetMimeTypeForFileExtension(".poop"));
        }

        [Fact]
        public void GetMimeTypeForFileExtension_ReturnsUserSuppliedForUnregisteredMimeType()
        {
            string userMimeType = "poopy";
            Assert.Same(userMimeType, MimeTypes.GetMimeTypeForFileExtension(".poop", userMimeType));
        }

        [Fact]
        public void GetMimeTypeForFileExtension_ThrowsOnNullExtension()
        {
            Assert.Throws<ArgumentNullException>(() => MimeTypes.GetMimeTypeForFileExtension(null));
        }
        [Fact]
        public void GetMimeTypeForFileExtension_ThrowsOnWhitespaceExtension()
        {
            Assert.Throws<ArgumentException>(() => MimeTypes.GetMimeTypeForFileExtension(""));
        }

        [Fact]
        public void GetMimeTypeForFileExtension_ThrowsOnNullDefault()
        {
            Assert.Throws<ArgumentNullException>(() => MimeTypes.GetMimeTypeForFileExtension(".htm", null));
        }

        [Fact]
        public void GetMimeTypeForFileExtension_ThrowsOnWhitespaceDefault()
        {
            Assert.Throws<ArgumentException>(() => MimeTypes.GetMimeTypeForFileExtension(".htm", ""));
        }
    }
}