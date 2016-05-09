// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2016-05-09
// 

using cloudscribe.Core.Models;
using Microsoft.Data.Entity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EF
{
    public class SiteCommands : ISiteCommands
    {
        public SiteCommands(CoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private CoreDbContext dbContext;

        public async Task Create(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (site == null) throw new ArgumentException("site must not be null");
            if (site.SiteGuid == Guid.Empty) throw new ArgumentException("site must have a non-empty Id");

            var siteSettings = SiteSettings.FromISiteSettings(site);
            dbContext.Sites.Add(siteSettings);
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task Update(
            ISiteSettings site,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (site == null) throw new ArgumentException("site must not be null");
            if (site.SiteGuid == Guid.Empty) throw new ArgumentException("site must have a non-empty Id");

            var siteSettings = SiteSettings.FromISiteSettings(site);
            
            bool tracking = dbContext.ChangeTracker.Entries<SiteSettings>().Any(x => x.Entity.SiteGuid == siteSettings.SiteGuid);
            if (!tracking)
            {
                dbContext.Sites.Update(siteSettings);
            }
            
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task Delete(
            Guid id,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id == Guid.Empty) throw new ArgumentException("id must not be empty guid");

            var itemToRemove = await dbContext.Sites.SingleOrDefaultAsync(
                x => x.SiteGuid == id
                , cancellationToken)
                .ConfigureAwait(false);

            if (itemToRemove == null) throw new InvalidOperationException("site not found");
            
            dbContext.Sites.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task AddHost(
            Guid siteId,
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("siteId must not be empty guid");

            var host = new SiteHost();
            host.SiteGuid = siteId;
            host.HostName = hostName;

            dbContext.SiteHosts.Add(host);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteHost(
            Guid hostId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (hostId == Guid.Empty) throw new ArgumentException("hostId must not be empty guid");

            var itemToRemove = await dbContext.SiteHosts.SingleOrDefaultAsync(x => x.HostGuid == hostId, cancellationToken);
            if (itemToRemove == null) throw new InvalidOperationException("host not found");
            
            dbContext.SiteHosts.Remove(itemToRemove);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }

        public async Task DeleteHostsBySite(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (siteId == Guid.Empty) throw new ArgumentException("siteId must not be empty guid");

            var query = from x in dbContext.SiteHosts.Where(x => x.SiteGuid == siteId)
                        select x;

            dbContext.SiteHosts.RemoveRange(query);
            int rowsAffected = await dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            
        }


        #region IDisposable Support

        private void ThrowIfDisposed()
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
