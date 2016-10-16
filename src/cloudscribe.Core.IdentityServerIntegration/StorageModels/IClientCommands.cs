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
    public interface IClientCommands
    {
        Task CreateClient(Client client, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateClient(Client client, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteClient(Client client, CancellationToken cancellationToken = default(CancellationToken));
    }
}
