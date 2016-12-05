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
    public interface IApiResourceCommands
    {
        Task CreateApiResource(string siteId, ApiResource apiResource, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateApiResource(string siteId, ApiResource apiResource, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteApiResource(string siteId, string name, CancellationToken cancellationToken = default(CancellationToken));
    }
}
