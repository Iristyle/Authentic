using System;
using System.Collections.Concurrent;
using EPS.Web.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication.Configuration
{
    /// <summary>   
    /// This class manages construction of IPrincipalBuilder instances given a <see cref="T:AuthenticatorConfigurationElement"/> instance.
    /// This is effectively a service locator. 
    /// </summary>
    /// <remarks>   ebrown, 4/7/2011. </remarks>
    public static class PrincipalBuilderLocator
    {
        private static ConcurrentDictionary<string, IPrincipalBuilderFactory> factories =
            new ConcurrentDictionary<string, IPrincipalBuilderFactory>();

        /// <summary>   
        /// Gets the principal builder factory instance implementing <see cref="T: EPS.Web.Abstractions.IPrincipalBuilderFactory"/>, and uses
        /// that to create instances of <see cref="T: EPS.Web.Abstractions.IPrincipalBuilder"/> given the specified configuration. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>   The principal builder instance or null if the PrincipalBuilderFactory property is not properly configured. </returns>
        public static IPrincipalBuilder Resolve(AuthenticatorConfigurationElement configuration)
        {
            if (null == configuration) { throw new ArgumentNullException("configuration"); }

            if (string.IsNullOrWhiteSpace(configuration.PrincipalBuilderFactory))
            {
                return null;
            }

            var factory = factories.GetOrAdd(configuration.PrincipalBuilderFactory, factoryName =>
                {
                    return Activator.CreateInstance(Type.GetType(factoryName)) as IPrincipalBuilderFactory;
                });

            return factory.Construct(configuration);
        }
    }
}