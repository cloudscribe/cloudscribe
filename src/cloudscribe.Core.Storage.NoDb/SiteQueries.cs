// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-05-13
// Last Modified:           2018-08-19
// 

using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.NoDb
{
    public class SiteQueries : ISiteQueries
    {
        public SiteQueries(
            IBasicQueries<SiteSettings> queries,
            IBasicQueries<SiteHost> hostQueries
            )
        {
            this.queries = queries;
            this.hostQueries = hostQueries;

        }

        private IBasicQueries<SiteSettings> queries;
        private IBasicQueries<SiteHost> hostQueries;
        
        // need custom NoDb logic for option to lookup across projects
        public async Task<ISiteSettings> Fetch(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = siteId.ToString();

            return await queries.FetchAsync(
                projectId,
                siteId.ToString(),
                cancellationToken).ConfigureAwait(false);

        }

        // need custom NoDb logic for option to lookup across projects
        public async Task<ISiteSettings> Fetch(
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allHosts = await hostQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var host = allHosts.Where(
                x => x.HostName.Equals(hostName)
                ).SingleOrDefault()
                ;

            if (host == null)
            {
                var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
                var query = from s in allSites
                            .Take(1)
                            orderby s.CreatedUtc ascending
                            select s;

                return query.SingleOrDefault();
            }

            return await queries.FetchAsync(
                projectId,
                host.SiteId.ToString(),
                cancellationToken).ConfigureAwait(false);
            
        }

        // need custom NoDb logic for option to lookup across projects
        public async Task<ISiteSettings> FetchByFolderName(
            string folderName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            ISiteSettings site = null;
            if (!string.IsNullOrEmpty(folderName) && folderName != "root")
            {
                site = allSites.Where(
                    x => x.SiteFolderName == folderName
                    ).SingleOrDefault();
                
            }

            if (site == null)
            {
                var query = from s in allSites
                            where string.IsNullOrEmpty(s.SiteFolderName)
                            orderby s.CreatedUtc ascending
                            select s;

                site = query.Take(1).SingleOrDefault();
            }

            return site;

        }

        public async Task<bool> AliasIdIsAvailable(
            Guid requestingSiteId,
            string aliasId,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var item = allSites.Where(
                    x => x.Id != requestingSiteId
                    && x.AliasId == aliasId
                    ).FirstOrDefault();
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
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var item = allSites.Where(
                    x => x.Id != requestingSiteId
                    && x.PreferredHostName == hostName
                    ).FirstOrDefault();
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
            return false;
        }

        public async Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            return allSites.ToList().Count;
        }

        public async Task<List<ISiteInfo>> GetList(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allSites
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

            return query.ToList<ISiteInfo>();
            
        }

        public async Task<int> CountOtherSites(
            Guid currentSiteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {

            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            return allSites.Where(x => x.Id != currentSiteId).ToList().Count;
        }

        public async Task<PagedResult<ISiteInfo>> GetPageOtherSites(
            Guid currentSiteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in allSites
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

            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<ISiteInfo>();

            var result = new PagedResult<ISiteInfo>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await CountOtherSites(currentSiteId, cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task<List<ISiteHost>> GetAllHosts(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allHosts = await hostQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allHosts
                        orderby x.HostName ascending
                        select x;

            return query.ToList<ISiteHost>();

        }

        public async Task<int> GetHostCount(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allHosts = await hostQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);
            return allHosts.ToList().Count;
        }

        public async Task<PagedResult<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allHosts = await hostQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            int offset = (pageSize * pageNumber) - pageSize;

            var query = from x in allHosts
                        orderby x.HostName ascending
                        select x
                        ;

            var data = query
                .Skip(offset)
                .Take(pageSize)
                .ToList<ISiteHost>();

            var result = new PagedResult<ISiteHost>();
            result.Data = data;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = await GetHostCount(cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task<List<ISiteHost>> GetSiteHosts(
            Guid siteId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allHosts = await hostQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allHosts
                        where x.SiteId == siteId
                        orderby x.HostName ascending
                        select x
                        ;

            return query.ToList<ISiteHost>();

        }

        public async Task<ISiteHost> GetSiteHost(
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allHosts = await hostQueries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allHosts
                        where x.HostName == hostName
                        orderby x.HostName ascending
                        select x
                        ;

            return query.SingleOrDefault<SiteHost>();

        }

        public async Task<List<string>> GetAllSiteFolders(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            //await EnsureProjectId().ConfigureAwait(false);
            var projectId = "default";

            var allSites = await queries.GetAllAsync(projectId, cancellationToken).ConfigureAwait(false);

            var query = from x in allSites
                        where x.SiteFolderName != null && x.SiteFolderName != ""
                        orderby x.SiteFolderName ascending
                        select x.SiteFolderName;

            var items = query.ToList<string>();

            return items;

        }
        
    }
}
