// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-10
// Last Modified:			2015-12-26
// 

using System;
using cloudscribe.Logging.Web;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace cloudscribe.Logging.EF
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(
            IServiceProvider serviceProvider,
            DbContextOptions options) : base(serviceProvider, options)
        {
            // we don't want to track any logitems because we dont edit them
            // we add them delete them and view them
            //ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
              
        }

        public DbSet<LogItem> LogItems { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ILogModelMapper mapper = this.GetService<ILogModelMapper>();
            if (mapper == null)
            {
                mapper = new SqlServerLogModelMapper();
            }

            modelBuilder.HasSequence<int>("LogIds")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.Entity<LogItem>(entity =>
            {
                mapper.Map(entity);
            });

            // should this be called before or after we do our thing?

            base.OnModelCreating(modelBuilder);

        }


    }
}
