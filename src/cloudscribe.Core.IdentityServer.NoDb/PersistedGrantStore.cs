// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-12
// 

using cloudscribe.Core.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
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
            SiteContext site,
            IBasicQueries<PersistedGrant> queries,
            IBasicCommands<PersistedGrant> commands,
            ILogger<PersistedGrantStore> logger
            )
        {
            _siteId = site.Id.ToString();
            _logger = logger;
            _queries = queries;
            _commands = commands;
        }

        private readonly ILogger _logger;
        private string _siteId;
        private IBasicQueries<PersistedGrant> _queries;
        private IBasicCommands<PersistedGrant> _commands;

        private async Task<IEnumerable<PersistedGrant>> GetAllAsync()
        {
            //TODO: cache
            var all = await _queries.GetAllAsync(_siteId).ConfigureAwait(false);
            return all;
        }

        private async Task RemoveRange(IEnumerable<PersistedGrant> list)
        {
            foreach (var g in list)
            {
                await _commands.DeleteAsync(_siteId, g.Key).ConfigureAwait(false);
            }
        }

        public async Task StoreAsync(PersistedGrant token)
        {
            try
            {
                var existing = await GetAsync(token.Key).ConfigureAwait(false); 
                if (existing == null)
                {
                    if (string.IsNullOrEmpty(token.Key)) token.Key = Guid.NewGuid().ToString();
                    await _commands.CreateAsync(_siteId, token.Key, token).ConfigureAwait(false);
                }
                else
                {
                    await _commands.UpdateAsync(_siteId, token.Key, token).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "StoreAsync");
            }

            
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            return await _queries.FetchAsync(_siteId, key).ConfigureAwait(false);
           
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var all = await GetAllAsync().ConfigureAwait(false);
            return all.Where(x => x.SubjectId == subjectId).ToList();
            
        }

        public async Task RemoveAsync(string key)
        {
            await _commands.DeleteAsync(_siteId, key).ConfigureAwait(false);
            
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var all = await GetAllAsync().ConfigureAwait(false);
            var persistedGrants = all.Where(x => x.SubjectId == subjectId && x.ClientId == clientId).ToList();
            await RemoveRange(persistedGrants).ConfigureAwait(false);  
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var all = await GetAllAsync().ConfigureAwait(false);
            var persistedGrants = all.Where(x =>
                x.SubjectId == subjectId &&
                x.ClientId == clientId &&
                x.Type == type).ToList();

            await RemoveRange(persistedGrants).ConfigureAwait(false);
        }


    }
}
