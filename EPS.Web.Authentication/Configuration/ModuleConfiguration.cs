using System;
using System.Collections.Generic;
using EPS.Web.Authentication.Abstractions;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   Code-based API for configuring the Http Context authentication module. </summary>
    /// <remarks>   ebrown, 4/8/2011. </remarks>
    public class ModuleConfiguration : 
        IModuleConfiguration
    {
        private bool _enabled = true;

        /// <summary>
        /// Initializes a new instance of the ModuleConfiguration class.
        /// </summary>
        public ModuleConfiguration(IEnumerable<IAuthenticator> inspectors, IFailureHandler failureHandler)
        {
            Inspectors = inspectors;
            FailureHandler = failureHandler;
        }
        
        /// <summary>   Gets or sets a value indicating whether the authentication module is enabled. </summary>
        /// <value> true if enabled, false if not.  Default for new instances is true. </value>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>   Gets the collection of defined inspectors. </summary>
        /// <value> The inspectors. </value>
        public IEnumerable<IAuthenticator> Inspectors { get; private set; }

        /// <summary>   Gets or sets the failure handler factory instance. </summary>
        /// <value> The failure handler factory. </value>
        public IFailureHandler FailureHandler { get; private set; }
    }
}