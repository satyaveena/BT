using System;
using System.Threading;
using BT.TS360API.Logging;
using Microsoft.ApplicationServer.Caching;

namespace BT.TS360API.Cache
{
    public sealed class CachingController
    {
        private const string CacheCategory = "Caching";
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private static string EnvRegion
        {
            get { return AppSettings.DistributedCacheRegion; }
        }

        private static string CacheName
        {
            get { return AppSettings.DistributedCacheName; }
        }

        private static int CacheDuration
        {
            get 
            {
                int cacheDuration = 480;

                if(!int.TryParse(AppSettings.DistributedCacheDuration, out cacheDuration))
                {
                    return 480;
                }

                return cacheDuration;
            }
        }

        private static DataCacheFactory _factory;
        private static DataCache _ts360Cache;

        private static volatile CachingController _instance;
        private static readonly object SyncRoot = new Object();

        public static CachingController Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                        _instance = new CachingController();
                }

                return _instance;
            }
        }

        private CachingController()
        {
            _ts360Cache = GetCache();
            if (_ts360Cache == null)
            {
                // re-try 5 times
                for (int i = 0; i < 5; i++)
                {
                    PricingLogger.LogDebug("CachingController", "CachingController null");
                    Thread.Sleep(500);
                    _ts360Cache = GetCache();
                    if (_ts360Cache != null)
                    {
                        break;
                    }
                }
            }
        }

        public bool IsCacheAvailable
        {
            get { return _ts360Cache != null; }
        }

        private static DataCache GetCache()
        {
            try
            {
                if (_factory == null)
                {
                    _factory = new DataCacheFactory();

                    if (_factory == null)
                    {
                        // re-try 5 times
                        for (int i = 0; i < 5; i++)
                        {
                            Thread.Sleep(500);
                            _factory = new DataCacheFactory();
                            if (_factory != null)
                            {
                                break;
                            }
                        }
                    }
                }

                if (_factory == null) return null;

                var cache = _factory.GetCache(CacheName);
                if (!string.IsNullOrEmpty(EnvRegion))
                {
                    cache.CreateRegion(EnvRegion);
                }

                return cache;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, CacheCategory);
            }
            return null;
        }

        public void Write(string cacheKey, object value)
        {
            Write(cacheKey, value, CacheDuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <param name="cacheDuration">in minutes</param>
        public void Write(string cacheKey, object value, int cacheDuration)
        {
            if (!IsCacheAvailable) return;

            _lock.EnterWriteLock();
            try
            {
                try
                {
                    _ts360Cache.Put(cacheKey, value, TimeSpan.FromMinutes(cacheDuration), EnvRegion);
                }
                catch (DataCacheException dataCacheException)
                {
                    if (dataCacheException.ErrorCode == DataCacheErrorCode.RegionDoesNotExist)
                    {
                        _ts360Cache.CreateRegion(EnvRegion);
                    }

                    // re-try
                    if (dataCacheException.ErrorCode == DataCacheErrorCode.RegionDoesNotExist
                        || dataCacheException.ErrorCode == DataCacheErrorCode.RetryLater)
                    {
                        Thread.Sleep(500);
                        _ts360Cache.Put(cacheKey, value, TimeSpan.FromMinutes(cacheDuration), EnvRegion);
                    }
                    else
                    {
                        Logger.WriteLog(dataCacheException, CacheCategory);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(ex, CacheCategory);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public object Read(string cacheKey)
        {
            if (!IsCacheAvailable) return null;

            _lock.EnterReadLock();
            try
            {
                if (_ts360Cache == null)
                    return null;

                // re-try 3 times
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        return _ts360Cache.Get(cacheKey, EnvRegion);
                    }
                    catch (DataCacheException dataCacheEx)
                    {
                        // connection issue. wait for awhile and try again.
                        if (dataCacheEx.ErrorCode == DataCacheErrorCode.RetryLater)
                            Thread.Sleep(500);
                        else
                            throw;
                    }
                }
            }
            catch (DataCacheException dataCacheException)
            {
                if (_ts360Cache == null) return null;
                if (dataCacheException.ErrorCode == DataCacheErrorCode.ReadThroughRegionDoesNotExist ||
                    dataCacheException.ErrorCode == DataCacheErrorCode.RegionDoesNotExist ||
                    dataCacheException.ErrorCode == DataCacheErrorCode.CacheAdminRegionNotPresent)
                {
                    _ts360Cache.CreateRegion(EnvRegion);
                }
                else
                {
                    Logger.WriteLog(dataCacheException, CacheCategory);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, CacheCategory);
            }
            finally
            {
                _lock.ExitReadLock();
            }
            return null;
        }

        public bool Contains(string key)
        {
            if (!IsCacheAvailable) return false;

            try
            {
                object obj = string.IsNullOrEmpty(EnvRegion) ? _ts360Cache.Get(key) : _ts360Cache.Get(key, EnvRegion);

                return obj != null;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, CacheCategory);
            }

            return false;
        }

        public object this[string key]
        {
            get
            {
                return Read(key);
            }
        }

        public void SetExpired(string cacheKey)
        {
            if (!IsCacheAvailable) return;

            try
            {
                if (string.IsNullOrEmpty(EnvRegion))
                {
                    _ts360Cache.Remove(cacheKey);
                }
                else
                {
                    _ts360Cache.Remove(cacheKey, EnvRegion);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex, CacheCategory);
            }
        }
    }
}
