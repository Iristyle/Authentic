using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Security;
using EPS.Diagnostics;
using EPS.Security;
using EPS.Web.Abstractions;

namespace EPS.Web
{
    /// <summary>   A set of helper methods to log objects. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class LogHelpers
    {
        /// <summary>   A HttpRequestBase extension that creates a nicely formatted log entry for the object. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="request">  The request to act on. </param>
        /// <returns>   A log statement that summarizes the request. </returns>
        [LogMethod]
        public static string ToLogString(this HttpRequestBase request)
        {
            if (null == request)
            {                                
                string logString = string.Empty;
                if (null != Thread.CurrentPrincipal)
                    logString = String.Format(CultureInfo.InvariantCulture, "Current Thread Principal is a {0}{1}{2}{1}{1}", Thread.CurrentPrincipal.GetType(), Environment.NewLine, Thread.CurrentPrincipal.ToLogString());
                
                var user = HttpContextHelper.GetContextUser(request);
                if (null != user)
                    logString += String.Format(CultureInfo.InvariantCulture, "{0}{0}HTTP Context Principal is a {1}{0}{2}", Environment.NewLine, user.GetType(), user.ToLogString());

                return logString;
            }

            return GetRequestDetailsString(request.Url, request.UrlReferrer, request.Browser, request.UserAgent, request.UserHostAddress, request.Cookies);
        }

        /// <summary>   Gets a nicely formatted message given the details from a request.  Favor using ToLogString method that accepts <see cref="T:System.Web.HttpRequestBase "/> instead. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="url">              URL of the document. </param>
        /// <param name="urlReferrer">      The url referrer. </param>
        /// <param name="browser">          The browser. </param>
        /// <param name="userAgent">        The user agent. </param>
        /// <param name="userHostAddress">  The user host address. </param>
        /// <param name="cookies">          The cookies. </param>
        /// <returns>   A log friendly string that summarizes the request details. </returns>
        [SuppressMessage("Gendarme.Rules.Smells", "AvoidLongParameterListsRule", Justification = "ToLogString accepting HttpRequestBase is more appropriate in most cases")]
        public static string GetRequestDetailsString(Uri url, Uri urlReferrer, HttpBrowserCapabilitiesBase browser, string userAgent, string userHostAddress, HttpCookieCollection cookies)
        {
            string requestDetails = null != url && null != url.AbsoluteUri ? String.Format(CultureInfo.InvariantCulture, "URL: {0}{1}", url.AbsoluteUri, Environment.NewLine) : 
                string.Empty;
            
            if ((null != urlReferrer) && (null != urlReferrer.AbsoluteUri))
                requestDetails += String.Format(CultureInfo.InvariantCulture, "Referrer: {0}{1}", urlReferrer.AbsoluteUri, Environment.NewLine);
            requestDetails += String.Format(CultureInfo.InvariantCulture, "Browser (User-Agent): {0} ({1}){2}", (null != browser ? browser.Id ?? "Not Identified" : "Not Identified"), userAgent.IfMissing("NULL"), Environment.NewLine);
            requestDetails += String.Format(CultureInfo.InvariantCulture, "IP: {0}{1}", userHostAddress ?? "NULL", Environment.NewLine);
           
            if ((null != cookies) && (0 != cookies.Count))
            {
                requestDetails += String.Format(CultureInfo.InvariantCulture, "{0}Cookies:{0}{0}", Environment.NewLine);
                requestDetails += String.Format(CultureInfo.InvariantCulture, "Name\t\t\tValue{0}", Environment.NewLine);
                for (int i = 0; i < cookies.Count; ++i)
                    requestDetails += String.Format(CultureInfo.InvariantCulture, "{0}\t{1}{2}", cookies[i].Name, cookies[i].Value ?? string.Empty, Environment.NewLine);
            }

            if (null != Thread.CurrentPrincipal)
                requestDetails += String.Format(CultureInfo.InvariantCulture, "{0}{0}Current Thread Principal is a {1}{0}{2}", Environment.NewLine, Thread.CurrentPrincipal.GetType(), Thread.CurrentPrincipal.ToLogString());

            var user = HttpContextHelper.GetContextUser();
            if (null != user)
                requestDetails += String.Format(CultureInfo.InvariantCulture, "{0}{0}HTTP Context Principal is a {1}{0}{2}", Environment.NewLine, user.GetType(), user.ToLogString());

            return requestDetails;
        }        

        /// <summary>   A FormsIdentity extension method that converts a formsIdentity to a log string. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="formsIdentity">    The formsIdentity to act on. </param>
        /// <returns>   A log friendly string that summarizes the FormsIdentity. </returns>
        [LogMethod]
        public static string ToLogString(this FormsIdentity formsIdentity)
        {
            if (null == formsIdentity)
                return "FormsIdentity is null";

            return String.Format(CultureInfo.InvariantCulture, "Forms Identity - Name: {0}{1}", formsIdentity.Name ?? "N/A", Environment.NewLine)
                + String.Format(CultureInfo.InvariantCulture, "Forms Identity - Ticket: Name: {0} UserData: {1}{2}", null != formsIdentity.Ticket ? formsIdentity.Ticket.Name ?? "N/A" : "N/A", null != formsIdentity.Ticket ? formsIdentity.Ticket.UserData ?? "N/A" : "N/A", Environment.NewLine)
                + String.Format(CultureInfo.InvariantCulture, "Forms Identity - Authenticated: [{0}]{1}", (formsIdentity.IsAuthenticated ? "X" : " "), Environment.NewLine)
                + String.Format(CultureInfo.InvariantCulture, "Forms Identity - Authentication Type: {0}{1}", formsIdentity.AuthenticationType ?? "N/A", Environment.NewLine);
        }
    }
}