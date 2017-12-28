// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-10-17
// 

using cloudscribe.Core.IdentityServerIntegration.Storage;
using cloudscribe.Pagination.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Services
{
    public class ClientsManager
    {
        public ClientsManager(
            IClientCommands commands,
            IClientQueries queries,
            IHttpContextAccessor contextAccessor
            )
        {
            _commands = commands;
            _queries = queries;
            _context = contextAccessor?.HttpContext;
        }

        private IClientCommands _commands;
        private IClientQueries _queries;
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        public async Task<PagedResult<Client>> GetClients(
            string siteId,
            int pageNumber,
            int pageSize)
        {
            return await _queries.GetClients(siteId, pageNumber, pageSize, CancellationToken).ConfigureAwait(false);
        }

        public async Task<Client> FetchClient(string siteId, string clientId)
        {
            return await _queries.FetchClient(siteId, clientId, CancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> ClientExists(string siteId, string clientId)
        {
            return await _queries.ClientExists(siteId, clientId, CancellationToken).ConfigureAwait(false);
        }

        public async Task CreateClient(string siteId, Client client)
        {
            await _commands.CreateClient(siteId, client, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task UpdateClient(string siteId, Client client)
        {
            await _commands.UpdateClient(siteId, client, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task DeleteClient(string siteId, string clientId)
        {
            await _commands.DeleteClient(siteId, clientId, CancellationToken.None).ConfigureAwait(false);
        }

    }
}
