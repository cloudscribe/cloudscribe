using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Core.IdentityServer.EFCore.MySql
{
    public class ConfigurationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            var connString = "Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;";

            builder.UseMySql(connString, ServerVersion.AutoDetect(connString));


            return new ConfigurationDbContext(builder.Options);
        }
    }

    public class PersistedGrantDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            var connString = "Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;";

            builder.UseMySql(connString, ServerVersion.AutoDetect(connString));

            return new PersistedGrantDbContext(builder.Options);
        }
    }
}
