// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-10-12
// Last Modified:           2016-10-12
// 

using cloudscribe.Core.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using NoDb;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class ClientStore : IClientStore
    {
        public ClientStore(
            SiteContext site,
            IBasicQueries<Client> queries
            )
        {
            _siteId = site.Id.ToString();
            _queries = queries;
        }

        private IBasicQueries<Client> _queries;
        private string _siteId;

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await _queries.FetchAsync(
                _siteId, // aka nodb projectid
                clientId 
                ).ConfigureAwait(false);
        }
    }
}
