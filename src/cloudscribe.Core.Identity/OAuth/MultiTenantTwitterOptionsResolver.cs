// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2014-09-09
// Last Modified:           2016-02-05
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication.Twitter;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantTwitterOptionsResolver
    {
        public MultiTenantTwitterOptionsResolver(
            TwitterOptions originalOptions,
            ISiteSettings currentSite,
            MultiTenantOptions multiTenantOptions)
        {
            this.originalOptions = originalOptions;
            //this.siteResolver = siteResolver;
            site = currentSite;
            this.multiTenantOptions = multiTenantOptions;
            //siteRepo = siteRepository;
        }

        private TwitterOptions originalOptions;
        //private ISiteResolver siteResolver;
        //private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;
        private ISiteSettings site = null;
        //public ISiteSettings Site
        //{
        //    get
        //    {
        //        if (site == null)
        //        {
        //            if (multiTenantOptions.UseRelatedSitesMode)
        //            {
        //                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
        //                {
        //                    site = siteRepo.FetchNonAsync(multiTenantOptions.RelatedSiteId);
        //                }
        //            }

        //            site = siteResolver.Resolve();
        //        }

        //        return site;
        //    }
        //}

        public string ConsumerKey
        {
            get
            {
                if (site != null)
                {
                    if ((site.TwitterConsumerKey.Length > 0) && (site.TwitterConsumerSecret.Length > 0))
                    {
                        return site.TwitterConsumerKey;
                    }
                }

                return originalOptions.ConsumerKey;
            }
        }

        public string ConsumerSecret
        {
            get
            {
                if (site != null)
                {
                    if ((site.TwitterConsumerKey.Length > 0) && (site.TwitterConsumerSecret.Length > 0))
                    {
                        return site.TwitterConsumerSecret;
                    }
                }

                return originalOptions.ConsumerSecret;
            }
        }

        public string ResolveRedirectUrl(string redirectUrl)
        {
            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if ((site != null) && (site.SiteFolderName.Length > 0))
                {
                    if ((site.TwitterConsumerKey.Length > 0) && (site.TwitterConsumerSecret.Length > 0))
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
                if ((site != null) && (site.SiteFolderName.Length > 0))
                {
                    if ((site.TwitterConsumerKey.Length > 0) && (site.TwitterConsumerSecret.Length > 0))
                    {
                        return site.SiteFolderName + providedCookieName;
                    }
                }
            }


            return providedCookieName;
        }
    }
}
