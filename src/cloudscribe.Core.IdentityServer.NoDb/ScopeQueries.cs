//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0
//// Author:					Joe Audette
//// Created:					2016-10-14
//// Last Modified:			2016-10-17
//// 

//using cloudscribe.Core.Models.Generic;
//using cloudscribe.Core.IdentityServerIntegration.Storage;
//using IdentityServer4.Models;
//using NoDb;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.IdentityServer.NoDb
//{
//    public class ScopeQueries : IScopeQueries
//    {
//        public ScopeQueries(
//            IBasicQueries<Scope> queries
//            )
//        {
//            _queries = queries;
//        }

//        private IBasicQueries<Scope> _queries;

//        private async Task<List<Scope>> GetAllScopes(string siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            //TODO: cache
//            var allScopes = await _queries.GetAllAsync(siteId).ConfigureAwait(false);

//            return allScopes.ToList();
//        }

//        public async Task<bool> ScopeExists(string siteId, string scopeName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            var scope = await FetchScope(siteId, scopeName, cancellationToken).ConfigureAwait(false);
//            return (scope != null);
//        }

//        public async Task<Scope> FetchScope(string siteId, string scopeName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            return await _queries.FetchAsync(siteId, scopeName, cancellationToken).ConfigureAwait(false);
//        }

//        public async Task<int> CountScopes(string siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            var all = await GetAllScopes(siteId, cancellationToken).ConfigureAwait(false);
//            return all.Count;
//        }

//        public async Task<PagedResult<Scope>> GetScopes(
//            string siteId,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            var all = await GetAllScopes(siteId, cancellationToken).ConfigureAwait(false);

//            int offset = (pageSize * pageNumber) - pageSize;

//            var result = new PagedResult<Scope>();
//            result.TotalItems = all.Count;
//            result.Data = all
//                .OrderBy(x => x.Type).ThenBy(x => x.Name)
//                .Skip(offset)
//                .Take(pageSize).ToList();

//            return result;

//        }
//    }
//}
