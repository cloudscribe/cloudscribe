// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace cloudscribe.Core.IdentityServer.EFCore.DbContexts
{
    public class PersistedGrantDbContext : DbContext, IPersistedGrantDbContext
    {
        public PersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options) : base(options) { }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            IPersistedGrantModelMapper mapper = this.GetService<IPersistedGrantModelMapper>();
            if (mapper == null)
            {
                mapper = new SqlServerPersistedGrantModelMapper();
            }

            //modelBuilder.ConfigurePersistedGrantContext();
            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                mapper.Map(entity);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}