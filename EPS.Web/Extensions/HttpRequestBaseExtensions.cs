using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using EPS.Conversions;
using EPS.Web.Configuration;

namespace EPS.Web
{
    /// <summary>
    /// A simple set of extensions on top of the <see cref="T:System.Web.Abstractions.HttpRequestBase"/> class
    /// </summary>
    public static class HttpRequestBaseExtensions
    {
        /// <summary>   
        /// Determines whether the current browser is a mobile device (by built-in User-Agent sniffing) OR whether it has been configured as
        /// mobile indicated by the presence of a cookie. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="request">          The incoming request. </param>
        /// <param name="configuration">    An IMobileConfigurationSection to read settings from.  For testing purposes, instances may be faked,
        ///                                 otherwise use the appropriate xml config and register MobileConfigurationSection from new
        ///                                 ConfigurationManagerWrapper().GetSection&lt;MobileConfigurationSection&gt; in an IoC container. </param>
        /// <returns>   <c>true</c> if the device is mobile or has been configured as mobile with a cookie; otherwise, <c>false</c>. </returns>
        public static bool IsMobileDeviceOrConfiguredMobile(this HttpRequestBase request, IMobileConfigurationSection configuration)
        {
            if (null == request) { throw new ArgumentNullException("request"); }
            if (null == configuration) { throw new ArgumentNullException("configuration"); }

            var mobileCookie = request.Cookies.Get(configuration.OverrideCookie);
            return null == mobileCookie ? request.Browser.IsMobileDevice
                : mobileCookie.Value.ToBoolean(false);
        }

        /// <summary>   
        /// A HttpRequestBase extension method that takes extracts a list of HttpCookies from the request and converts them to a list of Cookie. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="request">  The request to act on. </param>
        /// <returns>   The cookies. </returns>
        public static IEnumerable<Cookie> GetCookies(this HttpRequestBase request)
        {
            if (null == request) { throw new ArgumentNullException("request"); }

            return request.Cookies.OfType<HttpCookie>().Select(c => c.ConvertToCookie());
        }
    }
}
