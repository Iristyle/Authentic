using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
	/// <summary>	The configuration class responsible for managing the settings for the basic challenge on failure handler. </summary>
	/// <remarks>	ebrown, 4/21/2011. </remarks>
	public class BasicFailureHandlerConfiguration :
            FailureHandlerConfiguration, IBasicFailureHandlerConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the BasicFailureHandlerConfiguration class.
        /// </summary>
        public BasicFailureHandlerConfiguration(string realm)
        {
            Realm = realm;
            //TODO: 4-8-2011 -- create FluentValidator class to use here
        }

        /// <summary>   Gets or sets the realm of the cookie on an outgoing cookie request. </summary>
        /// <value> The realm. </value>
        public string Realm { get; set; }
    }
}
