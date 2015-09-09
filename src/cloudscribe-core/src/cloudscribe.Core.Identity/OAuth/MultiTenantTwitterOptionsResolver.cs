// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-09-09
// Last Modified:		    2015-09-09
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Twitter;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantTwitterOptionsResolver
    {
        public MultiTenantTwitterOptionsResolver(
            TwitterAuthenticationOptions originalOptions,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            MultiTenantOptions multiTenantOptions)
        {
            this.originalOptions = originalOptions;
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            siteRepo = siteRepository;
        }

        private TwitterAuthenticationOptions originalOptions;
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

        public string ConsumerKey
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.TwitterConsumerKey.Length > 0) && (Site.TwitterConsumerSecret.Length > 0))
                    {
                        return Site.TwitterConsumerKey;
                    }
                }

                return originalOptions.ConsumerKey;
            }
        }

        public string ConsumerSecret
        {
            get
            {
                if (Site != null)
                {
                    if ((Site.TwitterConsumerKey.Length > 0) && (Site.TwitterConsumerSecret.Length > 0))
                    {
                        return Site.TwitterConsumerSecret;
                    }
                }

                return originalOptions.ConsumerSecret;
            }
        }

        public string ResolveRedirectUrl(string redirectUrl)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if ((Site != null) && (Site.SiteFolderName.Length > 0))
                {
                    if ((Site.TwitterConsumerKey.Length > 0) && (Site.TwitterConsumerSecret.Length > 0))
                    {
                        return redirectUrl.Replace("signin-twitter", site.SiteFolderName + "/signin-twitter");
                    }
                }
            }


            return redirectUrl;
        }

        public string ResolveStateCookieName(string providedCookieName)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if ((Site != null) && (Site.SiteFolderName.Length > 0))
                {
                    if ((Site.TwitterConsumerKey.Length > 0) && (Site.TwitterConsumerSecret.Length > 0))
                    {
                        return site.SiteFolderName + providedCookieName;
                    }
                }
            }


            return providedCookieName;
        }
    }
}
