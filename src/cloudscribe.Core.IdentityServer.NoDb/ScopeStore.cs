// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-14
// 

using cloudscribe.Core.Models;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using NoDb;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ScopeStore : IScopeStore
    {
        public ScopeStore(
            SiteContext site,
            IBasicQueries<Scope> queries
            )
        {
            _siteId = site.Id.ToString();
            _queries = queries;
        }

        private IBasicQueries<Scope> _queries;
        private string _siteId;

        public async Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
           
            if (scopeNames != null && scopeNames.Any())
            {
                var allScopes = await GetAllScopes().ConfigureAwait(false);
                var scopes = allScopes.Where(x => scopeNames.Contains(x.Name));
                return scopes;
            }

            return new List<Scope>();
        }

        public async Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            var allScopes = await GetAllScopes().ConfigureAwait(false);

            if (publicOnly)
            {
                return allScopes.Where(x => x.ShowInDiscoveryDocument);
            }

            return allScopes;
        }

        private async Task<IEnumerable<Scope>> GetAllScopes()
        {
            //TODO: cache
            var allScopes = await _queries.GetAllAsync(_siteId).ConfigureAwait(false);

            return allScopes;
        }


    }
}
