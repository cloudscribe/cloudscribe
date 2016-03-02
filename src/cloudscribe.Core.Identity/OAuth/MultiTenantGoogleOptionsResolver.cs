//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:				    2014-09-08
//// Last Modified:		    2016-02-05
//// 


//using cloudscribe.Core.Models;
//using Microsoft.AspNet.Authentication.Google;


//namespace cloudscribe.Core.Identity.OAuth
//{
//    public class MultiTenantGoogleOptionsResolver
//    {
//        public MultiTenantGoogleOptionsResolver(
//            GoogleOptions originalOptions,
//            //ISiteResolver siteResolver,
//            //ISiteRepository siteRepository,
//            ISiteSettings currentSite,
//            MultiTenantOptions multiTenantOptions)
//        {
//            this.originalOptions = originalOptions;
//            //this.siteResolver = siteResolver;
//            this.multiTenantOptions = multiTenantOptions;
//            //siteRepo = siteRepository;
//            site = currentSite;
//        }

//        private GoogleOptions originalOptions;
//        //private ISiteResolver siteResolver;
//        //private ISiteRepository siteRepo;
//        private MultiTenantOptions multiTenantOptions;
//        private ISiteSettings site = null;
        

//        public string ClientId
//        {
//            get
//            {
//                if (site != null)
//                {
//                    if ((site.GoogleClientId.Length > 0) && (site.GoogleClientSecret.Length > 0))
//                    {
//                        return site.GoogleClientId;
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
//                    if ((site.GoogleClientId.Length > 0) && (site.GoogleClientSecret.Length > 0))
//                    {
//                        return site.GoogleClientSecret;
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
//                    if ((site.GoogleClientId.Length > 0) && (site.GoogleClientSecret.Length > 0))
//                    {
//                        return redirectUrl.Replace("signin-google", site.SiteFolderName + "/signin-google");
//                    }
//                }
//            }


//            return redirectUrl;
//        }

//    }
//}
