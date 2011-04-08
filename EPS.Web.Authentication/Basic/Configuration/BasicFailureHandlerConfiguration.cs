using System;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Basic.Configuration
{
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
