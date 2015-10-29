// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-28
// Last Modified:		    2015-10-17
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Facebook;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantFacebookOptionsResolver
    {
        public MultiTenantFacebookOptionsResolver(
            FacebookOptions originalOptions,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            MultiTenantOptions multiTenantOptions)
        {
            this.originalOptions = originalOptions;
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            siteRepo = siteRepository;
        }

        private FacebookOptions originalOptions;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;
        private ISiteSettings site = null;
        public ISiteSettings Site
        {
            get
            {
                if(site == null)
                {
                    if(multiTenantOptions.UseRelatedSitesMode)
                    {
                        if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
                        {
                            site = siteRepo.FetchNonAsync(multiTenantOptions.RelatedSiteId);
                        }
                    }

                    site = siteResolver.Resolve();
                }

                return site;
            }
        }

        public string AppId
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.FacebookAppId.Length > 0) && (Site.FacebookAppSecret.Length > 0))
                    {
                        return Site.FacebookAppId;
                    }
                }

                return originalOptions.AppId;
            }
        }

        public string AppSecret
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.FacebookAppId.Length > 0) && (Site.FacebookAppSecret.Length > 0))
                    {
                        return Site.FacebookAppSecret;
                    }
                }

                return originalOptions.AppSecret;
            }
        }

        public string ResolveRedirectUrl(string redirectUrl)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if ((Site != null)&&(Site.SiteFolderName.Length > 0))
                {
                    if ((Site.FacebookAppId.Length > 0) && (Site.FacebookAppSecret.Length > 0))
                    {
                        return redirectUrl.Replace("signin-facebook", site.SiteFolderName + "/signin-facebook");
                    }
                }
            }
            

            return redirectUrl;
        }

    }
}
