using System;
using System.Diagnostics.CodeAnalysis;
using EPS.Text;

namespace EPS.Web.Handlers
{
    /// <summary>   Values that represent the status of a file as it is being sent. </summary>
    /// <remarks>   ebrown, 2/9/2011. </remarks>
    [SuppressMessage("Gendarme.Rules.Naming", "UseSingularNameInEnumsUnlessAreFlagsRule", Justification = "False positive b/c Gendarme as of 2.10 is not smart about plurals")]
    public enum StreamWriteStatus
    {
        /// <summary> The application was restarted while the file send was taking place. </summary>
        [EnumDescription("AR", "Application Restarted / Send Never Completed")]
        ApplicationRestarted,
        
        /// <summary> HTTP headers have been sent in response to a client request.  </summary>
        [EnumDescription("SH", "Sent HTTP Headers")]
        SentHttpHead,

        /// <summary> The file was succesfully sent to the client.  </summary>
        [EnumDescription("SF", "Sent File")]
        SentFile,

        /// <summary> A response has been sent to the client issused multipart range request.  </summary>
        [EnumDescription("SM", "Sent Multipart Range Request")]
        SentMultipartRangeRequest,

        /// <summary> A response has been sent to the client issused range request.  </summary>
        [EnumDescription("SR", "Sent Range Request")]
        SentRangeRequest,

        /// <summary> An unexpected error has occurred.  </summary>
        [EnumDescription("UE", "Unexpected Error")]
        UnexpectedError,
        
        /// <summary> The client disconnected during the send.  </summary>
        [EnumDescription("CE", "Client Disconnected Error")]
        ClientDisconnected,
        
        /// <summary> The hash of the file stream does not match the expected hash.  </summary>
        [EnumDescription("HE", "MD5 Hash Error")]
        MD5Failed,
        
        /// <summary> The request was made for a file that does not exist.  </summary>
        [EnumDescription("NF", "The specified file was not found on disk")]
        NotFound,

        /// <summary> An error occurred while reading the file, which could include local file system access issues.  </summary>
        [EnumDescription("RE", "File Read Error")]
        StreamReadError,
                
        //[EnumDescription("IE", "Impersonation Error")]
        //ImpersonationError,
        
        /// <summary> The size of the file stream does not match the expected file size.  </summary>
        [EnumDescription("SE", "File Size Mismatch Error")]
        MismatchedSizeError,
        
        /// <summary> The file is too large to send based on a server or configuration limitation.  </summary>
        [EnumDescription("LE", "File Too Large To Send Error")]
        FileTooLargeError,
        
        /// <summary> The user does not have access to the specified file.  </summary>
        [EnumDescription("AE", "Unauthorized Access To File Error")]
        UnauthorizedAccessError,
        
        //[EnumDescription("PE", "Product Downloads Disabled Error")]
        //ProductDownloadsDisabledError,
        
        /// <summary> Sends have been disabled on this particular file.  </summary>
        [EnumDescription("FE", "Individual File Downloads Disabled Error")]
        FileDownloadsDisabledError,
        
        //[EnumDescription("FR", "File Too Large For HTTP / Redirected to FTP")]
        //FileRedirectedToFTP,
        
        /// <summary> The HTTP request was bad in some way.  </summary>
        [EnumDescription("UR", "User Invalid Request Error")]
        UserRequestError,
        
        /// <summary> The availability of a given file was changed during the send.  </summary>
        [EnumDescription("RD", "File Deactivated During Send")]
        FileRecordDeactivatedDuringSend,
        
        /// <summary> The HTTP range request made by the client was bad.  </summary>
        [EnumDescription("BR", "Bad Range Request For Resume")]
        BadRangeRequest,

        /// <summary> The request was made for a file hosted at another Uri.  </summary>
        [EnumDescription("CR", "Redirected Client To Cloud Uri")]
        RedirectedClientToCloudUri
    }
}