using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using EPS.Text;

namespace EPS.Web
{
    /// <summary>   Some helper classes for working with web goop. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class WebHelpers
    {
        /// <summary>   Encodes a NameValueCollection full of parameters as a POST friendly string. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="parameters">   The parameters. </param>
        /// <returns>   a string that can be used to POST with. </returns>
        public static string EncodePostString(NameValueCollection parameters)
        {
            return string.Join("&", parameters.AllKeys.Where(k => null != k)
                .Select(key => string.Format(CultureInfo.InvariantCulture, "{0}={1}", HttpUtility.UrlEncodeUnicode(key), HttpUtility.UrlEncodeUnicode(parameters[key]))));            
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
                    return String.Format(CultureInfo.InvariantCulture, "{0}{1}={2}", prepend.ToEnumValueString(), HttpUtility.UrlEncode(name), HttpUtility.UrlEncode(value));

                return String.Format(CultureInfo.InvariantCulture, "{0}{1}={2}", prepend.ToEnumValueString(), HttpUtility.HtmlEncode(name), HttpUtility.HtmlEncode(value));
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
        /// <param name="originalUri">  Any Url including those starting with ~. </param>
        /// <returns>   Site relative url. </returns>
        public static Uri ResolveUrl(Uri originalUri)
        {
            if (string.IsNullOrEmpty(originalUri.OriginalString) ||
                originalUri.IsAbsoluteUri ||
                // *** We don't start with the '~' -> we don't process the Url
                !originalUri.OriginalString.StartsWith("~"))
                return originalUri;

            // *** Fix up path for ~ root app dir directory
            // VirtualPathUtility blows up if there is a 
            // query string, so we have to account for this.
            string queryString = originalUri.Query;

            return !string.IsNullOrWhiteSpace(queryString) ?
                new Uri(VirtualPathUtility.ToAbsolute(originalUri.GetLeftPart(UriPartial.Path)) + queryString) : 
                new Uri(VirtualPathUtility.ToAbsolute(originalUri.OriginalString));
        }

        /// <summary>   
        /// This method returns a fully qualified absolute server Url which includes the protocol, server, port in addition to the server
        /// relative Url.
        /// 
        /// Works like Control.ResolveUrl including support for ~ syntax but returns an absolute URL. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="context">      Any Url, either App relative or fully qualified. </param>
        /// <param name="serverUri">    URL of the server. </param>
        /// <param name="forceHttps">   if true forces the url to use https - default is false. </param>
        /// <returns>   A fully qualified absolute server Uri . </returns>
        public static Uri ResolveServerUrl(HttpContextBase context, Uri serverUri, bool forceHttps = false)
        {
            if (null == serverUri || string.IsNullOrEmpty(serverUri.AbsolutePath) || serverUri.IsAbsoluteUri)
                return serverUri;

            Uri result = new Uri(context.Request.Url, ResolveUrl(serverUri));
            
            return forceHttps ? ForceUriToHttps(result): result;
        }

        /// <summary>   
        /// This method returns a fully qualified absolute server Url which includes the protocol, server, port in addition to the server
        /// relative Url.
        /// 
        /// It work like Page.ResolveUrl, but adds these to the beginning. This method is useful for generating Urls for AJAX methods. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="context">      Any Url, either App relative or fully qualified. </param>
        /// <param name="serverUri">    URL of the server. </param>
        /// <returns>   A fully qualified absolute server Uri . </returns>
        public static Uri ResolveServerUrl(HttpContextBase context, Uri serverUri)
        {
            return ResolveServerUrl(context, serverUri, false);
        }

        private static Uri ForceUriToHttps(Uri uri)
        {
            // ** Re-write Url using builder.
            return new UriBuilder(uri) { Scheme = Uri.UriSchemeHttps, Port = 443 }.Uri;
        }

        //deprecated
        /*
        private static bool IsAbsolutePath(string originalUrl)
        {
            // *** Absolute path - just return
            int indexOfSlashes = originalUrl.IndexOf("://", StringComparison.OrdinalIgnoreCase);
            int indexOfQuestionMarks = originalUrl.IndexOf("?", StringComparison.OrdinalIgnoreCase);

            if (indexOfSlashes > -1 && (indexOfQuestionMarks < 0 || (indexOfQuestionMarks > -1 && indexOfQuestionMarks > indexOfSlashes)))
                return true;

            return false;
        }
        */
    }
}
