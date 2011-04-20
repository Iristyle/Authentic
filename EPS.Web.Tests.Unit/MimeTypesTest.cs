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
		public void GetMimeTypeForFileExtension_ReturnsExpectedMimeTypeForJpg()
		{
			Assert.Same("image/jpeg", MimeTypes.GetMimeTypeForFileExtension(".jpg"));
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
        public void GetMimeTypeForFileExtension_ThrowsOnWhiteSpaceExtension()
        {
            Assert.Throws<ArgumentException>(() => MimeTypes.GetMimeTypeForFileExtension(string.Empty));
        }

        [Fact]
        public void GetMimeTypeForFileExtension_ThrowsOnNullDefault()
        {
            Assert.Throws<ArgumentNullException>(() => MimeTypes.GetMimeTypeForFileExtension(".htm", null));
        }

        [Fact]
        public void GetMimeTypeForFileExtension_ThrowsOnWhiteSpaceDefault()
        {
            Assert.Throws<ArgumentException>(() => MimeTypes.GetMimeTypeForFileExtension(".htm", string.Empty));
        }
    }
}