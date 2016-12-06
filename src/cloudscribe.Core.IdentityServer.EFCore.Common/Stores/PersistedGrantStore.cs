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
using Microsoft.AspNetCore.Http;

namespace cloudscribe.Core.IdentityServer.EFCore.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IPersistedGrantDbContext _context;
        private readonly ILogger _logger;
        private IHttpContextAccessor _contextAccessor;

        public PersistedGrantStore(
            IHttpContextAccessor contextAccessor,
            IPersistedGrantDbContext context, 
            ILogger<PersistedGrantStore> logger)
        {
            _contextAccessor = contextAccessor;
            _context = context;
            _logger = logger;
        }

        public async Task StoreAsync(PersistedGrant token)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }
            var _siteId = site.Id.ToString();

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
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return null;
            }
            var _siteId = site.Id.ToString();

            var persistedGrant = await _context.PersistedGrants.FirstOrDefaultAsync(x => x.SiteId == _siteId && x.Key == key)
                .ConfigureAwait(false);
            var model = persistedGrant?.ToModel();

            _logger.LogDebug("{persistedGrantKey} found in database: {persistedGrantKeyFound}", key, model != null);

            return model;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return new List<PersistedGrant>();
            }
            var _siteId = site.Id.ToString();

            var persistedGrants = await _context.PersistedGrants
                .Where(x => x.SiteId == _siteId && x.SubjectId == subjectId)
                .ToListAsync().ConfigureAwait(false);

            var model = persistedGrants.Select(x => x.ToModel());

            _logger.LogDebug("{persistedGrantCount} persisted grants found for {subjectId}", persistedGrants.Count, subjectId);


            return model;
        }

        public async Task RemoveAsync(string key)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }
            var _siteId = site.Id.ToString();

            var persistedGrant = await _context.PersistedGrants.FirstOrDefaultAsync(x => x.SiteId == _siteId && x.Key == key)
                .ConfigureAwait(false);
            if (persistedGrant!= null)
            {
                _logger.LogDebug("removing {persistedGrantKey} persisted grant from database", key);

                _context.PersistedGrants.Remove(persistedGrant);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            else
            {
                _logger.LogDebug("no {persistedGrantKey} persisted grant found in database", key);
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
            var _siteId = site.Id.ToString();

            var persistedGrants = await _context.PersistedGrants
                .Where(x => x.SiteId == _siteId && x.SubjectId == subjectId && x.ClientId == clientId)
                .ToListAsync();

            _logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}", persistedGrants.Count, subjectId, clientId);

            _context.PersistedGrants.RemoveRange(persistedGrants);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            
        }

        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }
            var _siteId = site.Id.ToString();

            var persistedGrants = await _context.PersistedGrants.Where(x =>
                x.SiteId == _siteId &&
                x.SubjectId == subjectId &&
                x.ClientId == clientId &&
                x.Type == type).ToListAsync().ConfigureAwait(false);

            _logger.LogDebug("removing {persistedGrantCount} persisted grants from database for subject {subjectId}, clientId {clientId}, grantType {persistedGrantType}", persistedGrants.Count, subjectId, clientId, type);

            _context.PersistedGrants.RemoveRange(persistedGrants);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}