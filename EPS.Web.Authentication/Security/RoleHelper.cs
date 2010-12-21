using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web.Configuration;
using System.Web.Security;
using EPS.Configuration.Abstractions;
using log4net;

namespace EPS.Web.Authentication.Security
{
    public static class RoleProviderHelper
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool initialized = false;
        private static Dictionary<string, RoleProvider> roleProviders = new Dictionary<string, RoleProvider>();

        private static void Initialize(IConfigurationManager configurationManager = null)
        {
            try
            {
                Monitor.Enter(roleProviders);
                if (initialized)
                    return;

                roleProviders.Clear();
                
                RoleManagerSection roleManagerConfig = configurationManager.GetSection<RoleManagerSection>("system.web/roleManager");

                //could also use this
                //System.Web.Configuration.ProvidersHelper.InstantiateProvider() is an alternative
                foreach (ProviderSettings settings in roleManagerConfig.Providers)
                {
                    Type c = MultiGetType(settings.Type);
                    if (!typeof(RoleProvider).IsAssignableFrom(c))
                        throw new ConfigurationErrorsException(String.Format("{0} must implement type {1}", settings.Type, typeof(RoleProvider)));

                    RoleProvider provider = (RoleProvider) Activator.CreateInstance(c);
                    /*
                    NameValueCollection parameters = settings.Parameters;
                    NameValueCollection config = new NameValueCollection(parameters.Count, StringComparer.Ordinal);
                    foreach (string str in parameters)
                        config[str] = parameters[str];
                    provider.Initialize(settings.Name, config);
                     */
                    provider.Initialize(settings.Name, new NameValueCollection(settings.Parameters));
                    roleProviders.Add(settings.Name, provider);
                }

                initialized = true;
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error", ex);
                throw;
            }
            finally
            {
                Monitor.Exit(roleProviders);
            }
        }

        private static Type MultiGetType(string name)
        {
            //types not fully qualified will try to resolve against our util library
            try
            {
                return Type.GetType(name, true, true);
            }
            //assume that the provider is located in System.Web instead of our util library
            catch (TypeLoadException)
            {
                return Assembly.Load("System.Web").GetType(name, true, true);
            }
        }

        public static RoleProvider GetProviderByName(string name)
        {
            if (!initialized)
                Initialize();

            KeyValuePair<string, RoleProvider> pair = roleProviders.FirstOrDefault(rp => (0 == string.Compare(name, rp.Key, true)));
            if (null == pair.Value)
                throw new ConfigurationErrorsException(String.Format("No RoleProvider by the name {0} exists in the <providers> configuration section of the <roleManager>", name));

            return pair.Value;
        }
    }
}
