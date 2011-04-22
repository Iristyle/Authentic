using System;

namespace EPS.Web.Handlers
{
	/// <summary>	Stream metadata that describes a given stream. </summary>
	/// <remarks>	ebrown, 4/22/2011. </remarks>
	public class StreamMetadata
	{
		/// <summary>   Gets the filename of the file. </summary>
		/// <value> The name of the file. </value>
		public string FileName { get; private set; }

		/// <summary>   Gets the anticipated MD5 checksum when this file was originally uploaded.  Can be used to verify the stream before sending. </summary>
		/// <value> A 32 character MD5 string. </value>
		public string ExpectedMD5 { get; private set; }

		/// <summary>   Gets the size of the file when originally uploaded. </summary>
		/// <value> The size. </value>
		public long? Size { get; private set; }

		/// <summary>   Gets the Date/Time of the last write time in UTC time. </summary>
		/// <value> The last write time in UTC format. </value>
		public DateTime? LastWriteTimeUtc { get; private set; }

		/// <summary>
		/// Initializes a new instance of the StreamMeta class.
		/// </summary>
		public StreamMetadata(string fileName, string expectedMD5, long? size, DateTime? lastWriteTimeUtc)
		{
			FileName = fileName;
			ExpectedMD5 = expectedMD5;
			Size = size;
			LastWriteTimeUtc = lastWriteTimeUtc;
		}
	}
}