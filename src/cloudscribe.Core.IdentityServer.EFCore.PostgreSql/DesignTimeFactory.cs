using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Core.IdentityServer.EFCore.PostgreSql
{
    public class ConfigurationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder.UseNpgsql("server=yourservername;UID=yourdatabaseusername;PWD=yourdatabaseuserpassword;database=yourdatabasename");
            return new ConfigurationDbContext(builder.Options);
        }
    }

    public class PersistedGrantDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            builder.UseNpgsql("server=yourservername;UID=yourdatabaseusername;PWD=yourdatabaseuserpassword;database=yourdatabasename");
            return new PersistedGrantDbContext(builder.Options);
        }
    }
}
