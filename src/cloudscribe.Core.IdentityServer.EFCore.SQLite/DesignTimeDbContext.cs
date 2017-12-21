using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Core.IdentityServer.EFCore.SQLite
{
    public class ConfigurationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder.UseSqlite("Data Source=cloudscribe.db");
            return new ConfigurationDbContext(builder.Options);
        }
    }

    public class PersistedGrantDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            builder.UseSqlite("Data Source=cloudscribe.db");
            return new PersistedGrantDbContext(builder.Options);
        }
    }
}
