// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2017-10-06
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Geography;
using Microsoft.EntityFrameworkCore;


namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class CoreDbContextBase : DbContext
    {
        public CoreDbContextBase(DbContextOptions options) : base(options)
        {

            
        }

        protected CoreDbContextBase() { }

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
        public DbSet<UserToken> UserTokens { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{    
        //    base.OnConfiguring(optionsBuilder);  
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{ 

        //    base.OnModelCreating(modelBuilder);

        //}

    }
}
