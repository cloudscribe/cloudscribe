// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2015-11-16
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using cloudscribe.Core.Models;

//http://ef.readthedocs.org/en/latest/modeling/configuring.html

namespace cloudscribe.Core.Repositories.EF
{
    public class CoreDbContext : DbContext
    {
        public DbSet<SiteSettings> Sites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteSettings>(entity =>
            {
                // trying to keep table and column naming consistent so that
                // one could change from cloudscribe.Core.Repositories.MSSQL
                // to cloudscribe.Core.Repositories.EF or vice versa

                entity.ToTable("mp_Sites"); 
                entity.HasKey(p => p.SiteId);

                entity.Property(p => p.SiteId)
                .UseSqlServerIdentityColumn()
                .HasColumnName("SiteID")
                .Metadata.SentinelValue = -1
                ;

                entity.Property(p => p.SiteGuid)
                .HasDefaultValueSql("newid()")
                .Metadata.SentinelValue = Guid.Empty
                ;

                entity.Index(p => p.SiteGuid)
                .Unique();

                entity.Property(p => p.SiteName)
                .HasMaxLength(255)
                .IsRequired();

                entity.Property(p => p.Layout)
                .HasColumnName("Skin")
                .HasMaxLength(100);



            });

                

           
        }

    }
}
