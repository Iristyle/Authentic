using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace EPS.Web
{
    /// <summary>   A set of helpers for processing cookies, including some extensions on <see cref="T:System.Web.HttpCookie"/>. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class CookieHelper
    {
        private static Regex cookieSplitter = new Regex(@"(?=\S),(?=\S)", RegexOptions.Compiled);

        /// <summary>   A HttpCookie extension method that converts a cookie to a HTTP header style cookie string. </summary>
        /// <remarks>   ebrown, 11/9/2010. </remarks>
        /// <param name="cookie">   The cookie to convert. </param>
        /// <param name="encode">   true to HTML encode the string; default false. </param>
        /// <returns>   The cookie as a string. </returns>
        public static string ToCookieString(this HttpCookie cookie, bool encode)
        {
            return String.Format("{0}={1}; ", (encode ? HttpUtility.HtmlEncode(cookie.Name) : cookie.Name), (encode ? HttpUtility.HtmlEncode(cookie.Value) : cookie.Value));
        }

        /// <summary>   A HttpCookie extension method that converts a cookie to a string that can be used in a HTTP 'Set-Cookie' header. </summary>
        /// <remarks>   ebrown, 11/9/2010. </remarks>
        /// <param name="cookie">       The cookie to convert. </param>
        /// <param name="includeSet">   Whether to emit the 'Set-Cookie: ' as part of the return string. true to emit, false otherwise. </param>
        /// <returns>   This cookie string. </returns>
        public static string ToSetCookieString(this HttpCookie cookie, bool includeSet)
        {
            return (includeSet ? "Set-Cookie: " : string.Empty) +
                EPS.Net.CookieHelper.GetSetCookieHeader(cookie.Name, cookie.Value, cookie.Expires, cookie.Domain, cookie.Path);
        }

        /// <summary>   A HttpCookie extension method that converts a cookie to a string that can be used in a HTTP 'Set-Cookie' header. </summary>
        /// <remarks>   ebrown, 11/9/2010. </remarks>
        /// <param name="cookie">   The cookie to convert. </param>
        /// <returns>   This cookie string. </returns>
        public static string GetSetCookieHeader(this HttpCookie cookie)
        {
            return EPS.Net.CookieHelper.GetSetCookieHeader(cookie.Name, cookie.Value, cookie.Expires, cookie.Domain, cookie.Path);
        }

        /// <summary>   
        /// This simply takes a HttpCookie and copies it to a HttpCookie, but only retains the name and value of the cookie -- not the other
        /// properties. 
        /// Path is reset to / by default, unless passed in. 
        /// Domain is set to the one passed in.
        /// </summary>
        /// <remarks>   ebrown, 11/9/2010. </remarks>
        /// <param name="cookie">   A cookie to copy. </param>
        /// <param name="domain">   New domain. </param>
        /// <param name="path">     Path on the site to apply the cookie to - by default /. </param>
        /// <returns>   A new HttpCookie. </returns>
        public static HttpCookie ToCookieWithNewDomain(this HttpCookie cookie, string domain, string path = "/")
        {
            return new HttpCookie(cookie.Name, cookie.Value) { Path = path, Domain = domain };
        }

        /// <summary>   
        /// This simply takes a HttpCookie (i.e. from a request) and converts it to a Cookie, but only retains the name and value of the cookie --
        /// not the other properties. 
        /// Path is reset to / by default, unless passed in
        /// Domain is set to the one passed in. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="cookie">   A cookie to copy. </param>
        /// <param name="domain">   New domain. </param>
        /// <param name="path">     Path on the site to apply the cookie to - by default /. </param>
        /// <returns>   A new Cookie. </returns>
        public static Cookie ConvertToCookieWithNewDomain(this HttpCookie cookie, string domain, string path = "/")
        {
            return new Cookie(cookie.Name, cookie.Value, "/", domain);
        }


        /// <summary>   A HttpCookie extension method that convert to a Cookie. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="cookie">   A HttpCookie to copy. </param>
        /// <returns>   A new Cookie. </returns>
        public static Cookie ConvertToCookie(this HttpCookie cookie)
        {            
            return new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain)
            {
                Expires = cookie.Expires,
                HttpOnly = cookie.HttpOnly,
                Secure = cookie.Secure
            };
        }

        /// <summary>   A Cookie extension method that converts to a HttpCookie. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="cookie">   A Cookie to copy. </param>
        /// <returns>   A new HttpCookie. </returns>
        public static HttpCookie ConvertToHttpCookie(this Cookie cookie)
        {
            return new HttpCookie(cookie.Name, cookie.Value)
            {
                Domain = cookie.Domain,
                Expires = cookie.Expires,
                HttpOnly = cookie.HttpOnly,
                Path = cookie.Path,
                Secure = cookie.Secure,
            };
        }

        /// <summary>   A HttpWebResponse extension method that gets a http cookies. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="response"> The response to act on. </param>
        /// <returns>   The http cookies. </returns>
        public static IEnumerable<HttpCookie> GetHttpCookies(this HttpWebResponse response)
        {
            return response.Cookies.OfType<Cookie>().Select(c => c.ConvertToHttpCookie());
        }
        //5-3-2010 -- old code -- instead just converters above since the runtime has done this work already
        /*
        if (null == response || null == response.Headers)
            return new List<HttpCookie>();

        string setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];
        if (string.IsNullOrEmpty(setCookieHeader))
            return new List<HttpCookie>();

        log.InfoFormat("Cookie retrieved: Set-Cookie: {0}", setCookieHeader);

        return cookieSplitter.Split(setCookieHeader).Where(s => !string.IsNullOrEmpty(s)).Select(cookieSplit =>
        {
            HttpCookie cookie = new HttpCookie(string.Empty, string.Empty);
            foreach (var s in cookieSplit.Trim().Split(';').Select(s => s.Trim()))
            {
                if (0 == s.IndexOf("expires="))
                    cookie.Expires = DateTime.Parse(s.Replace("expires=", string.Empty));
                else if (0 == s.IndexOf("path="))
                    cookie.Path = s.Replace("path=", string.Empty); //"/"; 
                else if (0 == s.IndexOf("domain="))
                    cookie.Domain = s.Replace("domain=", string.Empty); //".mykonicaminolta.com";
                else if (s.IndexOf('=') >= 0)
                {
                    int index = s.IndexOf('=');
                    cookie.Name = s.Substring(0, index);
                    if ((index + 1) < s.Length)
                        cookie.Value = s.Substring(index + 1);
                }
            }
            return cookie;

        }).ToList();
        */
    }
}