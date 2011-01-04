using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EPS.Web.Authentication.Abstractions;
using EPS.Web.Authentication.Configuration;

namespace EPS.Web.Authentication
{
	/// <summary>   This class builds (and caches) http context inspecting authenticator factory instances given configuration. </summary>
	/// <remarks>   ebrown, 1/3/2011. </remarks>
	public static class HttpContextInspectorsLocator
	{
		private static ConcurrentDictionary<HttpContextInspectingAuthenticatorConfigurationElement, IHttpContextInspectingAuthenticatorFactory> factoryInstances
			= new ConcurrentDictionary<HttpContextInspectingAuthenticatorConfigurationElement, IHttpContextInspectingAuthenticatorFactory>();

		private static ConcurrentDictionary<HttpContextInspectingAuthenticationModuleSection, IHttpContextInspectingAuthenticationFailureHandlerFactory> failureFactoryInstances
			= new ConcurrentDictionary<HttpContextInspectingAuthenticationModuleSection, IHttpContextInspectingAuthenticationFailureHandlerFactory>();

        /// <summary>   Gets an actual inspector instance based on configuration values. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="configuration">    The configuration specifying a factory. </param>
        /// <returns>   An inspector instance as specified in config. </returns>
		public static IHttpContextInspectingAuthenticator Construct(HttpContextInspectingAuthenticatorConfigurationElement configuration)
		{
			if (null == configuration) { throw new ArgumentNullException("configuration"); }

			//TODO: 5-21-2010 -- theres a bug here -- dynamic runtime can't find the appropriate Construct method for some reason
			//it *should* work, but doesn't
			//HACK: rather than deal with some funky reflection code, we just use dynamic, since we know there is a Construct here
			//dynamic factoryInstance = Activator.CreateInstance(Type.GetType(Factory));

			//http://stackoverflow.com/questions/266115/pass-an-instantiated-system-type-as-a-type-parameter-for-a-generic-class
			//var t = typeof(IHttpContextInspectingAuthenticatorFactory<>).MakeGenericType(this.GetType());

			return factoryInstances.GetOrAdd(configuration, 
                (config) =>
			    {
				    return (IHttpContextInspectingAuthenticatorFactory)Activator.CreateInstance(Type.GetType(config.Factory));
			    }).Construct(configuration);
		}

        /// <summary>   
        /// Enumerates all of the configured inspectors in the given collection, returning an Enumeration of the instances. Inspector instances
        /// are created given the type names specified in configuration. 
        /// </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="inspectors">   The inspectors. </param>
        /// <returns>   An enumerator that allows foreach to be used on the configured inspectors. </returns>
		public static IEnumerable<IHttpContextInspectingAuthenticator> Construct(HttpContextInspectingAuthenticatorConfigurationElementCollection inspectors)
		{
			if (null == inspectors) { throw new ArgumentNullException("inspectors"); }

			return inspectors.OfType<HttpContextInspectingAuthenticatorConfigurationElement>().Select(i => Construct(i));
		}

        /// <summary>   Gets the failure handler. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="configuration">    The configuration. </param>
        /// <returns>   The failure handler. </returns>
		public static IHttpContextInspectingAuthenticationFailureHandler GetFailureHandler(HttpContextInspectingAuthenticationModuleSection configuration)
		{
			if (null == configuration) { throw new ArgumentNullException("configuration"); }

			if (string.IsNullOrEmpty(configuration.FailureHandlerFactoryName))
				return null;

			return failureFactoryInstances.GetOrAdd(configuration, (config) =>            
			{
				return (IHttpContextInspectingAuthenticationFailureHandlerFactory)Activator.CreateInstance(Type.GetType(config.FailureHandlerFactoryName));
			}).Construct(configuration.GetCustomFailureHandlerConfigurationSection());
		}
	}
}