// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2018-10-08
// 

using cloudscribe.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class SiteCommands : ISiteCommands, ISiteCommandsSingleton
    {
        public SiteCommands(ICoreDbContextFactory coreDbContextFactory)
        {
            _contextFactory = coreDbContextFactory;
        }

        private readonly ICoreDbContextFactory _contextFactory;
        
        public async Task Create(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (site == null) throw new ArgumentException("site must not be null");
            if (site.Id == Guid.Empty) throw new ArgumentException("site must have a non-empty Id");

            var siteSettings = SiteSettings.FromISiteSettings(site);

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.Sites.Add(siteSettings);

                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task Update(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (site == null) throw new ArgumentException("site must not be null");
            if (site.Id == Guid.Empty) throw new ArgumentException("site must have a non-empty Id");

            var siteSettings = SiteSettings.FromISiteSettings(site);
            siteSettings.LastModifiedUtc = DateTime.UtcNow;
            
            using (var dbContext = _contextFactory.CreateContext())
            {
                bool tracking = dbContext.ChangeTracker.Entries<SiteSettings>().Any(x => x.Entity.Id == siteSettings.Id);
                if (!tracking)
                {
                    dbContext.Sites.Update(siteSettings);
                }
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            }
        }

        public async Task Delete(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("id must not be empty guid");

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.Sites.SingleOrDefaultAsync(
               x => x.Id == siteId
               , cancellationToken)
               .ConfigureAwait(false);

                if (itemToRemove == null) throw new InvalidOperationException("site not found");

                dbContext.Sites.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task AddHost(
            Guid siteId,
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("siteId must not be empty guid");

            var host = new SiteHost();
            host.SiteId = siteId;
            host.HostName = hostName;

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.SiteHosts.Add(host);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task DeleteHost(
            Guid siteId,
            Guid hostId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (hostId == Guid.Empty) throw new ArgumentException("hostId must not be empty guid");

            using (var dbContext = _contextFactory.CreateContext())
            {
                var itemToRemove = await dbContext.SiteHosts.SingleOrDefaultAsync(x => x.Id == hostId, cancellationToken);
                if (itemToRemove == null) throw new InvalidOperationException("host not found");

                dbContext.SiteHosts.Remove(itemToRemove);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        public async Task DeleteHostsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("siteId must not be empty guid");

            using (var dbContext = _contextFactory.CreateContext())
            {
                var query = from x in dbContext.SiteHosts.Where(x => x.SiteId == siteId)
                            select x;

                dbContext.SiteHosts.RemoveRange(query);
                int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }


        
    }
}
