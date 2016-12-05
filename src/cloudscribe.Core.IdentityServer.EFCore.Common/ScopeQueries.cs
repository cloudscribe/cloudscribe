//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0
//// Author:					Joe Audette
//// Created:					2016-10-16
//// Last Modified:			2016-10-21
//// 

//using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
//using cloudscribe.Core.IdentityServer.EFCore.Mappers;
//using cloudscribe.Core.IdentityServerIntegration.Storage;
//using cloudscribe.Core.Models.Generic;
//using IdentityServer4.Models;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.IdentityServer.EFCore
//{
//    public class ScopeQueries : IScopeQueries
//    {
//        public ScopeQueries(
//            IConfigurationDbContext context
//            )
//        {
//            if (context == null) throw new ArgumentNullException(nameof(context));
//            this.context = context;
//        }

//        private readonly IConfigurationDbContext context;

//        public async Task<bool> ScopeExists(string siteId, string scopeName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            var scope = await FetchScope(siteId, scopeName, cancellationToken).ConfigureAwait(false);
//            return (scope != null);
//        }

//        public async Task<Scope> FetchScope(string siteId, string scopeName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            IQueryable<Entities.Scope> query = context.Scopes
//                .AsNoTracking()
//                .Include(x => x.Claims)
//                .Include(x => x.ScopeSecrets);
            
//            query = query.Where(x => x.SiteId == siteId && x.Name == scopeName);
//            var ent =  await query.SingleOrDefaultAsync();

//            return ent.ToModel();

//        }

//        public async Task<int> CountScopes(string siteId, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            return await context.Scopes
//                .Where(x => x.SiteId == siteId)
//                .CountAsync();
//        }

//        public async Task<PagedResult<Scope>> GetScopes(
//            string siteId,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken))
//        {
          
//            int offset = (pageSize * pageNumber) - pageSize;

//            var list = await context.Scopes
//                .AsNoTracking()
//                .Where(x => x.SiteId == siteId)
//                .OrderBy(x => x.Type).ThenBy(x => x.Name)
//                .Skip(offset)
//                .Take(pageSize).ToListAsync();

//            var result = new PagedResult<Scope>();
//            result.TotalItems = await CountScopes(siteId, cancellationToken).ConfigureAwait(false);
//            var model = list.Select(x => x.ToModel());
//            result.Data = model.ToList();

//            return result;

//        }


//    }
//}
