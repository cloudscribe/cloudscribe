// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:              Joe Audette
// Created:             2016-02-04
// Last Modified:       2018-03-07
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SaasKit.Multitenancy;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteResolver : ITenantResolver<SiteContext>
    {
        public SiteResolver(
            ISiteQueries siteRepository,
            SiteDataProtector dataProtector,
            IOptions<MultiTenantOptions> multiTenantOptions)
        {
            _siteQueries = siteRepository;
            _multiTenantOptions = multiTenantOptions.Value;
            _dataProtector = dataProtector;
        }

        private MultiTenantOptions _multiTenantOptions;
        private ISiteQueries _siteQueries;
        private SiteDataProtector _dataProtector;

        public Task<TenantContext<SiteContext>> ResolveAsync(HttpContext context)
        {
            if(_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                return ResolveByFolderAsync(context);
            }

            return ResolveByHostAsync(context);
        }

        private async Task<TenantContext<SiteContext>> ResolveByFolderAsync(HttpContext context)
        {
            var siteFolderName = context.Request.Path.StartingSegment();
            if (siteFolderName.Length == 0) { siteFolderName = "root"; }

            TenantContext<SiteContext> tenantContext = null;

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;

            var site  = await _siteQueries.FetchByFolderName(siteFolderName, cancellationToken);

            if (site != null)
            {
                _dataProtector.UnProtect(site);
                var siteContext = new SiteContext(site);
                tenantContext = new TenantContext<SiteContext>(siteContext);
            }

            return tenantContext;

            
        }

        private async Task<TenantContext<SiteContext>> ResolveByHostAsync(HttpContext context)
        {
            TenantContext<SiteContext> tenantContext = null;

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;

            var site = await _siteQueries.Fetch(context.Request.Host.Value, cancellationToken);

            if (site != null)
            {
                _dataProtector.UnProtect(site);

                var siteContext = new SiteContext(site);

                tenantContext = new TenantContext<SiteContext>(siteContext);
            }

            return tenantContext;
        }

    }
}
