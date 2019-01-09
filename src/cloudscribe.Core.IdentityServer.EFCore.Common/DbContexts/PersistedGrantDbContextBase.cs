// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.EFCore.DbContexts
{
    public class PersistedGrantDbContextBase : DbContext
    {
        public PersistedGrantDbContextBase(DbContextOptions options) : base(options) { }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}