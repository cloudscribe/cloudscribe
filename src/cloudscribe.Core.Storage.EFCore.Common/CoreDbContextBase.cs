// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2016-11-08
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

//http://ef.readthedocs.org/en/latest/modeling/configuring.html
// "If you are targeting more than one relational provider with the same model then you 
// probably want to specify a data type for each provider rather than a global one to be used for 
// all relational providers."

// ie this:
// modelBuilder.Entity<Blog>()
//         .Property(b => b.Url)
//         .HasSqlServerColumnType("varchar(200)");  //sql server specific

// rather than this:

// modelBuilder.Entity<Blog>()
//          .Property(b => b.Url)
//          .HasColumnType("varchar(200)");
//
// JA - since we do want to target more than one relational provider
// and we don't want to have dependencies on multiple providers in this project
// then we need to abstract the platform specific model mapping so the default one is for mssql
// but a different one can be injected
// that is why we are using ICoreModelMapper
// however it seems that the migrations must be generated in a platform specific way
// and since generated migration code becomes part of the project
// it seems like we would need a separate project for each platform

// https://github.com/aspnet/EntityFramework/wiki/Configuring-a-DbContext

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class CoreDbContextBase : DbContext
    {
        public CoreDbContextBase(DbContextOptions options) : base(options)
        {

        }

        public DbSet<GeoCountry> Countries { get; set; }
        public DbSet<GeoZone> States { get; set; }
        public DbSet<SiteSettings> Sites { get; set; }
        public DbSet<SiteHost> SiteHosts { get; set; }
        public DbSet<SiteUser> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<SiteRole> Roles { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.Options.
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            //ICoreModelMapper mapper = this.GetService<ICoreModelMapper>();
            //if(mapper == null)
            //{
            //    mapper = new SqlServerCoreModelMapper();
            //}


            //modelBuilder.Entity<SiteSettings>(entity =>
            //{
            //    mapper.Map(entity);
            //});

            //modelBuilder.Entity<SiteHost>(entity =>
            //{
            //    mapper.Map(entity);
            //});
            
            //modelBuilder.Entity<SiteUser>(entity =>
            //{
            //    mapper.Map(entity);
            //});

            //modelBuilder.Entity<SiteRole>(entity =>
            //{
            //    entity.Ignore(x => x.MemberCount);

            //    mapper.Map(entity);
            //});

            //modelBuilder.Entity<UserClaim>(entity =>
            //{
            //    mapper.Map(entity);
            //});

            //modelBuilder.Entity<UserLogin>(entity =>
            //{
            //    mapper.Map(entity);
            //});

            //modelBuilder.Entity<GeoCountry>(entity =>
            //{
            //    mapper.Map(entity);
            //});

            //modelBuilder.Entity<GeoZone>(entity =>
            //{
            //    mapper.Map(entity);
            //});
            
            //modelBuilder.Entity<UserRole>(entity =>
            //{
            //    mapper.Map(entity);
            //});
            
            //modelBuilder.Entity<UserLocation>(entity =>
            //{
            //    mapper.Map(entity);
            //});

            // should this be called before or after we do our thing?

            base.OnModelCreating(modelBuilder);

        }

    }
}
