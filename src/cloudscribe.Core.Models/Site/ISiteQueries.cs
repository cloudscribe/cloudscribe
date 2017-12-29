// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2017-12-29
// 

using cloudscribe.Pagination.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ISiteQueries : IDisposable
    {
        Task<ISiteSettings> Fetch(Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ISiteSettings> Fetch(string hostName, CancellationToken cancellationToken = default(CancellationToken));
        Task<ISiteSettings> FetchByFolderName(string folderName, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> AliasIdIsAvailable(
            Guid requestingSiteId,
            string aliasId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> HostNameIsAvailable(
            Guid requestingSiteId,
            string hostName,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> CountOtherSites(Guid currentSiteGuid, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ISiteInfo>> GetList(CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<ISiteInfo>> GetPageOtherSites(
            Guid currentSiteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));
        
        Task<List<ISiteHost>> GetSiteHosts(Guid siteId, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ISiteHost>> GetAllHosts(CancellationToken cancellationToken = default(CancellationToken));
        Task<ISiteHost> GetSiteHost(string hostName, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> GetHostCount(CancellationToken cancellationToken = default(CancellationToken));
        Task<PagedResult<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        Task<List<string>> GetAllSiteFolders(CancellationToken cancellationToken = default(CancellationToken));

    }
}
