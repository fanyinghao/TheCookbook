using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.Core.Utilities;

namespace FYH.Cookbook.Core.Caches
{
    /// <summary>
    /// Cache Manager
    /// </summary>
    public static class CacheManager
    {
        /// <summary>
        /// Memory Cache
        /// </summary>
        public static ICacheManager MemoryCacheManager
        {
            get { return Singleton<MemoryCacheManager>.Instance; }
        }
    }
}
