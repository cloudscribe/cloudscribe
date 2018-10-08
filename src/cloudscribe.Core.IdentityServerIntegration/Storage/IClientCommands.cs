// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2018-10-08
// 

using IdentityServer4.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Storage
{
    public interface IClientCommandsSingleton : IClientCommands
    {

    }

    public interface IClientCommands
    {
        Task CreateClient(string siteId, Client client, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateClient(string siteId, Client client, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteClient(string siteId, string clientId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
