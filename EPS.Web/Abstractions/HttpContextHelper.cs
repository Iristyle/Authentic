using System;
using System.Web;
using System.Security.Principal;

namespace EPS.Web.Abstractions
{
    /// <summary>   A testing friendly class that provides access to HttpContext.Current OR a user defined substitute. </summary>
    /// <remarks>   ebrown, 11/8/2010. </remarks>
    public static class HttpContextHelper
    {
        private static bool currentUserDefined = false;
        private static Func<HttpContext> current = () =>
        {
            return HttpContext.Current;
        };

        /// <summary>   Gets or sets a <see cref="Func{Httpcontext}"/> that can be used as a substitute in code for HttpContext.Current. </summary>
        /// <remarks>   ebrown, 11/8/2010. </remarks>
        /// <returns>   A <see cref="Func{HttpContext}"/> that can be evaluated to return a HttpContext</returns>
        public static Func<HttpContext> Current
        {
            get { return current; }
            set
            {
                lock (typeof(HttpContextHelper))
                {
                    if (currentUserDefined)
                        throw new InvalidOperationException("The Current Func<HttpContext> may only be set once per AppDomain");

                    current = value;
                    currentUserDefined = true;
                }
            }
        }

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
