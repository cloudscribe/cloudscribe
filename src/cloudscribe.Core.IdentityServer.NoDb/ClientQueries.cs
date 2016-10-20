// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-17
// Last Modified:			2016-10-19
// 

using cloudscribe.Core.IdentityServer.NoDb.Models;
using cloudscribe.Core.IdentityServerIntegration.Storage;
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
            IBasicQueries<Client> queries,
            IBasicQueries<ClientClaim> claimQueries
            )
        {
            _queries = queries;
            _claimQueries = claimQueries;
        }

        private IBasicQueries<Client> _queries;
        private IBasicQueries<ClientClaim> _claimQueries;

        private async Task<List<Client>> GetAllClients(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            //TODO: cache
            var all = await _queries.GetAllAsync(siteId, cancellationToken).ConfigureAwait(false);

            return all.ToList();
        }

        private async Task<List<ClientClaim>> GetAllClientClaims(string siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            //TODO: cache
            var all = await _claimQueries.GetAllAsync(siteId, cancellationToken).ConfigureAwait(false);

            return all.ToList();
        }

        private async Task<List<ClientClaim>> GetClientClaims(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var all = await GetAllClientClaims(siteId, cancellationToken).ConfigureAwait(false);

            return all.
                Where(x => x.ClientId == clientId).ToList();
        }

        public async Task<Client> FetchClient(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var client = await _queries.FetchAsync(siteId, clientId, cancellationToken).ConfigureAwait(false);
            if(client != null)
            {
                var clientClaims = await GetClientClaims(siteId, client.ClientId, cancellationToken).ConfigureAwait(false);
                foreach(var cc in clientClaims)
                {
                    var c = new System.Security.Claims.Claim(cc.Type, cc.Value);
                    client.Claims.Add(c);
                }
            }
            

            return client;
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
