using System;
using System.Web;

namespace EPS.Web
{
    /// <summary>   A set of helper extensions that sit atop <see cref="T:System.Web.Abstractions.HttpContextBase"/>.
    /// 			To use against a <see cref="T:System.Web.HttpContext"/>, use <see cref="T:System.Web.Abstractions.HttpContextWrapper"/> </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class HttpContextBaseExtensions
    {
        /// <summary>   A HttpContextBase extension method that gets a current user name by safely looking at User.Identity.Name. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentNullException">    Thrown when the HttpContextBase is null. </exception>
        /// <param name="httpContextBase">  The httpContextBase to act on. </param>
        /// <returns>   The current user name. </returns>
        public static string GetCurrentUserName(this HttpContextBase httpContextBase)
        {
            if (null == httpContextBase)
                throw new ArgumentNullException("httpContextBase");

            var user = httpContextBase.User;
            var identity = null != user ? user.Identity : null;
            return (null == identity ? string.Empty : identity.Name ?? string.Empty);
        }
    }
}
