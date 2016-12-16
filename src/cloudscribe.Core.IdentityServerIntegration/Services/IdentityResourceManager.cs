// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-12-15
// 

using cloudscribe.Core.IdentityServerIntegration.Storage;
using cloudscribe.Core.Models.Generic;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Services
{
    public class IdentityResourceManager
    {
        public IdentityResourceManager(
            IIdentityResourceCommands commands,
            IIdentityResourceQueries queries,
            IHttpContextAccessor contextAccessor
            )
        {
            _commands = commands;
            _queries = queries;
            _context = contextAccessor?.HttpContext;
        }

        private IIdentityResourceCommands _commands;
        private IIdentityResourceQueries _queries;
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        public async Task<PagedResult<IdentityResource>> GetIdentityResources(
            string siteId,
            int pageNumber,
            int pageSize)
        {
            return await _queries.GetIdentityResources(siteId, pageNumber, pageSize, CancellationToken).ConfigureAwait(false);
        }

        public async Task<IdentityResource> FetchIdentityResource(string siteId, string name)
        {
            return await _queries.FetchIdentityResource(siteId, name, CancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> IdentityResourceExists(string siteId, string name)
        {
            return await _queries.IdentityResourceExists(siteId, name, CancellationToken).ConfigureAwait(false);
        }

        public async Task CreateIdentityResource(string siteId, IdentityResource resource)
        {
            await _commands.CreateIdentityResource(siteId, resource, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task UpdateIdentityResource(string siteId, IdentityResource resource)
        {
            await _commands.UpdateIdentityResource(siteId, resource, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task DeleteIdentityResource(string siteId, string name)
        {
            await _commands.DeleteIdentityResource(siteId, name, CancellationToken.None).ConfigureAwait(false);
        }

        
    }
}
