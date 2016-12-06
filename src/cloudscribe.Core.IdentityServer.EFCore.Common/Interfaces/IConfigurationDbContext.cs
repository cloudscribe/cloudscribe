// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using cloudscribe.Core.IdentityServer.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;

namespace cloudscribe.Core.IdentityServer.EFCore.Interfaces
{
    public interface IConfigurationDbContext : IDisposable
    {
        DbSet<Client> Clients { get; set; }
        DbSet<IdentityResource> IdentityResources { get; set; }
        DbSet<ApiResource> ApiResources { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}