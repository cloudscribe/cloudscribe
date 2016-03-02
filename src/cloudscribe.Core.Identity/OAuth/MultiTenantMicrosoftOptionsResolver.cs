//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:				    2014-09-01
//// Last Modified:		    2016-02-05
//// 


//using cloudscribe.Core.Models;
//using Microsoft.AspNet.Authentication.MicrosoftAccount;

//namespace cloudscribe.Core.Identity.OAuth
//{
//    public class MultiTenantMicrosoftOptionsResolver
//    {

//        public MultiTenantMicrosoftOptionsResolver(
//            MicrosoftAccountOptions originalOptions,
//            //ISiteResolver siteResolver,
//            //ISiteRepository siteRepository,
//            ISiteSettings currentSite,
//            MultiTenantOptions multiTenantOptions)
//        {
//            this.originalOptions = originalOptions;
//            //this.siteResolver = siteResolver;
//            site = currentSite;
//            this.multiTenantOptions = multiTenantOptions;
//            //siteRepo = siteRepository;
//        }

//        private MicrosoftAccountOptions originalOptions;
//        //private ISiteResolver siteResolver;
//        //private ISiteRepository siteRepo;
//        private MultiTenantOptions multiTenantOptions;
//        private ISiteSettings site = null;

//        //private ISiteSettings site = null;
//        //public ISiteSettings Site
//        //{
//        //    get
//        //    {
//        //        if (site == null)
//        //        {
//        //            if (multiTenantOptions.UseRelatedSitesMode)
//        //            {
//        //                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//        //                {
//        //                    site = siteRepo.FetchNonAsync(multiTenantOptions.RelatedSiteId);
//        //                }
//        //            }

//        //            site = siteResolver.Resolve();
//        //        }

//        //        return site;
//        //    }
//        //}

//        public string ClientId
//        {
//            get
//            {
//                if (site != null)
//                {
//                    if ((site.MicrosoftClientId.Length > 0) && (site.MicrosoftClientSecret.Length > 0))
//                    {
//                        return site.MicrosoftClientId;
//                    }
//                }

//                return originalOptions.ClientId;
//            }
//        }

//        public string ClientSecret
//        {
//            get
//            {
//                if (site != null)
//                {
//                    if ((site.MicrosoftClientId.Length > 0) && (site.MicrosoftClientSecret.Length > 0))
//                    {
//                        return site.MicrosoftClientSecret;
//                    }
//                }

//                return originalOptions.ClientSecret;
//            }
//        }

//        public string ResolveRedirectUrl(string redirectUrl)
//        {
//            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if ((site != null) && (site.SiteFolderName.Length > 0))
//                {
//                    if ((site.MicrosoftClientId.Length > 0) && (site.MicrosoftClientSecret.Length > 0))
//                    {
//                        return redirectUrl.Replace("signin-microsoft", site.SiteFolderName + "/signin-microsoft");
//                    }
//                }
//            }


//            return redirectUrl;
//        }

//    }
//}
