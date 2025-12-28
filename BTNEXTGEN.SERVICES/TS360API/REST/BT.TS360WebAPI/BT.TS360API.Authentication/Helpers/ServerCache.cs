using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.TS360API.Authentication.Helpers
{
    public class ServerCache
    {
        private static volatile ServerCache _instance;
        private static readonly object SyncRoot = new Object();

        private static Dictionary<string, object> cachedCollection;

        public static ServerCache Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        cachedCollection = new Dictionary<string, object>();
                        _instance = new ServerCache();
                    }

                }

                return _instance;
            }
        }

        public object Read(string cacheKey)
        {
            if (cachedCollection.ContainsKey(cacheKey))
            {
                return cachedCollection[cacheKey];
            }
            return null;
        }

        public void Write(string cacheKey, object value)
        {
            if (cachedCollection.ContainsKey(cacheKey))
            {
                cachedCollection[cacheKey] = value;
            }
            else
            {
                cachedCollection.Add(cacheKey, value);
            }
        }

        public void SetExpired(string cacheKey)
        {
            if (cachedCollection.ContainsKey(cacheKey))
            {
                cachedCollection[cacheKey] = null;
            }
        }
    }
}