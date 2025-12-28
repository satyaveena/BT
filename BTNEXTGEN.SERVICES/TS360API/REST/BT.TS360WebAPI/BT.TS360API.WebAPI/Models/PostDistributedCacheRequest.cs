using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BT.TS360API.WebAPI.Models
{
    public class PostDistributedCacheRequest
    {
        public string CacheName { get; set; }
        public string CacheKey { get; set; }
    }
}