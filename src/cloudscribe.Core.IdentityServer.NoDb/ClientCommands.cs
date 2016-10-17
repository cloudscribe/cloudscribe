
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
    public class ClientCommands : IClientCommands
    {
        public ClientCommands(
            IBasicCommands<Client> commands
            )
        {
            _commands = commands;
        }

        private IBasicCommands<Client> _commands;

        public async Task CreateClient(string siteId, Client client, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.CreateAsync(siteId, client.ClientId, client, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateClient(string siteId, Client client, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.UpdateAsync(siteId, client.ClientId, client, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteClient(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.DeleteAsync(siteId, clientId, cancellationToken).ConfigureAwait(false);
        }


    }
}
