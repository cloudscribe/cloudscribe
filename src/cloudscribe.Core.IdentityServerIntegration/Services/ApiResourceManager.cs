// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-10-14
// 

using cloudscribe.Core.IdentityServerIntegration.Storage;
using cloudscribe.Pagination.Models;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Services
{
    public class ApiResourceManager
    {
        public ApiResourceManager(
            IApiResourceCommands commands,
            IApiResourceQueries queries,
            IHttpContextAccessor contextAccessor
            )
        {
            _commands = commands;
            _queries = queries;
            _context = contextAccessor?.HttpContext;
        }

        private IApiResourceCommands _commands;
        private IApiResourceQueries _queries;
        private readonly HttpContext _context;
        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        public async Task<PagedResult<ApiResource>> GetApiResources(
            string siteId,
            int pageNumber,
            int pageSize)
        {
            return await _queries.GetApiResources(siteId, pageNumber, pageSize, CancellationToken).ConfigureAwait(false);
        }

        public async Task<ApiResource> FetchApiResource(string siteId, string name)
        {
            return await _queries.FetchApiResource(siteId, name, CancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> ApiResourceExists(string siteId, string name)
        {
            return await _queries.ApiResourceExists(siteId, name, CancellationToken).ConfigureAwait(false);
        }

        public async Task CreateApiResource(string siteId, ApiResource resource)
        {
            await _commands.CreateApiResource(siteId, resource, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task UpdateApiResource(string siteId, ApiResource resource)
        {
            await _commands.UpdateApiResource(siteId, resource, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task DeleteApiResource(string siteId, string name)
        {
            await _commands.DeleteApiResource(siteId, name, CancellationToken.None).ConfigureAwait(false);
        }

        
    }
}
