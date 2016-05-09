// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-16
// Last Modified:			2016-05-09
// 

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public interface ISiteQueries : IDisposable
    {
        //TODO: review all places where these non async methods are used
        // and consider whether they could be changed to use async

        ISiteSettings FetchNonAsync(Guid siteGuid);
        ISiteSettings FetchNonAsync(string hostName);
        ISiteSettings FetchByFolderNameNonAsync(string folderName);

        //Task<ISiteSettings> Fetch(int siteId, CancellationToken cancellationToken);
        Task<ISiteSettings> Fetch(Guid siteGuid, CancellationToken cancellationToken = default(CancellationToken));
        Task<ISiteSettings> Fetch(string hostName, CancellationToken cancellationToken = default(CancellationToken));
        Task<ISiteSettings> FetchByFolderName(string folderName, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> CountOtherSites(Guid currentSiteGuid, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ISiteInfo>> GetList(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ISiteInfo>> GetPageOtherSites(
            Guid currentSiteGuid,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));



        Task<List<ISiteHost>> GetSiteHosts(Guid siteGuid, CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ISiteHost>> GetAllHosts(CancellationToken cancellationToken = default(CancellationToken));
        Task<ISiteHost> GetSiteHost(string hostName, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> GetHostCount(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<ISiteHost>> GetPageHosts(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));

        //Task<int> GetSiteIdByHostName(string hostName, CancellationToken cancellationToken);
        List<ISiteHost> GetAllHostsNonAsync();

        // we don't need multiple folders to map to a single site
        // we have foldername on the sitesettings object and on't need this extra table
        //Task<List<ISiteFolder>> GetSiteFoldersBySite(Guid siteGuid, CancellationToken cancellationToken);
        List<string> GetAllSiteFolders();

    }
}
