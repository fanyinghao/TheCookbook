using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace FYH.Cookbook.Core.Caches
{
    internal class MemoryCacheManager : ICacheManager
    {
        private MemoryCache cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public bool Add(string key, object data, CacheExpirationTypeEnum expirationType, double? cacheMinutes)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            if (data == null)
                return false;
            if (expirationType != CacheExpirationTypeEnum.None && !cacheMinutes.HasValue)
                return false;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = expirationType == CacheExpirationTypeEnum.Absolute ? DateTime.Now.AddMinutes(cacheMinutes.Value) : Cache.NoAbsoluteExpiration;
            policy.SlidingExpiration = expirationType == CacheExpirationTypeEnum.Sliding ? TimeSpan.FromMinutes(cacheMinutes.Value) : Cache.NoSlidingExpiration;
            return cache.Add(key, data, policy);
        }

        public bool Set(string key, object data, CacheExpirationTypeEnum expirationType, double? cacheMinutes)
        {
            if (string.IsNullOrEmpty(key))
                return false;
            if (data == null)
                return false;
            if (expirationType != CacheExpirationTypeEnum.None && !cacheMinutes.HasValue)
                return false;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = expirationType == CacheExpirationTypeEnum.Absolute ? DateTime.Now.AddMinutes(cacheMinutes.Value) : Cache.NoAbsoluteExpiration;
            policy.SlidingExpiration = expirationType == CacheExpirationTypeEnum.Sliding ? TimeSpan.FromMinutes(cacheMinutes.Value) : Cache.NoSlidingExpiration;
            cache.Set(key, data, policy);
            return true;
        }

        public bool Contains(string key)
        {
            return cache.Contains(key);
        }


        public long GetCount()
        {
            return cache.GetCount();
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public T Get<T>(string key)
        {
            return (T)cache.Get(key);
        }

        public IDictionary<string, object> GetAll()
        {
            return cache.ToDictionary(i => i.Key, i => i.Value);
        }

        public IEnumerable<string> GetAllKeys()
        {
            return GetAll().Keys;
        }

        public bool Remove(string key)
        {
            return cache.Remove(key) != null;
        }

        public void RemoveAll()
        {
            foreach (var key in GetAllKeys())
            {
                Remove(key);
            }
        }
    }
}
