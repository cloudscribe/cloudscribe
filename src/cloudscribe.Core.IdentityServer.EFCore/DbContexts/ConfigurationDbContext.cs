// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace cloudscribe.Core.IdentityServer.EFCore.DbContexts
{
    public class ConfigurationDbContext : DbContext, IConfigurationDbContext
    {
        public ConfigurationDbContext(
            DbContextOptions<ConfigurationDbContext> options
            ) : base(options)
        {
           
        }


        public DbSet<Client> Clients { get; set; }
        public DbSet<Scope> Scopes { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IConfigurationModelMapper mapper = this.GetService<IConfigurationModelMapper>();
            if (mapper == null)
            {
                mapper = new SqlServerConfigurationModelMapper();
            }
            //modelBuilder.ConfigureClientContext();
            modelBuilder.Entity<Client>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientGrantType>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientRedirectUri>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientScope>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientSecret>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientClaim>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientIdPRestriction>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ClientCorsOrigin>(entity =>
            {
                mapper.Map(entity);
            });


            //modelBuilder.ConfigureScopeContext();

            modelBuilder.Entity<ScopeClaim>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<ScopeSecret>(entity =>
            {
                mapper.Map(entity);
            });
            
            modelBuilder.Entity<Scope>(entity =>
            {
                mapper.Map(entity);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}