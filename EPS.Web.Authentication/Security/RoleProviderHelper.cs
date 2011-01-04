using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
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
        /// <summary>   Gets or sets the IConfigurationManager instance - intended to be used by IoC containers. </summary>
        /// <value> The configuration manager. </value>
        public static IConfigurationManager ConfigurationManager { get; private set; }

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

        /// <summary>   Gets a role provider by name. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <param name="name">                 The name. </param>
        /// <param name="configurationManager"> ConfigurationManager instance - can be injected via IoC. </param>
        /// <returns>   The provider by name. </returns>
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This code is only used server-side internally where we control source languages - default params are perfectly acceptable")]
        public static RoleProvider GetProviderByName(string name, IConfigurationManager configurationManager = null)
        {
            ConfigurationManager = configurationManager;
            KeyValuePair<string, RoleProvider> pair = roleProviders.Value.FirstOrDefault(rp => (0 == string.Compare(name, rp.Key, StringComparison.CurrentCulture)));
            if (null == pair.Value)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "No RoleProvider by the name {0} exists in the <providers> configuration section of the <roleManager>", name), "name");
            }

            return pair.Value;
        }
    }
}
