using System;
using System.Globalization;
using System.Web.Security;
using Common.Logging;

namespace EPS.Web.Authentication.Utility
{
    /// <summary>   Provides a Membership provider locator. </summary>
    /// <remarks>   ebrown, 3/28/2011. </remarks>
    public static class MembershipProviderLocator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        /// <summary>   Gets the membership provider defined in configuration, or null if not specified. </summary>
        /// <remarks>   ebrown, 1/3/2011. </remarks>
        /// <exception cref="ArgumentOutOfRangeException">  Thrown when one or more arguments are outside the required range. </exception>
        /// <returns>   The membership provider that is used to validate incoming credentials. </returns>
        public static MembershipProvider GetProvider(string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
            {
                return null;
            }

            if (string.Equals(providerName, "default", StringComparison.OrdinalIgnoreCase))
            {
                MembershipProvider currentProvider = Membership.Provider;
                log.InfoFormat(CultureInfo.InvariantCulture, "Default provider of [{0}] selected", (null != currentProvider ? currentProvider.Name ?? "N/A" : "N/A"));
                return currentProvider;
            }

            MembershipProvider provider = Membership.Providers[providerName];
            if (provider == null)
            {
                throw new ArgumentOutOfRangeException(String.Format(CultureInfo.InvariantCulture, "Provider {0} specified in configuration not found", providerName));
            }

            log.InfoFormat(CultureInfo.InvariantCulture, "Custom provider of [{0}] specified in configuration selected", provider.Name ?? "*No Name*");
            return provider;
        }
    }
}
