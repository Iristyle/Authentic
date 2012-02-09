using System;
using System.Collections.Concurrent;
using EPS.Web.Authentication.Digest;
using EPS.Web.Authentication.Digest.Configuration;

namespace EPS.Web.Authentication.Configuration
{
	/// <summary>	Internal class responsible for creating IPasswordRetriever Password retriever locator. </summary>
	/// <remarks>	ebrown, 5/16/2011. </remarks>
	public static class PasswordRetrieverLocator
	{
		private static ConcurrentDictionary<string, IPasswordRetriever> retrievers = new ConcurrentDictionary<string, IPasswordRetriever>();
		
		/// <summary>   
		/// Gets the IPasswordRetriever instance implementing <see cref="T: EPS.Web.Authentication.Digest.IPasswordRetriever"></see>. 
		/// </summary>
		/// <remarks>   ebrown, 1/3/2011. </remarks>
		/// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
		/// <param name="configuration">    The configuration. </param>
		/// <returns>   The password retriever instance or null if the PasswordRetrieverName property is not properly configured. </returns>
		public static IPasswordRetriever Resolve(DigestAuthenticatorConfigurationElement configuration)
		{
			if (null == configuration)
				throw new ArgumentNullException("configuration");
			if (string.IsNullOrWhiteSpace(configuration.PasswordRetrieverName))
				return null;
			return retrievers.GetOrAdd(configuration.PasswordRetrieverName, retrieverName =>
			{
				return Activator.CreateInstance(Type.GetType(retrieverName)) as IPasswordRetriever;
			});
		}
	}
}