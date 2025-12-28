using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.TS360API.Cache
{
    public class WFECachingController
    {
        private static volatile WFECachingController _instance;
        private static readonly object SyncRoot = new Object();

        private static Dictionary<string, object> wfeCache;

        public static WFECachingController Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        wfeCache = new Dictionary<string, object>();
                        _instance = new WFECachingController();
                    }

                }

                return _instance;
            }
        }

        public void Write(string cacheKey, object value)
        {
            if (wfeCache.ContainsKey(cacheKey))
            {
                wfeCache[cacheKey] = value;
            }
            else
            {
                wfeCache.Add(cacheKey, value);
            }
        }

        public object Read(string cacheKey)
        {
            if (wfeCache.ContainsKey(cacheKey))
            {
                return wfeCache[cacheKey];
            } 
            return null;
        }

        public void SetExpired(string cacheKey)
        {
            if (wfeCache.ContainsKey(cacheKey))
            {
                wfeCache[cacheKey] = null;
            }
        }
    }
}
