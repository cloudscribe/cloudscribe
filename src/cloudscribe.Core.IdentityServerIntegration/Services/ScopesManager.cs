//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0
//// Author:					Joe Audette
//// Created:					2016-10-13
//// Last Modified:			2016-10-14
//// 

//using cloudscribe.Core.IdentityServerIntegration.Storage;
//using cloudscribe.Core.Models.Generic;
//using IdentityServer4.Models;
//using Microsoft.AspNetCore.Http;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.IdentityServerIntegration.Services
//{
//    public class ScopesManager
//    {
//        public ScopesManager(
//            IScopeCommands commands,
//            IScopeQueries queries,
//            IHttpContextAccessor contextAccessor
//            )
//        {
//            _commands = commands;
//            _queries = queries;
//            _context = contextAccessor?.HttpContext;
//        }

//        private IScopeCommands _commands;
//        private IScopeQueries _queries;
//        private readonly HttpContext _context;
//        private CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

//        public async Task<PagedResult<Scope>> GetScopes(
//            string siteId,
//            int pageNumber,
//            int pageSize)
//        {
//            return await _queries.GetScopes(siteId, pageNumber, pageSize, CancellationToken).ConfigureAwait(false);
//        }

//        public async Task<Scope> FetchScope(string siteId, string scopeName)
//        {
//            return await _queries.FetchScope(siteId, scopeName, CancellationToken).ConfigureAwait(false);
//        }

//        public async Task<bool> ScopeExists(string siteId, string scopeName)
//        {
//            return await _queries.ScopeExists(siteId, scopeName, CancellationToken).ConfigureAwait(false);
//        }

//        public async Task CreateScope(string siteId, Scope scope)
//        {
//            await _commands.CreateScope(siteId, scope, CancellationToken.None).ConfigureAwait(false);
//        }

//        public async Task UpdateScope(string siteId, Scope scope)
//        {
//            await _commands.UpdateScope(siteId, scope, CancellationToken.None).ConfigureAwait(false);
//        }

//        public async Task DeleteScope(string siteId, string scopeName)
//        {
//            await _commands.DeleteScope(siteId, scopeName, CancellationToken.None).ConfigureAwait(false);
//        }

//        public ScopeType? ResolveScopeType(string scopeTypeName)
//        {
//            switch(scopeTypeName)
//            {
                
//                case "Identity":
//                    return ScopeType.Identity;
//                case "Resource":
//                    return ScopeType.Resource;
                
//            }

//            return null;
//        }


//    }
//}
