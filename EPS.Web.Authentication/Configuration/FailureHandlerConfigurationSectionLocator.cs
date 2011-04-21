using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Globalization;

namespace EPS.Web.Authentication.Configuration
{
	/// <summary>   Provides a registration / location mechanism for custom failure handler configuration sections. </summary>
	/// <remarks>   ebrown, 4/7/2011. </remarks>
	public static class FailureHandlerConfigurationSectionLocator
	{
		private readonly static ConcurrentDictionary<string, IFailureHandlerConfiguration> customConfigurationSections
			= new ConcurrentDictionary<string, IFailureHandlerConfiguration>();

		/// <summary>   
		/// Registers the custom failure handler configuration section, so that it may be used instead of resolving against a config file. 
		/// </summary>
		/// <remarks>   ebrown, 4/7/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <exception cref="ArgumentException">        Thrown when one or more arguments have unsupported or illegal values. </exception>
		/// <param name="name">             The name. </param>
		/// <param name="configuration">    The configuration. </param>
		public static void Register(string name, IFailureHandlerConfiguration configuration)
		{
			if (null == name) { throw new ArgumentNullException("name"); }
			if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("cannot be empty", "name"); }
			if (null == configuration) { throw new ArgumentNullException("configuration"); }

			var storedConfiguration = customConfigurationSections.GetOrAdd(name, configuration);
			if (storedConfiguration != configuration) { throw new ArgumentException("A configuration section by the name specified is already registered"); }
		}

		/// <summary>	Gets the custom failure handler configuration section based on the name specified in the given configuration. </summary>
		/// <remarks>	
		/// Will attempt to resolve against the cached list first (included registered configuration), and then will roll-over automatically to
		/// loading from the current .config file. 
		/// </remarks>
		/// <exception cref="ArgumentException">	Thrown when one or more arguments have unsupported or illegal values. </exception>
		/// <param name="configSectionName">	The configuration. </param>
		/// <returns>	The custom failure handler configuration section. </returns>
		public static IFailureHandlerConfiguration Resolve(string configSectionName)
		{
			if (string.IsNullOrWhiteSpace(configSectionName))
			{
				return null;
			}

			return customConfigurationSections.GetOrAdd(configSectionName, (name) =>
				{
					//TOOD: 5-5-2010 -- unfortunately this defers this error until runtime rather than config parse time
					//see if we can fix that
					var section = ConfigurationManager.GetSection(name) as IFailureHandlerConfiguration;
					if (null == section)
					{
						throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
							"The custom configuration section specified by \"customFailureHandlerConfigurationSection\" [{0}] must exist - check configuration settings",
							name));
					}
					return section;
				});
		}
	}
}