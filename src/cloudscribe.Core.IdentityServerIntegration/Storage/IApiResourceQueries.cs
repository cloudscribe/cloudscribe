// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-12-05
// Last Modified:			2017-12-28
// 

using cloudscribe.Pagination.Models;
using IdentityServer4.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Storage
{
    public interface IApiResourceQueriesSingleton : IApiResourceQueries
    {

    }

    public interface IApiResourceQueries
    {
        Task<bool> ApiResourceExists(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<ApiResource> FetchApiResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> CountApiResources(string siteId, CancellationToken cancellationToken = default(CancellationToken));

        Task<PagedResult<ApiResource>> GetApiResources(
            string siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
