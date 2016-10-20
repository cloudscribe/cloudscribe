// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-19
// 

using cloudscribe.Core.IdentityServer.NoDb.Models;
using cloudscribe.Core.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ClientStore : IClientStore
    {
        public ClientStore(
            SiteContext site,
            IBasicQueries<Client> queries,
            IBasicQueries<ClientClaim> claimQueries
            )
        {
            _siteId = site.Id.ToString();
            _queries = queries;
            _claimQueries = claimQueries;
        }

        private IBasicQueries<Client> _queries;
        private IBasicQueries<ClientClaim> _claimQueries;
        private string _siteId;

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

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client =  await _queries.FetchAsync(
                _siteId, // aka nodb projectid
                clientId 
                ).ConfigureAwait(false);

            if (client != null)
            {
                var clientClaims = await GetClientClaims(_siteId, client.ClientId).ConfigureAwait(false);
                foreach (var cc in clientClaims)
                {
                    var c = new System.Security.Claims.Claim(cc.Type, cc.Value);
                    client.Claims.Add(c);
                }
            }

            return client;
        }
    }
}
