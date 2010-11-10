using System;
using System.Web;
using System.Security.Principal;

namespace EPS.ServiceModel.Web.Abstractions
{
    /// <summary>   A testing friendly class that provides access to HttpContext.Current OR a user defined substitute. </summary>
    /// <remarks>   ebrown, 11/8/2010. </remarks>
    public class HttpContextHelper
    {
        /// <summary>   Gets or sets a Func&lt;HttpContext&gt; that can be used as a substitute in code for HttpContext.Current. </summary>
        /// <remarks>   ebrown, 11/8/2010. </remarks>
        /// <returns>   A Func&lt;HttpContext&gt; that can be evaluated to return a HttpContext</returns>
        public static Func<HttpContext> Current = () =>
        {
            return HttpContext.Current;
        };

        /// <summary>   Returns the HttpContextBase attached to the HttpRequestBase if one exists, otherwise wraps up HttpContextHelper.Current(). </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="request">  An optional request. </param>
        /// <returns>   The context. </returns>
        public static HttpContextBase GetContext(HttpRequestBase request = null)
        {
            if (null != request && null != request.RequestContext && null != request.RequestContext.HttpContext)
                return request.RequestContext.HttpContext;

            return new HttpContextWrapper(Current());
        }


        /// <summary>   Gets the user as an IPrincipal attached to the current HttpRequestBase if specified, or from HttpContextHelper.Current() </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="request">  An optional request. </param>
        /// <returns>   The context user if one exists, otherwise null. </returns>
        public static IPrincipal GetContextUser(HttpRequestBase request = null)
        {
            var context = GetContext(request);
            if (null != context) return context.User;

            return null;
        }
    }
}
