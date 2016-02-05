// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-09-04
// Last Modified:		    2016-02-05
//

using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using SaasKit.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantOAuthOptionsResolver
    {
        public MultiTenantOAuthOptionsResolver(
            //ISiteResolver siteResolver,
            IHttpContextAccessor contextAccessor,
            ITenantResolver<SiteSettings> siteResolver,
            MultiTenantOptions multiTenantOptions)
        {
            this.siteResolver = siteResolver;
            this.contextAccessor = contextAccessor;
            //site = currentSite;
            this.multiTenantOptions = multiTenantOptions;
        }

        private IHttpContextAccessor contextAccessor;
        private ITenantResolver<SiteSettings> siteResolver;
        private MultiTenantOptions multiTenantOptions;

        private async Task<SiteSettings> GetSite()
        {
            TenantContext<SiteSettings> tenantContext 
                = await siteResolver.ResolveAsync(contextAccessor.HttpContext);

            if(tenantContext != null && tenantContext.Tenant != null)
            {
                return tenantContext.Tenant;
            }

            return null;
        }

        //private ISiteSettings site = null;
        //public ISiteSettings Site
        //{
        //    get
        //    {
        //        //if (site == null) {  site = siteResolver.Resolve(); }
        //        return site;
        //    }
        //}

        public async Task<string> ResolveCorrelationKey(string originalCorrelationKey)
        {
            if(!multiTenantOptions.UseRelatedSitesMode)
            {
                if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    var site = await GetSite();
                    if ((site != null)&&(site.SiteFolderName.Length > 0))
                    {
                        return originalCorrelationKey + "-" + site.SiteFolderName;
                    }
                    
                }
            }

            return originalCorrelationKey;
        }
    }
}
