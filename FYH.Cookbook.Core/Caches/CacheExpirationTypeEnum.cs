namespace FYH.Cookbook.Core.Caches
{
    public enum CacheExpirationTypeEnum
    {
        /// <summary>
        /// No expire
        /// </summary>
        None,
        /// <summary>
        /// Absolute time
        /// </summary>
        Absolute,
        /// <summary>
        /// Sliding from last time
        /// </summary>
        Sliding
    }
}
