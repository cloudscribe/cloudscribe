// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2016-12-05
// Last Modified:			2016-12-05
// 

using IdentityServer4.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Storage
{
    public interface IIdentityResourceCommands
    {
        Task CreateIdentityResource(string siteId, IdentityResource identityResource, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateIdentityResource(string siteId, IdentityResource identityResource, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteIdentityResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}
