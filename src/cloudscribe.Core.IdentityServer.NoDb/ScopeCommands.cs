//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0
//// Author:					Joe Audette
//// Created:					2016-10-14
//// Last Modified:			2016-10-14
//// 

//using cloudscribe.Core.IdentityServerIntegration.Storage;
//using IdentityServer4.Models;
//using NoDb;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.IdentityServer.NoDb
//{
//    public class ScopeCommands : IScopeCommands
//    {
//        public ScopeCommands(
//            IBasicCommands<Scope> commands
//            )
//        {
//            _commands = commands;
//        }

//        private IBasicCommands<Scope> _commands;

//        public async Task CreateScope(string siteId, Scope scope, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            await _commands.CreateAsync(siteId, scope.Name, scope, cancellationToken).ConfigureAwait(false);
//        }

//        public async Task UpdateScope(string siteId, Scope scope, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            await _commands.UpdateAsync(siteId, scope.Name, scope, cancellationToken).ConfigureAwait(false);
//        }

//        public async Task DeleteScope(string siteId, string scopeName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            await _commands.DeleteAsync(siteId, scopeName, cancellationToken).ConfigureAwait(false);
//        }
//    }
//}
