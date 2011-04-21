using System;

namespace EPS.Web.Authentication.Configuration
{
	/// <summary>	The configuration class responsible for managing the settings for the failure on redirect handler. </summary>
	/// <remarks>	ebrown, 4/21/2011. </remarks>
	public class RedirectFailureHandlerConfiguration :
		FailureHandlerConfiguration, IRedirectFailureHandlerConfiguration
	{
		/// <summary>
		/// Initializes a new instance of the RedirectFailureHandlerConfiguration class.
		/// </summary>
		public RedirectFailureHandlerConfiguration(Uri redirectUri)
		{
			//TODO: 4-8-2011 -- cook up FluentValidator class
			RedirectUri = redirectUri;
		}

		/// <summary>   Gets or sets URI for the redirect on a failed request. </summary>
		/// <value> The redirect uri. </value>
		public Uri RedirectUri { get; set; }
	}
}
