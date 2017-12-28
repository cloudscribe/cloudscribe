// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-12-05
// Last Modified:			2016-12-05
// 

using cloudscribe.Pagination.Models;
using IdentityServer4.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Storage
{
    public interface IIdentityResourceQueries
    {
        Task<bool> IdentityResourceExists(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<IdentityResource> FetchIdentityResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> CountIdentityResources(string siteId, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<IdentityResource>> GetIdentityResources(
            string siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
