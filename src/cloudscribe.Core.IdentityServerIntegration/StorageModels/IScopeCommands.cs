// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-10-13
// 

using IdentityServer4.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.StorageModels
{
    public interface IScopeCommands
    {
        Task CreateScope(Scope scope, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateScope(Scope scope, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteScope(Scope scope, CancellationToken cancellationToken = default(CancellationToken));
    }
}
