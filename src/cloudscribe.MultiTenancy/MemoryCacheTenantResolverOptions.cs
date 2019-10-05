//namespace cloudscribe.Multitenancy
//{
//    /// <summary>
//    /// Configuration options for <see cref="MemoryCacheTenantResolver{TTenant}"/>.
//    /// </summary>
//    public class MemoryCacheTenantResolverOptions
//    {
//        /// <summary>
//        /// Creates a new <see cref="MemoryCacheTenantResolverOptions"/> instance.
//        /// </summary>
//        public MemoryCacheTenantResolverOptions()
//        {
//            EvictAllEntriesOnExpiry = true;
//            DisposeOnEviction = true;
//        }

//        /// <summary>
//        /// Gets or sets a setting that determines whether all cache entries for a <see cref="TenantContext{TTenant}"/> 
//        /// instance should be evicted when any of the entries expire. Default: True.
//        /// </summary>
//        public bool EvictAllEntriesOnExpiry { get; set; }

//        /// <summary>
//        /// Gets or sets a setting that determines whether cached tenant context instances should be disposed 
//        /// when upon eviction from the cache. Default: True.
//        /// </summary>
//        public bool DisposeOnEviction { get; set; }
//    }
//}
