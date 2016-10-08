// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:              Joe Audette
// Created:             2016-02-04
// Last Modified:       2016-10-08
// 

//  2016-02-04 found this blog post by Ben Foster
//  http://benfoster.io/blog/asp-net-5-multitenancy
//  and the related project https://github.com/saaskit/saaskit
//  I like his approach better than mine though they are similar
//  his seems a little cleaner so I'm adopting it here to replace my previous pattern
//  actual resolution process is the same as before

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
            siteRepo = siteRepository;
            this.multiTenantOptions = multiTenantOptions.Value;
            this.dataProtector = dataProtector;
        }

        private MultiTenantOptions multiTenantOptions;
        private ISiteQueries siteRepo;
        private SiteDataProtector dataProtector;

        public Task<TenantContext<SiteContext>> ResolveAsync(HttpContext context)
        {
            if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
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

            var site  = await siteRepo.FetchByFolderName(siteFolderName, cancellationToken);

            if (site != null)
            {
                dataProtector.UnProtect(site);
                var siteContext = new SiteContext(site);
                tenantContext = new TenantContext<SiteContext>(siteContext);
            }

            return tenantContext;

            
        }

        private async Task<TenantContext<SiteContext>> ResolveByHostAsync(HttpContext context)
        {
            TenantContext<SiteContext> tenantContext = null;

            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;

            var site = await siteRepo.Fetch(context.Request.Host.Value, cancellationToken);

            if (site != null)
            {
                dataProtector.UnProtect(site);

                var siteContext = new SiteContext(site);

                tenantContext = new TenantContext<SiteContext>(siteContext);
            }

            return tenantContext;
        }

    }
}
