using System;
using System.IO;

namespace EPS.Web.Handlers
{
    /// <summary>   Represents the details of a file returned through the <see cref="T:EPS.Web.Handlers.IFileHttpHandlerStreamLoader"/>. </summary>
    /// <remarks>   ebrown, 2/9/2011. </remarks>
    public class StreamLoaderResult
    {
        /// <summary>   Gets the status of the request. </summary>
        /// <value> The status. </value>
        public StreamLoadStatus Status { get; private set; }

        /// <summary>   Gets the filename of the file. </summary>
        /// <value> The name of the file. </value>
        public string FileName { get; private set; }
        
        /// <summary>   Gets the Content-Type that can be used in a HTTP response. </summary>
        /// <value> The type of the content. </value>
        public string ContentType { get; private set; }

        /// <summary>   Gets the anticipated MD5 checksum when this file was originally uploaded.  Can be used to verify the stream before sending. </summary>
        /// <value> A 32 character MD5 string. </value>
        public string ExpectedMD5 { get; private set; }
        
        /// <summary>   Gets the size of the file when originally uploaded. </summary>
        /// <value> The size. </value>
        public long? Size { get; private set; }

        /// <summary>   Gets the Date/Time of the last write time in UTC time. </summary>
        /// <value> The last write time in UTC format. </value>
        public DateTime? LastWriteTimeUtc { get; private set; }

        /// <summary>   Gets the Uri in the cloud where this file is located if there is no direct stream access. </summary>
        /// <value> The cloud location. </value>
        public Uri CloudLocation { get; private set; }
        
        /// <summary>   Gets the actual stream for the file. </summary>
        /// <value> The file stream. </value>
        public Stream FileStream { get; private set; }

        /// <summary>
        /// Initializes a new instance of the FileDetails class.
        /// </summary>
        public StreamLoaderResult(StreamLoadStatus status, string fileName, string contentType, string expectedMD5, long? size, Uri cloudLocation, DateTime? lastWriteTimeUtc, Stream fileStream)
        {
            Status = status;
            FileName = fileName;
            ContentType = contentType;
                //MimeTypes.GetMimeTypeForFileExtension(Path.GetExtension(FileName));
            ExpectedMD5 = expectedMD5;
            Size = size;
            CloudLocation = cloudLocation;
            LastWriteTimeUtc = lastWriteTimeUtc;
            FileStream = fileStream;
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