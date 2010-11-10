using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using EPS.Configuration.Abstractions;
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
        /// <param name="request">              The incoming request. </param>
        /// <param name="configurationManager"> An optional IConfigurationManager to read settings from.  If left unspecified or null, the
        ///                                     executing application ConfigurationManager is used. </param>
        /// <returns>   <c>true</c> if the device is mobile or has been configured as mobile with a cookie; otherwise, <c>false</c>. </returns>
        public static bool IsMobileDeviceOrConfiguredMobile(this HttpRequestBase request, IConfigurationManager configurationManager = null)
        {
            if (null == configurationManager)
                configurationManager = new ConfigurationManagerWrapper();

            MobileConfigurationSection config = configurationManager.GetSection<MobileConfigurationSection>(MobileConfigurationSection.ConfigurationPath);

            var mobileCookie = request.Cookies.Get(config.OverrideCookie);
            if (null == mobileCookie)
                return request.Browser.IsMobileDevice;

            return mobileCookie.Value.ToBoolean(false);
        }

        /// <summary>   A HttpRequestBase extension method that takes extracts a list of HttpCookies from the request and converts them to a list of Cookie. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="request">  The request to act on. </param>
        /// <returns>   The cookies. </returns>
        public static IEnumerable<Cookie> GetCookies(this HttpRequestBase request)
        {
            return request.Cookies.OfType<HttpCookie>().Select(c => c.ConvertToCookie());
        }
    }
}
