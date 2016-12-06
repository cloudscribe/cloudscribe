// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-12-06
// Last Modified:			2016-12-06
// 

using cloudscribe.Core.IdentityServerIntegration.Storage;
using IdentityServer4.Models;
using NoDb;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ApiResourceCommands : IApiResourceCommands
    {
        public ApiResourceCommands(
            IBasicCommands<ApiResource> commands
            )
        {
            _commands = commands;
        }

        private IBasicCommands<ApiResource> _commands;

        public async Task CreateApiResource(string siteId, ApiResource resource, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.CreateAsync(siteId, resource.Name, resource, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateApiResource(string siteId, ApiResource resource, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.UpdateAsync(siteId, resource.Name, resource, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteApiResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _commands.DeleteAsync(siteId, name, cancellationToken).ConfigureAwait(false);
        }
    }
}
