// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-11-16
// Last Modified:			2018-08-19
// 

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class SiteQueries : ISiteQueries, ISiteQueriesSingleton
    {
        public SiteQueries(ICoreDbContextFactory coreDbContextFactory)
        {
            _contextFactory = coreDbContextFactory;
        }

        private readonly ICoreDbContextFactory _contextFactory;
        
        public async Task<ISiteSettings> Fetch(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var item = await _db.Sites
               .AsNoTracking()
               .SingleOrDefaultAsync(
                   x => x.Id.Equals(siteId)
                   , cancellationToken)
                   .ConfigureAwait(false);

                return item;
            }

        }
        
        public async Task<ISiteSettings> Fetch(
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var host = await _db.SiteHosts
                .AsNoTracking()
                .FirstOrDefaultAsync(
                x => x.HostName.Equals(hostName)
                , cancellationToken)
                .ConfigureAwait(false);

                if (host == null)
                {
                    
                    return await _db.Sites
                        .AsNoTracking()
                        .OrderBy(s => s.CreatedUtc)
                        .FirstOrDefaultAsync(cancellationToken)
                        .ConfigureAwait(false)
                        ;
                }

                return await _db.Sites
                    .AsNoTracking()
                    .SingleOrDefaultAsync(
                    x => x.Id.Equals(host.SiteId)
                    , cancellationToken)
                    .ConfigureAwait(false);
            }
            
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
            cancellationToken.ThrowIfCancellationRequested();

            ISiteSettings site = null;

            using (var _db = _contextFactory.CreateContext())
            {
                if (!string.IsNullOrEmpty(folderName) && folderName != "root")
                {
                    site = await _db.Sites
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.SiteFolderName == folderName
                    , cancellationToken)
                    .ConfigureAwait(false);
                }

                if (site == null)
                {
                    site = await _db.Sites
                        .AsNoTracking()
                        .Where(x => string.IsNullOrEmpty(x.SiteFolderName))
                        .OrderBy(x => x.CreatedUtc)
                        .FirstOrDefaultAsync(cancellationToken)
                        .ConfigureAwait(false);
                }

                return site;
            }
            
        }
        
        public async Task<bool> AliasIdIsAvailable(
            Guid requestingSiteId,
            string aliasId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var item = await _db.Sites.FirstOrDefaultAsync(
                    x => x.Id != requestingSiteId
                    && x.AliasId == aliasId
                    ).ConfigureAwait(false);
                // if no site exists that has that alias with a different siteid then it is available
                if (item == null) { return true; }
                return false;
            }
            
        }

        public async Task<bool> HostNameIsAvailable(
            Guid requestingSiteId,
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var item = await _db.Sites.FirstOrDefaultAsync(
                    x => x.Id != requestingSiteId
                    && x.PreferredHostName == hostName
                    ).ConfigureAwait(false);
                // if no site exists that has that host with a different siteid then it is available
                if (item == null)
                {
                    var host = await GetSiteHost(hostName, cancellationToken).ConfigureAwait(false);
                    if (host != null)
                    {
                        if (host.SiteId != requestingSiteId) return false;
                    }
                    return true;
                }
            }

            
            return false;
        }

        public async Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var _db = _contextFactory.CreateContext())
            {
                return await _db.Sites.CountAsync<SiteSettings>(cancellationToken).ConfigureAwait(false);
            }
           
        }

        public async Task<List<ISiteInfo>> GetList(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.Sites
                            orderby x.SiteName ascending
                            select new SiteInfo
                            {
                                Id = x.Id,
                                AliasId = x.AliasId,
                                IsServerAdminSite = x.IsServerAdminSite,
                                PreferredHostName = x.PreferredHostName,
                                SiteFolderName = x.SiteFolderName,
                                SiteName = x.SiteName,
                                CreatedUtc = x.CreatedUtc,
                                LastModifiedUtc = x.LastModifiedUtc
                            }
                        ;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<ISiteInfo>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }

        }

        public async Task<int> CountOtherSites(
            Guid currentSiteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                return await _db.Sites.CountAsync<SiteSettings>(
                x => x.Id != currentSiteId
                , cancellationToken);
            }
            
        }

        public async Task<PagedResult<ISiteInfo>> GetPageOtherSites(
            Guid currentSiteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.Sites

                            where (x.Id != currentSiteId)
                            orderby x.SiteName ascending
                            select new SiteInfo
                            {
                                Id = x.Id,
                                AliasId = x.AliasId,
                                IsServerAdminSite = x.IsServerAdminSite,
                                PreferredHostName = x.PreferredHostName,
                                SiteFolderName = x.SiteFolderName,
                                SiteName = x.SiteName,
                                CreatedUtc = x.CreatedUtc,
                                LastModifiedUtc = x.LastModifiedUtc
                            };


                var data = await query
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<ISiteInfo>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<ISiteInfo>
                {
                    Data = data,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = await CountOtherSites(currentSiteId, cancellationToken).ConfigureAwait(false)
                };
                return result;
            }
            
        }

        public async Task<List<ISiteHost>> GetAllHosts(CancellationToken cancellationToken = default(CancellationToken))
        {

            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.SiteHosts
                            orderby x.HostName ascending
                            select x;

                var items = await query
                    .AsNoTracking()
                    .ToListAsync<ISiteHost>(cancellationToken)
                    .ConfigureAwait(false);

                return items;
            }
            
        }

       
        public async Task<int> GetHostCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                return await _db.SiteHosts.CountAsync<SiteHost>(cancellationToken);
            }
            
        }

        public async Task<PagedResult<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            int offset = (pageSize * pageNumber) - pageSize;

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.SiteHosts

                            orderby x.HostName ascending
                            select x
                        ;

                var data = await query
                    .AsNoTracking()
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync<ISiteHost>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<ISiteHost>();
                result.Data = data;
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await GetHostCount(cancellationToken).ConfigureAwait(false);
                return result;

            }

            

        }

        public async Task<List<ISiteHost>> GetSiteHosts(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.SiteHosts
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
            
        }

        public async Task<ISiteHost> GetSiteHost(
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.SiteHosts
                            where x.HostName == hostName
                            orderby x.HostName ascending
                            select x
                        ;

                return await query.SingleOrDefaultAsync<SiteHost>(cancellationToken)
                    .ConfigureAwait(false);
            }
            
        }

        public async Task<List<string>> GetAllSiteFolders(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var _db = _contextFactory.CreateContext())
            {
                var query = from x in _db.Sites
                            where x.SiteFolderName != null && x.SiteFolderName != ""
                            orderby x.SiteFolderName ascending
                            select x.SiteFolderName;

                var items = await query.ToListAsync<string>();

                return items;
            }

           

        }


        

       
    }
}
