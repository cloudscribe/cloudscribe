//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:				    2014-08-28
//// Last Modified:		    2016-02-05
//// 


//using cloudscribe.Core.Models;
//using Microsoft.AspNet.Authentication.Facebook;
//using Microsoft.AspNet.Http;
//using SaasKit.Multitenancy;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Identity.OAuth
//{
//    public class MultiTenantFacebookOptionsResolver
//    {
//        public MultiTenantFacebookOptionsResolver(
//            FacebookOptions originalOptions,
//            //ISiteResolver siteResolver,
//            //IHttpContextAccessor contextAccessor,
//            //ITenantResolver<SiteSettings> siteResolver,
//            ISiteSettings currentSite,
//            //ISiteRepository siteRepository,
//            MultiTenantOptions multiTenantOptions)
//        {
//            this.originalOptions = originalOptions;
//            //this.siteResolver = siteResolver;
//            //this.contextAccessor = contextAccessor;
//            this.multiTenantOptions = multiTenantOptions;
//            //siteRepo = siteRepository;
//            site = currentSite;
//        }

//        private FacebookOptions originalOptions;
//        private IHttpContextAccessor contextAccessor;
//        private ITenantResolver<SiteSettings> siteResolver;
//        private ISiteRepository siteRepo;
//        private MultiTenantOptions multiTenantOptions;
//        private ISiteSettings site = null;
//        //public ISiteSettings Site
//        //{
//        //    get
//        //    {
//        //        if(site == null)
//        //        {
//        //            if(multiTenantOptions.UseRelatedSitesMode)
//        //            {
//        //                if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
//        //                {
//        //                    site = siteRepo.FetchNonAsync(multiTenantOptions.RelatedSiteId);
//        //                }
//        //            }

//        //            site = siteResolver.Resolve();
//        //        }

//        //        return site;
//        //    }
//        //}

//        //private async Task<SiteSettings> GetSite()
//        //{
//        //    TenantContext<SiteSettings> tenantContext
//        //        = await siteResolver.ResolveAsync(contextAccessor.HttpContext);

//        //    if (tenantContext != null && tenantContext.Tenant != null)
//        //    {
//        //        return tenantContext.Tenant;
//        //    }

//        //    return null;
//        //}

//        public string AppId
//        {
//            get
//            {
//                if (site != null)
//                {
//                    if ((site.FacebookAppId.Length > 0) && (site.FacebookAppSecret.Length > 0))
//                    {
//                        return site.FacebookAppId;
//                    }
//                }

//                return originalOptions.AppId;
//            }
//        }

//        public string AppSecret
//        {
//            get
//            {
//                if (site != null)
//                {
//                    if ((site.FacebookAppId.Length > 0) && (site.FacebookAppSecret.Length > 0))
//                    {
//                        return site.FacebookAppSecret;
//                    }
//                }

//                return originalOptions.AppSecret;
//            }
//        }

//        public string ResolveRedirectUrl(string redirectUrl)
//        {
//            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if ((site != null)&&(site.SiteFolderName.Length > 0))
//                {
//                    if ((site.FacebookAppId.Length > 0) && (site.FacebookAppSecret.Length > 0))
//                    {
//                        return redirectUrl.Replace("signin-facebook", site.SiteFolderName + "/signin-facebook");
//                    }
//                }
//            }
            

//            return redirectUrl;
//        }

//    }
//}
