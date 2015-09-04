// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-09-04
// Last Modified:		    2015-09-04
//

using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantOAuthOptionsResolver
    {
        public MultiTenantOAuthOptionsResolver(
            ISiteResolver siteResolver,
            MultiTenantOptions multiTenantOptions)
        {
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
        }

        private ISiteResolver siteResolver;
        private MultiTenantOptions multiTenantOptions;

        private ISiteSettings site = null;
        public ISiteSettings Site
        {
            get
            {
                if (site == null) {  site = siteResolver.Resolve(); }
                return site;
            }
        }

        public string ResolveCorrelationKey(string originalCorrelationKey)
        {
            if(!multiTenantOptions.UseRelatedSitesMode)
            {
                if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
                {
                    if((Site != null)&&(Site.SiteFolderName.Length > 0))
                    {
                        return originalCorrelationKey + "-" + Site.SiteFolderName;
                    }
                    
                }
            }

            return originalCorrelationKey;
        }
    }
}
