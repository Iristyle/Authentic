using System;
using System.Web;

namespace EPS.Web
{
    /// <summary>   Some helper methods that sit atop <see cref="T:System.Web.Abstractions.HttpResponseBase"/>. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public static class HttpResponseBaseExtensions
    {
        /// <summary>   
        /// A HttpResponse extension method that disables the caching by setting Cacheability to HttpCacheability.NoCache, setting Expiration to
        /// DateTime.Now and add the pragma:no-cache header. 
        /// </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <param name="response"> The response to act on. </param>
        public static void DisableCaching(this HttpResponseBase response)
        {
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetExpires(DateTime.Now);
            response.AddHeader("pragma", "no-cache");
        }
    }
}