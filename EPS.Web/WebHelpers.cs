using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using EPS.Text;

namespace EPS.Web
{
    /// <summary>   Some helper classes for working with web goop. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class WebHelpers
    {
        /// <summary>   Encodes a NameValueCollection full of parameters as a POST friendly string. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="parameters">   The parameters. </param>
        /// <returns>   a string that can be used to POST with. </returns>
        public static string EncodePOSTString(NameValueCollection parameters)
        {
            return string.Join("&", parameters.AllKeys.Where(k => null != k)
                .Select(key => string.Format("{0}={1}", HttpUtility.UrlEncodeUnicode(key), HttpUtility.UrlEncodeUnicode(parameters[key]))));            
        }
        
        /// <summary>   Encodes a parameter for use in a URL with an optional separator. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="encodeType">   Type of the encode. </param>
        /// <param name="name">         Name of the parameter. </param>
        /// <param name="value">        The parameter value. </param>
        /// <param name="prepend">      A separator to prepend - default of no separator. </param>
        /// <param name="allowBlank">   true to allow blank values, false to deny blank values. </param>
        /// <returns>   The parameter encoded into a URL. </returns>
        public static string EncodeUrlParameter(EncodeType encodeType, string name, string value, UrlParameterSeparator prepend = UrlParameterSeparator.None, bool allowBlank = true)
        {
            if (null == name)
                return string.Empty;

            value = value.Trim();
            if (allowBlank || !string.IsNullOrEmpty(value))
            {                
                if (EncodeType.Url == encodeType)
                    return String.Format("{0}{1}={2}", prepend.ToEnumValueString(), HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(value));

                return String.Format("{0}{1}={2}", prepend.ToEnumValueString(), HttpUtility.HtmlEncode(name), HttpUtility.HtmlEncode(value));
            }

            return string.Empty;
        }

        /// <summary>   
        /// Returns a site relative HTTP path from a partial path starting out with a ~. Same syntax that ASP.Net internally supports but this
        /// method can be used outside of the Page framework.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax but returns an absolute URL. 
        /// <a href="http://www.west-wind.com/Weblog/posts/154812.aspx" />
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="originalUrl">  Any Url including those starting with ~. </param>
        /// <returns>   Site relative url. </returns>
        public static string ResolveUrl(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl) ||
                IsAbsolutePath(originalUrl) ||
                // *** We don't start with the '~' -> we don't process the Url
                !originalUrl.StartsWith("~"))
                return originalUrl;

            // *** Fix up path for ~ root app dir directory
            // VirtualPathUtility blows up if there is a 
            // query string, so we have to account for this.
            int queryStringStartIndex = originalUrl.IndexOf('?');
            if (queryStringStartIndex != -1)
            {
                string queryString = originalUrl.Substring(queryStringStartIndex);
                string baseUrl = originalUrl.Substring(0, queryStringStartIndex);

                return string.Concat(VirtualPathUtility.ToAbsolute(baseUrl), queryString);
            }

            return VirtualPathUtility.ToAbsolute(originalUrl);
        }

        /// <summary>   
        /// This method returns a fully qualified absolute server Url which includes the protocol, server, port in addition to the server
        /// relative Url.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax but returns an absolute URL. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="context">      Any Url, either App relative or fully qualified. </param>
        /// <param name="serverUrl">    URL of the server. </param>
        /// <param name="forceHttps">   if true forces the url to use https - default is false. </param>
        /// <returns>   A fully qualified absolute server Url . </returns>
        public static string ResolveServerUrl(HttpContextBase context, string serverUrl, bool forceHttps = false)
        {
            if (string.IsNullOrEmpty(serverUrl) || IsAbsolutePath(serverUrl))
                return serverUrl;

            Uri result = new Uri(context.Request.Url, ResolveUrl(serverUrl));
            
            return forceHttps ? ForceUriToHttps(result).ToString()
                : result.ToString();
        }

        /// <summary>   
        /// This method returns a fully qualified absolute server Url which includes the protocol, server, port in addition to the server
        /// relative Url.
        /// 
        /// It work like Page.ResolveUrl, but adds these to the beginning. This method is useful for generating Urls for AJAX methods. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="context">      Any Url, either App relative or fully qualified. </param>
        /// <param name="serverUrl">    URL of the server. </param>
        /// <returns>   A fully qualified absolute server Url . </returns>
        public static string ResolveServerUrl(HttpContextBase context, string serverUrl)
        {
            return ResolveServerUrl(context, serverUrl, false);
        }

        private static Uri ForceUriToHttps(Uri uri)
        {
            // ** Re-write Url using builder.
            return new UriBuilder(uri) { Scheme = Uri.UriSchemeHttps, Port = 443 }.Uri;
        }

        private static bool IsAbsolutePath(string originalUrl)
        {
            // *** Absolute path - just return
            int indexOfSlashes = originalUrl.IndexOf("://");
            int indexOfQuestionMarks = originalUrl.IndexOf("?");

            if (indexOfSlashes > -1 && (indexOfQuestionMarks < 0 || (indexOfQuestionMarks > -1 && indexOfQuestionMarks > indexOfSlashes)))
                return true;

            return false;
        }
    }
}
