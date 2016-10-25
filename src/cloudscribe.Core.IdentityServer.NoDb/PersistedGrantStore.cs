// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-12
// 

using cloudscribe.Core.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        public PersistedGrantStore(
            IHttpContextAccessor contextAccessor,
            IBasicQueries<PersistedGrant> queries,
            IBasicCommands<PersistedGrant> commands,
            ILogger<PersistedGrantStore> logger
            )
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _queries = queries;
            _commands = commands;
        }

        private readonly ILogger _logger;
        private IHttpContextAccessor _contextAccessor;
        private IBasicQueries<PersistedGrant> _queries;
        private IBasicCommands<PersistedGrant> _commands;

        private async Task<IEnumerable<PersistedGrant>> GetAllInternalAsync(string siteId)
        {
            //TODO: cache
            var all = await _queries.GetAllAsync(siteId).ConfigureAwait(false);
            return all;
        }

        private async Task RemoveRange(string siteId, IEnumerable<PersistedGrant> list)
        {
            foreach (var g in list)
            {
                await _commands.DeleteAsync(siteId, g.Key).ConfigureAwait(false);
            }
        }

        public async Task StoreAsync(PersistedGrant token)
        {
            try
            {
                var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
                if(site == null)
                {
                    _logger.LogError("sitecontext was null");
                    return;
                }
                var existing = await GetAsync(token.Key).ConfigureAwait(false); 
                if (existing == null)
                {
                    if (string.IsNullOrEmpty(token.Key)) token.Key = Guid.NewGuid().ToString();
                    await _commands.CreateAsync(site.Id.ToString(), token.Key, token).ConfigureAwait(false);
                }
                else
                {
                    await _commands.UpdateAsync(site.Id.ToString(), token.Key, token).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "StoreAsync");
            }

            
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return null;
            return await _queries.FetchAsync(site.Id.ToString(), key).ConfigureAwait(false);
           
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return new List<PersistedGrant>();

            var all = await GetAllInternalAsync(site.Id.ToString()).ConfigureAwait(false);
            return all.Where(x => x.SubjectId == subjectId).ToList();
            
        }

        public async Task RemoveAsync(string key)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }
            await _commands.DeleteAsync(site.Id.ToString(), key).ConfigureAwait(false);
            
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }

            var all = await GetAllInternalAsync(site.Id.ToString()).ConfigureAwait(false);
            var persistedGrants = all.Where(x => x.SubjectId == subjectId && x.ClientId == clientId).ToList();
            await RemoveRange(site.Id.ToString(), persistedGrants).ConfigureAwait(false);  
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }

            var all = await GetAllInternalAsync(site.Id.ToString()).ConfigureAwait(false);
            var persistedGrants = all.Where(x =>
                x.SubjectId == subjectId &&
                x.ClientId == clientId &&
                x.Type == type).ToList();

            await RemoveRange(site.Id.ToString(), persistedGrants).ConfigureAwait(false);
        }


    }
}
