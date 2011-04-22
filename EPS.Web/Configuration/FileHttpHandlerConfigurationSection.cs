using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace EPS.Web.Configuration
{
	/// <summary>   A ConfigurationSection representing valid configuration options for a FileHttpHandler. </summary>
	/// <remarks>   ebrown, 2/14/2011. </remarks>
	public class FileHttpHandlerConfigurationSection : ConfigurationSection, IFileHttpHandlerConfiguration
	{
		/// <summary> Default path in the configuration file </summary>
		public static readonly string ConfigurationPath = "eps.web/fileHttpHandler";

		/// <summary>   
		/// Gets the URL to redirect to on unauthorize access errors.  Because this is a string, application relative Urls like ~/foo are
		/// supported. 
		/// </summary>
		/// <value> The unauthorized error redirect URL. </value>
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~")]
		[ConfigurationProperty("unauthorizedErrorRedirectUrl", IsRequired = true)]
		public string UnauthorizedErrorRedirectUrl
		{
			get { return (string)this["unauthorizedErrorRedirectUrl"]; }
		}

		/// <summary>   
		/// Gets the URL to redirect to on unexpected or expected (missing file, corrupted file) server errors.  Because this is a string,
		/// application relative Urls like ~/foo are supported. 
		/// </summary>
		/// <value> The server error URL. </value>
		[SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "These strings may be partial / relative urls or may be prefixed with ~")]
		[ConfigurationProperty("serverErrorUrl", IsRequired = true)]
		public string ServerErrorUrl
		{
			get { return (string)this["serverErrorUrl"]; }
		}

		/// <summary>   Gets the size of the buffer in KB to use when reading from a file stream and transferring over the wire. </summary>
		/// <value> The file transfer buffer size in KB.  If unspecified in the configuration file, the default is 256 KB. </value>
		[ConfigurationProperty("fileTransferBufferSizeInKBytes", IsRequired = false, DefaultValue = 256)]
		public int FileTransferBufferSizeInKBytes
		{
			get { return (int)this["fileTransferBufferSizeInKBytes"]; }
		}

		/// <summary>   Gets the maximum file size in KB to hash on outgoing transfers. </summary>
		/// <value> The maximum file size in KB.  If unspecified in the configuration file, the default is 30 MB (30720 KB) </value>
		[ConfigurationProperty("maximumFileSizeInKBytesToHash", IsRequired = false, DefaultValue = 30720)]
		public int MaximumFileSizeInKBytesToHash
		{
			get { return (int)this["maximumFileSizeInKBytesToHash"]; }
		}

		/// <summary>   Gets the maximum file size in KB to allow for outgoing transfers. </summary>
		/// <value> The maximum file size in KB. If unspecified in the configuration file, 4 GB (4194304 KB). </value>
		[ConfigurationProperty("maximumFileSizeInKBytesToSend", IsRequired = false, DefaultValue = 4194304)]
		public long MaximumFileSizeInKBytesToSend
		{
			get { return (long)this["maximumFileSizeInKBytesToSend"]; }
		}
	}
}