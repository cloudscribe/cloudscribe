// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-09-01
// Last Modified:		    2015-09-01
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.MicrosoftAccount;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantMicrosoftOptionsResolver
    {

        public MultiTenantMicrosoftOptionsResolver(
            MicrosoftAccountAuthenticationOptions originalOptions,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            MultiTenantOptions multiTenantOptions)
        {
            this.originalOptions = originalOptions;
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            siteRepo = siteRepository;
        }

        private MicrosoftAccountAuthenticationOptions originalOptions;
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
                    if ((Site.MicrosoftClientId.Length > 0) && (Site.MicrosoftClientSecret.Length > 0))
                    {
                        return Site.MicrosoftClientId;
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
                    if ((Site.MicrosoftClientId.Length > 0) && (Site.MicrosoftClientSecret.Length > 0))
                    {
                        return Site.MicrosoftClientSecret;
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
                    if ((Site.MicrosoftClientId.Length > 0) && (Site.MicrosoftClientSecret.Length > 0))
                    {
                        return redirectUrl.Replace("signin-microsoft", site.SiteFolderName + "/signin-microsoft");
                    }
                }
            }


            return redirectUrl;
        }

    }
}
