// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.DbContexts
{
    public class ConfigurationDbContextBase : DbContext
    {
        public ConfigurationDbContextBase(
            DbContextOptions options
            ) : base(options)
        {
           
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}