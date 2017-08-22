using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.IdentityServer.EFCore.MySql
{
    public class ConfigurationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder.UseMySql("Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;");

            return new ConfigurationDbContext(builder.Options);
        }
    }

    public class PersistedGrantDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            builder.UseMySql("Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;");

            return new PersistedGrantDbContext(builder.Options);
        }
    }
}
