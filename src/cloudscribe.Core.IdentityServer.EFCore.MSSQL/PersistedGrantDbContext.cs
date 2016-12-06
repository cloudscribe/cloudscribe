// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using cloudscribe.Core.IdentityServer.EFCore.DbContexts;
using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.MSSQL
{
    public class PersistedGrantDbContext : PersistedGrantDbContextBase, IPersistedGrantDbContext
    {
        public PersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options) : base(options) { }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.PersistedGrant);
                entity.HasKey(x => new { x.Key, x.Type });

                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);

                entity.Property(x => x.Key).HasMaxLength(200);
                entity.Property(x => x.Type).HasMaxLength(50);
                entity.Property(x => x.SubjectId).HasMaxLength(200);
                entity.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                entity.Property(x => x.CreationTime).IsRequired();
                entity.Property(x => x.Expiration).IsRequired();
                entity.Property(x => x.Data).IsRequired();

                entity.HasIndex(x => x.SubjectId);
                entity.HasIndex(x => new { x.SubjectId, x.ClientId });
                entity.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}