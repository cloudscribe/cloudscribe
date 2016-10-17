// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-17
// Last Modified:			2016-10-17
// 

using cloudscribe.Core.IdentityServerIntegration.StorageModels;
using cloudscribe.Core.Models.Generic;
using IdentityServer4.Models;
using NoDb;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ClientQueries : IClientQueries
    {
        public ClientQueries(
            IBasicQueries<Client> queries
            )
        {
            _queries = queries;
        }

        private IBasicQueries<Client> _queries;

        private async Task<List<Client>> GetAllClients(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            //TODO: cache
            var all = await _queries.GetAllAsync(siteId, cancellationToken).ConfigureAwait(false);

            return all.ToList();
        }

        public async Task<Client> FetchClient(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _queries.FetchAsync(siteId, clientId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> ClientExists(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var client = await FetchClient(siteId, clientId, cancellationToken).ConfigureAwait(false);
            return (client != null);
        }

        public async Task<int> CountClients(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var all = await GetAllClients(siteId, cancellationToken).ConfigureAwait(false);
            return all.Count;
        }

        public async Task<PagedResult<Client>> GetClients(
            string siteId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var all = await GetAllClients(siteId, cancellationToken).ConfigureAwait(false);

            int offset = (pageSize * pageNumber) - pageSize;

            var result = new PagedResult<Client>();
            result.TotalItems = all.Count;
            result.Data = all
                .OrderBy(x => x.ClientName)
                .Skip(offset)
                .Take(pageSize).ToList();

            return result;
        }

    }
}
