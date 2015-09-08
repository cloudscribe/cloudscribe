// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-09-08
// Last Modified:		    2015-09-08
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Google;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantGoogleOptionsResolver
    {
        public MultiTenantGoogleOptionsResolver(
            GoogleAuthenticationOptions originalOptions,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            MultiTenantOptions multiTenantOptions)
        {
            this.originalOptions = originalOptions;
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            siteRepo = siteRepository;
        }

        private GoogleAuthenticationOptions originalOptions;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;
        private ISiteSettings site = null;
        public ISiteSettings Site
        {
            get
            {
                if (site == null)
                {
                    if (multiTenantOptions.UseRelatedSitesMode)
                    {
                        if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
                        {
                            site = siteRepo.FetchNonAsync(multiTenantOptions.RelatedSiteId);
                        }
                    }

                    site = siteResolver.Resolve();
                }

                return site;
            }
        }

        public string ClientId
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.GoogleClientId.Length > 0) && (Site.GoogleClientSecret.Length > 0))
                    {
                        return Site.GoogleClientId;
                    }
                }

                return originalOptions.ClientId;
            }
        }

        public string ClientSecret
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.GoogleClientId.Length > 0) && (Site.GoogleClientSecret.Length > 0))
                    {
                        return Site.GoogleClientSecret;
                    }
                }

                return originalOptions.ClientSecret;
            }
        }

        public string ResolveRedirectUrl(string redirectUrl)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if ((Site != null) && (Site.SiteFolderName.Length > 0))
                {
                    if ((Site.GoogleClientId.Length > 0) && (Site.GoogleClientSecret.Length > 0))
                    {
                        return redirectUrl.Replace("signin-google", site.SiteFolderName + "/signin-google");
                    }
                }
            }


            return redirectUrl;
        }

    }
}
