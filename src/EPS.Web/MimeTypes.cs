using System;
using System.Collections.Generic;

namespace EPS.Web
{
	/// <summary>   A class that maintains a mapping of file extensions to mime types. </summary>
	/// <remarks>   ebrown, 2/9/2011. </remarks>
	public static class MimeTypes
	{
		//http://www.microsoft.com/technet/prodtechnol/isa/2004/plan/mimetypes.mspx
		//http://www.webmaster-toolkit.com/mime-types.shtml
		private static readonly Dictionary<string, string> mimeTypes =
			new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{ "htm", "text/HTML" },
				{ "html", "text/HTML" },
				{ "1st", "text/plain" },
				{ "txt", "text/plain" },
				{ "doc", "Application/msword" },
				{ "rtf", "Application/msword" },
				{ "xls", "application/excel" },
				{ "ps", "application/postscript" },
				{ "hqx", "application/mac-binhex40" },
				{ "sit", "application/x-stuffit" },
				{ "sitx", "application/x-stuffit" },
				{ "sea", "application/x-sea" },
				{ "dmg", "application/x-apple-diskimage" },
				{ "msi", "application/x-msi" },
				{ "jar", "x-java-archive" },
				//{ "ppd", "application/vnd.cups-postscript" },
				{ "rpm", "application/x-rpm" },
				{ "7z", "application/x-compressed" },
				{ "iso", "application/x-compressed" },
				{ "zip", "application/x-compressed" },
				{ "rar", "application/x-compressed" },
				{ "z", "application/x-compressed" },
				{ "tar", "application/x-tar" },
				{ "tgz", "application/x-tar-gz" },
				{ "gz", "application/x-gzip" },
				{ "pdf", "application/pdf" },
				{ "pdt", "application/pdf" },
				{ "exe", "application/octet-stream" },
				{ "dll", "application/octet-stream" },
				{ "bin", "application/octet-stream" },
				{ "png", "image/png" },
				{ "jpg", "image/jpeg" },
				{ "jpeg", "image/jpeg" },
				{ "bmp", "image/bmp" },
				{ "gif", "image/gif" },

			};

		/// <summary>   
		/// Gets a mime type based on a file extension.  Mappings are derived from the listing at <a href="http:
		/// //www.microsoft.com/technet/prodtechnol/isa/2004/plan/mimetypes.mspx" /> .  If a file extension is not registered, "application/octet-
		/// stream" is returned. 
		/// </summary>
		/// <remarks>   ebrown, 2/9/2011. </remarks>
		/// <param name="extension">	The case insensitive extension - the extension may include preceding periods.  For instance, ".htm" and
		///							 "htm" are both accepted values. </param>
		/// <returns>   The mime type for file extension if registered, otherwise the default of "application/octet-stream". </returns>
		public static string GetMimeTypeForFileExtension(string extension)
		{
			return GetMimeTypeForFileExtension(extension, "application/octet-stream");
		}

		/// <summary>   
		/// Gets a mime type based on a file extension.  Mappings are derived from the listing at <a href="http:
		/// //www.microsoft.com/technet/prodtechnol/isa/2004/plan/mimetypes.mspx" /> 
		/// </summary>
		/// <remarks>   ebrown, 2/9/2011. </remarks>
		/// <exception cref="ArgumentNullException">	Thrown when the passed extension or the default value are null. </exception>
		/// <exception cref="ArgumentException">		Thrown when the passed extension or the default value contain only whitespace. </exception>
		/// <param name="extension">	The case insensitive extension - the extension may include preceding periods.  For instance, ".htm" and
		///							 "htm" are both accepted values. </param>
		/// <param name="default">	 The default mime type to use if the given extension is not registered in the local mapping. </param>
		/// <returns>   The mime type for file extension if registered, otherwise the given default. </returns>
		public static string GetMimeTypeForFileExtension(string extension, string @default)
		{
			if (null == extension) { throw new ArgumentNullException("extension"); }
			if (string.IsNullOrWhiteSpace(extension)) { throw new ArgumentException("must not be whitespace", "extension"); }
			if (null == @default) { throw new ArgumentNullException("default"); }
			if (string.IsNullOrWhiteSpace(@default)) { throw new ArgumentException("must not be whitespace", "default"); }

			string trimmedExtension = extension.TrimStart('.');
			return mimeTypes.ContainsKey(trimmedExtension) ?
				mimeTypes[trimmedExtension] : @default;
		}
	}
}