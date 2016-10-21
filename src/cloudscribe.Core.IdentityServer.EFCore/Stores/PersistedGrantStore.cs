// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.IdentityServer.EFCore.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IPersistedGrantDbContext _context;
        private readonly ILogger _logger;
        private string _siteId;

        public PersistedGrantStore(
            SiteContext site,
            IPersistedGrantDbContext context, 
            ILogger<PersistedGrantStore> logger)
        {
            _siteId = site.Id.ToString();
            _context = context;
            _logger = logger;
        }

        public async Task StoreAsync(PersistedGrant token)
        {
            var existing = await _context.PersistedGrants.SingleOrDefaultAsync(x => x.SiteId == _siteId && x.Key == token.Key)
                .ConfigureAwait(false);
            if (existing == null)
            {
                var persistedGrant = token.ToEntity();
                persistedGrant.SiteId = _siteId;
                _context.PersistedGrants.Add(persistedGrant);
            }
            else
            {
                token.UpdateEntity(existing);
            }

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "StoreAsync");
            }  
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = await _context.PersistedGrants.FirstOrDefaultAsync(x => x.SiteId == _siteId && x.Key == key)
                .ConfigureAwait(false);
            var model = persistedGrant.ToModel();

            return model;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = await _context.PersistedGrants
                .Where(x => x.SiteId == _siteId && x.SubjectId == subjectId)
                .ToListAsync().ConfigureAwait(false);

            var model = persistedGrants.Select(x => x.ToModel());

            return model;
        }

        public async Task RemoveAsync(string key)
        {
            var persistedGrant = await _context.PersistedGrants.FirstOrDefaultAsync(x => x.SiteId == _siteId && x.Key == key)
                .ConfigureAwait(false);
            if (persistedGrant!= null)
            {
                _context.PersistedGrants.Remove(persistedGrant);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            } 
        }

        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            var persistedGrants = await _context.PersistedGrants
                .Where(x => x.SiteId == _siteId && x.SubjectId == subjectId && x.ClientId == clientId)
                .ToListAsync();

            _context.PersistedGrants.RemoveRange(persistedGrants);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var persistedGrants = await _context.PersistedGrants.Where(x =>
                x.SiteId == _siteId &&
                x.SubjectId == subjectId &&
                x.ClientId == clientId &&
                x.Type == type).ToListAsync().ConfigureAwait(false);

            _context.PersistedGrants.RemoveRange(persistedGrants);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}