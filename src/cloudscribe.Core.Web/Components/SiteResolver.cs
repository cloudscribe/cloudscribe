// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:              Joe Audette
// Created:             2016-02-04
// Last Modified:       2018-04-26
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using cloudscribe.Multitenancy;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteResolver : ITenantResolver<SiteContext>
    {
        public SiteResolver(ISiteContextResolver siteContextResolver)
        {
            
            _siteContextResolver = siteContextResolver;
        }

        private ISiteContextResolver _siteContextResolver;
        
        public async Task<TenantContext<SiteContext>> ResolveAsync(HttpContext context)
        {
            TenantContext<SiteContext> tenantContext = null;
            CancellationToken cancellationToken = context?.RequestAborted ?? CancellationToken.None;
            var site = await _siteContextResolver.ResolveSite(context.Request.Host.Value, context.Request.Path, cancellationToken);

            if (site != null)
            {
                tenantContext = new TenantContext<SiteContext>(site);
            }

            return tenantContext;
            
        }
        
    }
}
