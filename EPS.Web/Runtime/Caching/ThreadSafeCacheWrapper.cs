using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using log4net;

namespace EPS.Runtime.Caching
{
    //TODO: 5-11-2010 -- move this to our Util library when System.Runtime.Caching gets moved to the Client profile -- maybe 4 SP1?
    /// <summary>   A class for thread-safe access to System.Runtime.Caching on a specific Type</summary>
    /// <remarks>   ebrown, 11/10/2010. </remarks>
    public class ThreadSafeCacheManager<T> where T : class
    {        
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //our static shared list of constructed wrappers... reuse ones constructed with the same type and name
        private static Dictionary<string, ThreadSafeCacheManager<T>> existingCaches = new Dictionary<string, ThreadSafeCacheManager<T>>();

        //TODO: potentially integrate PerfmonManager
        private string cacheName = string.Empty;
        private CacheItemPolicy cachePolicy;
        private Dictionary<string, ReaderWriterLockSlim> constructionLocks = new Dictionary<string, ReaderWriterLockSlim>();
        private ReaderWriterLockSlim masterCollectionLock = new ReaderWriterLockSlim();
        
        private ThreadSafeCacheManager(string cacheName, TimeSpan cacheTimeSpan, CacheItemPriority cacheItemPriority)
        {
            this.cacheName = cacheName;
            this.cachePolicy = new CacheItemPolicy()
            {
                SlidingExpiration = cacheTimeSpan,
                Priority = cacheItemPriority,
                RemovedCallback = (removed) =>
                {
                    //add to perfmon stuff
                    /*
                    if (config.Utility.Caching.PerformanceMonitoring)
                    {
                        long size = typeof(string) == CacheType ? removed.CacheItem.Value.Cast<string>().Length * 2 : 0;
                        RemoveItemFromPerfmon(cacheName, size);
                    }
                    */
                }
            };
        }

        /// <summary>   A factory method for constructing a cache manager for a given type. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="ArgumentOutOfRangeException">  Thrown when one or more arguments are outside the required range. </exception>
        /// <exception cref="ArgumentException">            Thrown when one or more arguments have unsupported or illegal values. </exception>
        /// <exception cref="Exception">                    Thrown when exception. </exception>
        /// <param name="cacheName">            Name of the cache. </param>
        /// <param name="cacheTimeSpan">        The cache time span. </param>
        /// <param name="cacheItemPriority">    The cache item priority. </param>
        /// <returns>   An instance of the cache manager. </returns>
        public static ThreadSafeCacheManager<T> Construct(string cacheName, TimeSpan cacheTimeSpan, CacheItemPriority cacheItemPriority = CacheItemPriority.Default)
        {
            if (string.IsNullOrEmpty(cacheName))
                throw new ArgumentOutOfRangeException("cacheName", "cacheName must not be null or empty");

            try
            {
                Monitor.Enter(existingCaches);
                if (existingCaches.ContainsKey(cacheName))
                {
                    //validate that T passed in is same type of existing T
                    var cache = existingCaches[cacheName];
                    if (typeof(T) != cache.GetType().GetGenericArguments().First())
                        throw new ArgumentException(String.Format("A cache with name [{0}] exists, but the type specified [{1}] does not match the existing type", cacheName, typeof(T).Name), "T");

                    return cache;
                }

                var newCache = new ThreadSafeCacheManager<T>(cacheName, cacheTimeSpan, cacheItemPriority);
                existingCaches[cacheName] = newCache;
                //if (config.Utility.Caching.PerformanceMonitoring)
                 //   DestroyAndCreatePerfmonCounters();
                return newCache;

            }
            catch (Exception ex)
            {
                log.Error(String.Format("Unexpected error constructing SimpleThreadSafeCacheWrapper<{0}>", typeof(T).Name), ex);
                throw;
            }
            finally
            {
                Monitor.Exit(existingCaches);
            }
        }

        /// <summary>   Gets the Type of the cache. </summary>
        /// <value> The type of the cache. </value>
        public Type CacheType
        {
            get { return typeof(T); }
        }

