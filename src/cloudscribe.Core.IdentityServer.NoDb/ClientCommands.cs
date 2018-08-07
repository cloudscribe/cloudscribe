
// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-17
// Last Modified:			2016-10-19
// 

using cloudscribe.Core.IdentityServer.NoDb.Models;
using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Models;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ClientCommands : IClientCommands
    {
        public ClientCommands(
            IBasicCommands<Client> commands,
            IBasicCommands<ClientClaim> claimCommands,
            IBasicQueries<ClientClaim> claimQueries
            )
        {
            _commands = commands;
            _claimCommands = claimCommands;
            _claimQueries = claimQueries;
        }

        private IBasicCommands<Client> _commands;
        private IBasicCommands<ClientClaim> _claimCommands;
        private IBasicQueries<ClientClaim> _claimQueries;

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

        private async Task DeleteClientClaims(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var claims = await GetClientClaims(siteId, clientId, cancellationToken).ConfigureAwait(false);
            foreach(var cc in claims)
            {
                await _claimCommands.DeleteAsync(siteId, cc.Id, cancellationToken).ConfigureAwait(false);
            }

        }

        private async Task SaveClientClaims(string siteId, Client client, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach(var c in client.Claims)
            {
                var cc = new ClientClaim
                {
                    Id = Guid.NewGuid().ToString(),
                    ClientId = client.ClientId,
                    Type = c.Type,
                    Value = c.Value
                };

                await _claimCommands.CreateAsync(siteId, cc.Id, cc, cancellationToken).ConfigureAwait(false);

            }

        }

        public async Task CreateClient(string siteId, Client client, CancellationToken cancellationToken = default(CancellationToken))
        {
            // claims can only be added during update
            await _commands.CreateAsync(siteId, client.ClientId, client, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateClient(string siteId, Client client, CancellationToken cancellationToken = default(CancellationToken))
        {
            await DeleteClientClaims(siteId, client.ClientId, cancellationToken).ConfigureAwait(false);
            await SaveClientClaims(siteId, client, cancellationToken).ConfigureAwait(false);
            // remove the System.Security.Claims because we can't deserialize them
            client.Claims.Clear();
            await _commands.UpdateAsync(siteId, client.ClientId, client, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteClient(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.DeleteAsync(siteId, clientId, cancellationToken).ConfigureAwait(false);
        }


    }
}
