using System.Web.Management;

namespace EPS.Web.Management
{
    /// <summary>   Internal Event codes. </summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    internal class EventCodes
    {
        private const int _offset = WebEventCodes.WebExtendedBase;

        /// <summary> Authentication failure </summary>
        public const int AuthenticationFailure = _offset + 1;
        /// <summary> Authentication success </summary>
        public const int AuthenticationSuccess = _offset + 2;
        /// <summary> The cache hit </summary>
        public const int CacheHit = _offset + 3;
        /// <summary> The cache missed </summary>
        public const int CacheMiss = _offset + 4;
        /// <summary> Added to the cache </summary>
        public const int CacheAdd = _offset + 5;
        /// <summary> Error adding to the cache </summary>
        public const int CacheAddError = _offset + 6;
    }
}
