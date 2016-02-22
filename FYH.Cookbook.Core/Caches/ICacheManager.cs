using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYH.Cookbook.Core.Caches
{
    public interface ICacheManager
    {
        bool Add(string key, object data, CacheExpirationTypeEnum expirationType, double? cacheMinutes);

        bool Set(string key, object data, CacheExpirationTypeEnum expirationType, double? cacheMinutes);

        bool Contains(string key);

        long GetCount();

        object Get(string key);

        T Get<T>(string key);

        IDictionary<string, object> GetAll();

        IEnumerable<string> GetAllKeys();

        bool Remove(string key);

        void RemoveAll();
    }
}
