using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL
{
    public class ConfigurationDbContextFactory : IConfigurationDbContextFactory
    {
        public ConfigurationDbContextFactory(DbContextOptions<ConfigurationDbContext> options)
        {
            _options = options;
        }

        private DbContextOptions<ConfigurationDbContext> _options;

        public IConfigurationDbContext CreateContext()
        {
            return new ConfigurationDbContext(_options);
        }
    }

    public class PersistedGrantDbContextFactory : IPersistedGrantDbContextFactory
    {
        public PersistedGrantDbContextFactory(DbContextOptions<PersistedGrantDbContext> options)
        {
            _options = options;
        }

        private DbContextOptions<PersistedGrantDbContext> _options;

        public IPersistedGrantDbContext CreateContext()
        {
            return new PersistedGrantDbContext(_options);
        }
    }
}