        /// <summary>   Gets an or fill cache. </summary>
        /// <remarks>   ebrown, 11/10/2010. </remarks>
        /// <exception cref="Exception">                Thrown when exception. </exception>
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are null. </exception>
        /// <param name="key">              The key. </param>
        /// <param name="fillIfMissing">    The delegate used to provide the item for the cache. </param>
        /// <returns>   The item if it exists, or after construction of the instance using the fillIfMissing delegate. </returns>
        public T GetOrFillCache(string key, Func<T> fillIfMissing)
        {
            //TODO: this code is crying for refactoring
            T cachedItem = default(T);
            string cacheKey = String.Format("{0}//{1}", cacheName, key);

            try
            {
                //very simple approach to pulling from the http cache if we already have what we need in there                
                cachedItem = MemoryCache.Default.Get(cacheKey) as T;
                if (null != cachedItem)
                    return cachedItem;

                //now we get a bit more complicated -- see if we need to put the item in there -- but first see if we've ever done that
                //by looking for a lock tied into that key
                ReaderWriterLockSlim keySpecificLock = null;
                masterCollectionLock.EnterUpgradeableReadLock();
                if (constructionLocks.ContainsKey(cacheKey))
                    keySpecificLock = constructionLocks[cacheKey];
                else
                {
                    try
                    {
                        //could have multiple threads blocking here 
                        masterCollectionLock.EnterWriteLock();

                        //did another blocked thread get here first?
                        if (constructionLocks.ContainsKey(cacheKey))
                            keySpecificLock = constructionLocks[cacheKey];
                        //nope, so create a new lock for this particular key while other threads wait
                        else
                        {
                            keySpecificLock = new ReaderWriterLockSlim();
                            constructionLocks.Add(cacheKey, keySpecificLock);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(String.Format("Error getting lock in cache [{0}] for key [{1}]", cacheName, cacheKey), ex);
                        throw;
                    }
                    finally
                    {
                        //release our write lock on our lock collection
                        masterCollectionLock.ExitWriteLock();
                    }
                }

                try
                {
                    //similar process as above ... now we have a lock that we can use for controlling creation of the new item to cache
                    //the nice thing is that this lock is tied into that key specifically so items with other keys aren't blocked or anything
                    keySpecificLock.EnterWriteLock();

                    //did another thread beat us to create this new cached dictionary
                    cachedItem = MemoryCache.Default.Get(cacheKey) as T;

                    //nope, so call the delegate to have our caller create it... then add it to the cache
                    if (null == cachedItem)
                    {
                        try
                        {
                            cachedItem = fillIfMissing();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Item creation function returned a null -- nothing to cache", ex);
                        }

                        if (null == cachedItem)
                            throw new ArgumentNullException(String.Format("Call to {0} failed -- The user supplied object creation function must return a valid object", fillIfMissing.Method.Name));

                        MemoryCache.Default.Add(cacheKey, cachedItem, cachePolicy);
                        /*
                        if (config.Utility.Caching.PerformanceMonitoring)
                        {
                            long size = typeof(string) == CacheType ? cachedItem.Cast<string>().Length * 2 : 0;
                            AddItemToPerfmon(cacheName, size);
                        }
                        */
                        /*
                        MemoryCache.Default.CreateCacheEntryChangeMonitor(new[] { cacheKey }).NotifyOnChanged((target) =>
                            {
                                //the only notification that can be made is a removal
                                if (config.Utility.Caching.PerformanceMonitoring)
                                    RemoveItemFromPerfmon(cacheName, size);
                            });
                         */
                    }
                }
                catch (Exception ex)
                {
                    log.Error(String.Format("Problem retrieving or creating new cached item in cache [{0}] for key [{1}]", cacheName, cacheKey), ex);
                    throw;
                }
                finally
                {
                    keySpecificLock.ExitWriteLock();
                }
            }
            //just let our exception bubble up
            finally
            {
                if (masterCollectionLock.IsUpgradeableReadLockHeld)
                    masterCollectionLock.ExitUpgradeableReadLock();
            }

            return cachedItem;
        }
    }
}
