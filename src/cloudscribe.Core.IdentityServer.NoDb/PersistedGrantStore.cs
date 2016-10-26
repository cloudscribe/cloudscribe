// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-26
// 

using cloudscribe.Core.IdentityServer.NoDb.Models;
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
            IBasicQueries<GrantItem> queries,
            IBasicCommands<GrantItem> commands,
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
        private IBasicQueries<GrantItem> _queries;
        private IBasicCommands<GrantItem> _commands;

        // key is not a good thing to use to name the storage file, an example key is
        // OIvXyoxM+JDUP4c0PA6EzeYCvxcZpYdZWQn2mP+Q34Q=
        // so stored as OIvXyoxM+JDUP4c0PA6EzeYCvxcZpYdZWQn2mP+Q34Q=.json results in file system errors
        // therefore introduced GrantItem which adds a guid id

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
                var gi = g as GrantItem;
                if(gi != null)
                {
                    await _commands.DeleteAsync(siteId, gi.Id).ConfigureAwait(false);
                }
                
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
                var grant = new GrantItem(token);
                await RemoveAsync(grant.Key).ConfigureAwait(false);
                await _commands.CreateAsync(site.Id.ToString(), grant.Id, grant).ConfigureAwait(false);
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

            var all = await GetAllInternalAsync(site.Id.ToString()).ConfigureAwait(false);

            return all.Where(x => x.Key == key).FirstOrDefault();
           
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
            var found = await GetAsync(key).ConfigureAwait(false) as GrantItem;
            if(found != null)
            {
                await _commands.DeleteAsync(site.Id.ToString(), found.Id).ConfigureAwait(false);
            }
            
            
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
