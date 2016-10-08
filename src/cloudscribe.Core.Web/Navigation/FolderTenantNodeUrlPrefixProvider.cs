// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-08-02
// Last Modified:			2016-10-08
// 

using cloudscribe.Core.Models;
using cloudscribe.Web.Navigation;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Web.Navigation
{
    public class FolderTenantNodeUrlPrefixProvider : INodeUrlPrefixProvider
    {
        public FolderTenantNodeUrlPrefixProvider(
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptions)
        {
            site = currentSite;
            options = multiTenantOptions.Value;
        }

        private MultiTenantOptions options;
        private ISiteContext site;

        public string GetPrefix()
        {
            if(options.Mode == MultiTenantMode.FolderName)
            {
                if((site != null)&&(site.SiteFolderName.Length > 0))
                {
                    return site.SiteFolderName;
                }
            }

            return string.Empty;
        }

    }
}
