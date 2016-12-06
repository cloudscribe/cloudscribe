//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0
//// Author:					Joe Audette
//// Created:					2016-10-13
//// Last Modified:			2016-10-13
//// 

//using cloudscribe.Core.Models.Generic;
//using IdentityServer4.Models;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.IdentityServerIntegration.Storage
//{
//    public interface IScopeQueries
//    {
//        Task<bool> ScopeExists(string siteId, string scopeName, CancellationToken cancellationToken = default(CancellationToken));
//        Task<Scope> FetchScope(string siteId, string scopeName, CancellationToken cancellationToken = default(CancellationToken));
//        Task<int> CountScopes(string siteId, CancellationToken cancellationToken = default(CancellationToken));

//        Task<PagedResult<Scope>> GetScopes(
//            string siteId,
//            int pageNumber,
//            int pageSize,
//            CancellationToken cancellationToken = default(CancellationToken));
//    }
//}
