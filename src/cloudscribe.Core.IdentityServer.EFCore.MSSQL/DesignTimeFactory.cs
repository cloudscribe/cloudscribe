using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL
{
    public class ConfigurationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder.UseSqlServer("Server=(local);Database=DATABASENAME;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ConfigurationDbContext(builder.Options);
        }
    }

    public class PersistedGrantDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            builder.UseSqlServer("Server=(local);Database=DATABASENAME;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new PersistedGrantDbContext(builder.Options);
        }
    }


}
