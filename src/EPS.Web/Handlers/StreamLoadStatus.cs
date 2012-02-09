using System;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Handlers
{
	/// <summary>   
	/// These specific values represent the status of a given FileStreamDetails as requested through the IFileHttpHandlerStreamLoader. 
	/// </summary>
	/// <remarks>   ebrown, 2/14/2011. </remarks>
	[SuppressMessage("Gendarme.Rules.Naming", "UseSingularNameInEnumsUnlessAreFlagsRule", Justification = "False Positive Since Status ends in s")]
	public enum StreamLoadStatus
	{
		/// <summary> The FileStreamDetails were successfully loaded.  </summary>
		Success,
		/// <summary> The given request was invalid.  </summary>
		InvalidRequest,
		/// <summary> The request was denied based on the requesting user / security.  </summary>
		UnauthorizedAccess,
		/// <summary> No file metadata was found for the given contextual request information.  </summary>
		FileMetadataNotFound,
		/// <summary> File metadata was found, but a physical file could not be located.  </summary>
		FileNotFound,
		/// <summary> File metadata was found, and the file resides at an alternate Uri.  </summary>
		FileStoredInCloud,
		/// <summary> File metadata was found, but the actual FileStream found did not match the MD5 metadata stored.  </summary>
		MD5Failed,
		/// <summary> File metadata was found, but the actual FileStream size did not match the file size metadata stored.  </summary>
		FileSizeMismatched,
		/// <summary> An unexpected error occurred.  </summary>
		UnexpectedError,
	}
}
