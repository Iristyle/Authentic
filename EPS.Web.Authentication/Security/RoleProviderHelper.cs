using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Security;
using EPS.Configuration.Abstractions;
using log4net;

namespace EPS.Web.Authentication.Security
{
    /// <summary>   Role provider helper. </summary>
    /// <remarks>   ebrown, 1/3/2011. </remarks>
    public static class RoleProviderHelper
    {
        private static Lazy<IConfigurationManager> _configurationManager = new Lazy<IConfigurationManager>(() => new ConfigurationManagerWrapper());

        /// <summary>   Gets or sets the IConfigurationManager instance - intended to be wired up at application startup before being used. </summary>
        /// <remarks>   Can only be set once. </remarks>
        /// <value> The configuration manager. </value>
        /// <exception cref="InvalidOperationException">    A non-default IConfigurationManager may only be supplied before any methods are
        ///                                                     called that use configuration information and it may only be set once. </exception>
        public static IConfigurationManager ConfigurationManager
        {
            get
            {
                return _configurationManager.Value;
            }
            set
            {
                if (!_configurationManager.IsValueCreated)
                {
                    _configurationManager = new Lazy<IConfigurationManager>(() => value);
                    //ensure that IsValueCreated set to true
                    var throwAway = _configurationManager.Value; 
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Lazy<Dictionary<string, RoleProvider>> roleProviders = new Lazy<Dictionary<string, RoleProvider>>(() =>
        {
            try
            {
                RoleManagerSection roleManagerConfig = ConfigurationManager.GetSection<RoleManagerSection>("system.web/roleManager");
                //could also use this
                //System.Web.Configuration.ProvidersHelper.InstantiateProvider() is an alternative
                return roleManagerConfig.Providers.OfType<ProviderSettings>().ToDictionary(p => p.Name, settings =>
                {
                    Type c = MultiGetType(settings.Type);
                    if (!typeof(RoleProvider).IsAssignableFrom(c))
                        throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture, "{0} must implement type {1}", settings.Type, typeof(RoleProvider)));
                    var provider = (RoleProvider)Activator.CreateInstance(c);
                    provider.Initialize(settings.Name, new NameValueCollection(settings.Parameters));
                    return provider;
                });
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error", ex);
                throw;
            }
        });
        
        private static Type MultiGetType(string name)
        {
            //types not fully qualified will try to resolve against our util library
            try
            {
                return Type.GetType(name, true, true);
            }            
            catch (TypeLoadException)
            {
                //assume that the provider is located in System.Web instead of our util library
                return Assembly.Load("System.Web").GetType(name, true, true);
            }
        }

        /// <summary>   Gets a role provider by name.  Will load default RoleManagerSection from config unless overriden. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="name"> The name of the provider. </param>
        /// <returns>   The specified RoleProvider instance. </returns>
        public static RoleProvider GetProviderByName(string name)
        {            
            KeyValuePair<string, RoleProvider> pair = roleProviders.Value.FirstOrDefault(rp => (0 == string.Compare(name, rp.Key, StringComparison.CurrentCulture)));
            if (null == pair.Value)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "No RoleProvider by the name {0} exists in the <providers> configuration section of the <roleManager>", name), "name");
            }

            return pair.Value;
        }
    }
}