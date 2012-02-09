using System;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Configuration
{
    /// <summary>   Interface for the ConfigurationSection that stores valid configuration for a FileHttpHandler. </summary>
    /// <remarks>   ebrown, 2/14/2011. </remarks>
    public interface IFileHttpHandlerConfiguration
    {
        /// <summary>   
        /// Gets the URL to redirect to on unauthorize access errors.  Because this is a string, application relative Urls like ~/foo are
        /// supported. 
        /// </summary>
        /// <value> The unauthorized error redirect URL. </value>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~")]
        string UnauthorizedErrorRedirectUrl { get; }

        /// <summary>   
        /// Gets the URL to redirect to on unexpected or expected (missing file, corrupted file) server errors.  Because this is a string,
        /// application relative Urls like ~/foo are supported. 
        /// </summary>
        /// <value> The server error URL. </value>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~")]
        string ServerErrorUrl { get; }

        /// <summary>   Gets the size of the buffer in KB to use when reading from a file stream and transferring over the wire. </summary>
        /// <value> The file transfer buffer size in KB. </value>
        int FileTransferBufferSizeInKBytes { get; }

        /// <summary>   Gets the maximum file size in KB to hash on outgoing transfers. </summary>
        /// <value> The maximum file size in KB. </value>
        int MaximumFileSizeInKBytesToHash { get; }

        /// <summary>   Gets the maximum file size in KB to allow for outgoing transfers. </summary>
        /// <value> The maximum file size in KB. </value>
        long MaximumFileSizeInKBytesToSend { get; }
    }
}