// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-12-06
// Last Modified:			2016-12-06
// 

using cloudscribe.Core.Models.Generic;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Models;
using NoDb;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ApiResourceQueries : IApiResourceQueries
    {
        public ApiResourceQueries(
            IBasicQueries<ApiResource> queries
            )
        {
            _queries = queries;
        }

        private IBasicQueries<ApiResource> _queries;

        private async Task<List<ApiResource>> GetAllApiResources(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            //TODO: cache
            var all = await _queries.GetAllAsync(siteId).ConfigureAwait(false);

            return all.ToList();
        }

        public async Task<bool> ApiResourceExists(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var scope = await FetchApiResource(siteId, name, cancellationToken).ConfigureAwait(false);
            return (scope != null);
        }

        public async Task<ApiResource> FetchApiResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _queries.FetchAsync(siteId, name, cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> CountApiResources(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var all = await GetAllApiResources(siteId, cancellationToken).ConfigureAwait(false);
            return all.Count;
        }

        public async Task<PagedResult<ApiResource>> GetApiResources(
            string siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var all = await GetAllApiResources(siteId, cancellationToken).ConfigureAwait(false);

            int offset = (pageSize * pageNumber) - pageSize;

            var result = new PagedResult<ApiResource>();
            result.TotalItems = all.Count;
            result.Data = all
                .OrderBy(x => x.Name)
                .Skip(offset)
                .Take(pageSize).ToList();

            return result;

        }
    }
}
