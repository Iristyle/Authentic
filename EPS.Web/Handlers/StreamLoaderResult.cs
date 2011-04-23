using System;
using System.IO;

namespace EPS.Web.Handlers
{
	/// <summary>   Represents the details of a file returned through the <see cref="T:EPS.Web.Handlers.IFileHttpHandlerStreamLoader"/>. </summary>
	/// <remarks>   ebrown, 2/9/2011. </remarks>
	public class StreamLoaderResult : IDisposable
	{
		private bool _disposed;

		/// <summary>   Gets the status of the request. </summary>
		/// <value> The status. </value>
		public StreamLoadStatus Status { get; private set; }

		/// <summary>   Gets the Content-Type that can be used in a HTTP response. </summary>
		/// <value> The type of the content. </value>
		public string ContentType { get; private set; }

		/// <summary>	Gets the metadata associated with the Stream. </summary>
		/// <value>	The metadata. </value>
		public StreamMetadata Metadata { get; private set; }

		/// <summary>   Gets the Uri in the cloud where this file is located if there is no direct stream access. </summary>
		/// <value> The cloud location. </value>
		public Uri CloudLocation { get; private set; }

		/// <summary>   Gets the actual stream for the file. </summary>
		/// <value> The file stream. </value>
		public Stream FileStream { get; private set; }

		/// <summary>
		/// Initializes a new instance of the FileDetails class.
		/// </summary>
		public StreamLoaderResult(StreamLoadStatus status, StreamMetadata streamMeta, string contentType, Uri cloudLocation, Stream fileStream)
		{
			Status = status;
			Metadata = streamMeta;
			ContentType = contentType;
			//MimeTypes.GetMimeTypeForFileExtension(Path.GetExtension(FileName));
			CloudLocation = cloudLocation;
			FileStream = fileStream;
		}

		/// <summary>	Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		/// <remarks>	ebrown, 4/22/2011. </remarks>
		public void Dispose()
		{
			if (!this._disposed)
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>	Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
		/// <remarks>	ebrown, 4/22/2011. </remarks>
		/// <param name="disposing">	true if resources should be disposed, false if not. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._disposed = true;
			}
		}

		//TODO: other details we might want at some point
		/* 
		public int FileId;
		public bool IsRecordValid;
		public bool IsAnonymous;
		public bool Exists;
		public bool IsDownloadEnabledForFile;
		public string DatabasePath;
		public string Path;
		public string ActualMD5;
		public bool IsLocalPath;
		 */
	}
}