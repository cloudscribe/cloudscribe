// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Core.Models;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.IdentityServer.EFCore.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace cloudscribe.Core.IdentityServer.EFCore.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IConfigurationDbContext context;
        private IHttpContextAccessor _contextAccessor;
        private readonly ILogger<ClientStore> _logger;

        public ClientStore(
            IHttpContextAccessor contextAccessor,
            IConfigurationDbContext context,
            ILogger<ClientStore> logger
            )
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _contextAccessor = contextAccessor;
            this.context = context;
            _logger = logger;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null) return null;
            var _siteId = site.Id.ToString();

            var client = await context.Clients
                .AsNoTracking()
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .FirstOrDefaultAsync(x => x.SiteId == _siteId && x.ClientId == clientId);

            var model = client?.ToModel();

            _logger.LogDebug("{clientId} found in database: {clientIdFound}", clientId, model != null);

            return model;
        }
    }
}