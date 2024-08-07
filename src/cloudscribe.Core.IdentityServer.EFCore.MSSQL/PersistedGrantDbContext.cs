﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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


        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention_IS4());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.ToTable(EfConstants.TableNames.PersistedGrant);


                entity.HasKey(x => new { x.Key, x.Type });

                //2.3 changed to this but not sure I really need to change it
                // and it seems dangerous to do so
                //grant.HasKey(x => x.Key);

                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);

                entity.Property(x => x.Key).HasMaxLength(200);
                entity.Property(x => x.Type).HasMaxLength(50);
                entity.Property(x => x.SubjectId).HasMaxLength(200);
                entity.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                entity.Property(x => x.CreationTime).IsRequired();
                //changed to nullable in 2.3
                //entity.Property(x => x.Expiration).IsRequired();
                entity.Property(x => x.Data).IsRequired();

                entity.HasIndex(x => x.SubjectId);
                entity.HasIndex(x => new { x.SubjectId, x.ClientId });
                entity.HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });

            });

            modelBuilder.Entity<DeviceFlowCodes>(entity =>
            {
                entity.ToTable("csids_DeviceFlowCodes");

                entity.Property(x => x.SiteId).HasMaxLength(36).IsRequired();
                entity.HasIndex(x => x.SiteId);

                entity.Property(x => x.DeviceCode).HasMaxLength(200).IsRequired();
                entity.Property(x => x.UserCode).HasMaxLength(200).IsRequired();
                entity.Property(x => x.SubjectId).HasMaxLength(200);
                entity.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                entity.Property(x => x.CreationTime).IsRequired();
                entity.Property(x => x.Expiration).IsRequired();
                // 50000 chosen to be explicit to allow enough size to avoid truncation, yet stay beneath the MySql row size limit of ~65K
                // apparently anything over 4K converts to nvarchar(max) on SqlServer
                entity.Property(x => x.Data).HasMaxLength(50000).IsRequired();

                entity.HasKey(x => new { x.UserCode });

                entity.HasIndex(x => x.DeviceCode).IsUnique();
            });
        }
    }
}