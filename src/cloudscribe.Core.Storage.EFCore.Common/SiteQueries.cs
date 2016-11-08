// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2016-08-28
// 

using cloudscribe.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class SiteQueries : ISiteQueries
    {
        public SiteQueries(ICoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private ICoreDbContext dbContext;

        public async Task<ISiteSettings> Fetch(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var item = await dbContext.Sites.AsNoTracking().SingleOrDefaultAsync(
                    x => x.Id.Equals(siteId)
                    , cancellationToken)
                    .ConfigureAwait(false);

            return item;
        }

        //public ISiteSettings FetchNonAsync(Guid siteId)
        //{
        //    SiteSettings item
        //        = dbContext.Sites.AsNoTracking().SingleOrDefault(x => x.Id.Equals(siteId));

        //    return item;
        //}

        public async Task<ISiteSettings> Fetch(
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var host = await dbContext.SiteHosts.AsNoTracking().FirstOrDefaultAsync(
                x => x.HostName.Equals(hostName)
                , cancellationToken)
                .ConfigureAwait(false);

            if (host == null)
            {
                var query = from s in dbContext.Sites
                            .Take(1)
                            orderby s.CreatedUtc ascending
                            select s;

                return await query
                    .AsNoTracking()
                    .SingleOrDefaultAsync<SiteSettings>(cancellationToken)
                    .ConfigureAwait(false);
            }

            return await dbContext.Sites
                .AsNoTracking()
                .SingleOrDefaultAsync(
                x => x.Id.Equals(host.SiteId)
                , cancellationToken)
                .ConfigureAwait(false);


        }

        /// <summary>
        /// tries to return a site with a matching folder, if not found returns the default site
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ISiteSettings> FetchByFolderName(
            string folderName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            ISiteSettings site = null;
            if (!string.IsNullOrEmpty(folderName) && folderName != "root")
            {
                site = await dbContext.Sites.AsNoTracking().FirstOrDefaultAsync(
                x => x.SiteFolderName == folderName
                , cancellationToken)
                .ConfigureAwait(false);
            }

            if (site == null)
            {
                var query = from s in dbContext.Sites
                            where string.IsNullOrEmpty(s.SiteFolderName)
                            orderby s.CreatedUtc ascending
                            select s;

                site = await query.Take(1)
                    .AsNoTracking()
                    .SingleOrDefaultAsync<SiteSettings>(cancellationToken)
                    .ConfigureAwait(false);
            }

            return site;


        }

        //public ISiteSettings FetchNonAsync(string hostName)
        //{
        //    var host = dbContext.SiteHosts.FirstOrDefault(x => x.HostName == hostName);
        //    if (host == null)
        //    {
        //        var query = from s in dbContext.Sites
        //                    .Take(1)
        //                    orderby s.CreatedUtc ascending
        //                    select s;

        //        return query.AsNoTracking().SingleOrDefault<SiteSettings>();
        //    }

        //    return dbContext.Sites.AsNoTracking().SingleOrDefault(x => x.Id == host.SiteGuid);

        //}

        //public ISiteSettings FetchByFolderNameNonAsync(string folderName)
        //{
        //    var site = dbContext.Sites
        //        .AsNoTracking()
        //        .FirstOrDefault(x => x.SiteFolderName == folderName);

        //    if (site == null)
        //    {
        //        var query = from s in dbContext.Sites
        //                    .Take(1)
        //                    orderby s.CreatedUtc ascending
        //                    select s;

        //        site = query.AsNoTracking().FirstOrDefault<SiteSettings>();
        //    }

        //    return site;

        //}

        public async Task<bool> AliasIdIsAvailable(
            Guid requestingSiteId,
            string aliasId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var item = await dbContext.Sites.FirstOrDefaultAsync(
                    x => x.Id != requestingSiteId
                    && x.AliasId == aliasId
                    ).ConfigureAwait(false);
            // if no site exists that has that alias with a different siteid then it is available
            if (item == null) { return true; }
            return false;
        }

        public async Task<bool> HostNameIsAvailable(
            Guid requestingSiteId,
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var item = await dbContext.Sites.FirstOrDefaultAsync(
                    x => x.Id != requestingSiteId
                    && x.PreferredHostName == hostName
                    ).ConfigureAwait(false);
            // if no site exists that has that host with a different siteid then it is available
            if (item == null)
            {
                var host = await GetSiteHost(hostName, cancellationToken).ConfigureAwait(false);
                if(host != null)
                {
                    if (host.SiteId != requestingSiteId) return false;
                }
                return true;
            }
            return false;
        }

        public async Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();
            return await dbContext.Sites.CountAsync<SiteSettings>(cancellationToken).ConfigureAwait(false);
        }

        public async Task<List<ISiteInfo>> GetList(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.Sites
                        orderby x.SiteName ascending
                        select new SiteInfo
                        {
                            Id = x.Id,
                            AliasId = x.AliasId,
                            IsServerAdminSite = x.IsServerAdminSite,
                            PreferredHostName = x.PreferredHostName,
                            SiteFolderName = x.SiteFolderName,
                            SiteName = x.SiteName
                        }
                        ;

            var items = await query
                .AsNoTracking()
                .ToListAsync<ISiteInfo>(cancellationToken)
                .ConfigureAwait(false);

            return items;
        }

        public async Task<int> CountOtherSites(
            Guid currentSiteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.Sites.CountAsync<SiteSettings>(
                x => x.Id != currentSiteId
                , cancellationToken);
        }

        public async Task<List<ISiteInfo>> GetPageOtherSites(
            Guid currentSiteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in dbContext.Sites

                        where (x.Id != currentSiteId)
                        orderby x.SiteName ascending
                        select new SiteInfo
                        {
                            Id = x.Id,
                            AliasId = x.AliasId,
                            IsServerAdminSite = x.IsServerAdminSite,
                            PreferredHostName = x.PreferredHostName,
                            SiteFolderName = x.SiteFolderName,
                            SiteName = x.SiteName
                        };


            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<ISiteInfo>(cancellationToken)
                .ConfigureAwait(false);


        }

        public async Task<List<ISiteHost>> GetAllHosts(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.SiteHosts
                        orderby x.HostName ascending
                        select x;

            var items = await query
                .AsNoTracking()
                .ToListAsync<ISiteHost>(cancellationToken)
                .ConfigureAwait(false);

            return items;
        }

        //public List<ISiteHost> GetAllHostsNonAsync()
        //{
        //    var query = from x in dbContext.SiteHosts
        //                orderby x.HostName ascending
        //                select x;

        //    var items = query.AsNoTracking().ToList<ISiteHost>();

        //    return items;
        //}

        public async Task<int> GetHostCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            return await dbContext.SiteHosts.CountAsync<SiteHost>(cancellationToken);
        }

        public async Task<List<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in dbContext.SiteHosts

                        orderby x.HostName ascending
                        select x
                        ;

            return await query
                .AsNoTracking()
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync<ISiteHost>(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<List<ISiteHost>> GetSiteHosts(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.SiteHosts
                        where x.SiteId == siteId
                        orderby x.HostName ascending
                        select x
                        ;

            var items = await query
                .AsNoTracking()
                .ToListAsync<ISiteHost>(cancellationToken)
                .ConfigureAwait(false);

            return items;
        }

        public async Task<ISiteHost> GetSiteHost(
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.SiteHosts
                        where x.HostName == hostName
                        orderby x.HostName ascending
                        select x
                        ;

            return await query.SingleOrDefaultAsync<SiteHost>(cancellationToken)
                .ConfigureAwait(false);

        }

        public async Task<List<string>> GetAllSiteFolders(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ThrowIfDisposed();
            cancellationToken.ThrowIfCancellationRequested();

            var query = from x in dbContext.Sites
                        where x.SiteFolderName != null && x.SiteFolderName != ""
                        orderby x.SiteFolderName ascending
                        select x.SiteFolderName;

            var items = await query.ToListAsync<string>();

            return items;

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
