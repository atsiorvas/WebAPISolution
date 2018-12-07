using Apache.Ignite.EntityFramework;
using Apache.Ignite.Core.Cache.Configuration;
using Apache.Ignite.Core;

namespace Repository {
    public class MyDbConfiguration : IgniteDbConfiguration {
        public MyDbConfiguration()
          : base(
            // IIgnite instance to use
            Ignition.Start(),
            // Metadata cache configuration (small cache, does not tolerate data loss)
            // Should be replicated or partitioned with backups
            new CacheConfiguration("metaCache") {
                CacheMode = CacheMode.Replicated
            },
            // Data cache configuration (large cache, holds actual query results, 
            // tolerates data loss). Can have no backups.
            new CacheConfiguration("dataCache") {
                CacheMode = CacheMode.Partitioned,
                Backups = 0
            },
            // Custom caching policy.
            new DbCachingPolicy()) {
            // No-op.
        }
    }
}
