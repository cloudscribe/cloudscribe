//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2015-08-01
//// Last Modified:			2016-02-04
//// 

//using cloudscribe.Core.Models;
//using Microsoft.Extensions.OptionsModel;
//using Microsoft.Extensions.Logging;
//using System;

//namespace cloudscribe.Core.Identity
//{
//    public class MultiTenantCookieOptionsResolver
//    {
//        public MultiTenantCookieOptionsResolver(
//            //ISiteResolver siteResolver,
//            SiteSettings currentSite,
//            IOptions<MultiTenantOptions> multiTenantOptions,
//            ILoggerFactory loggerFactory)
//        {
//            site = currentSite;
//            //this.siteResolver = siteResolver;
//            this.multiTenantOptions = multiTenantOptions.Value;
//            log = loggerFactory.CreateLogger<MultiTenantCookieOptionsResolver>();
//        }

//        private ILogger log;
//        private MultiTenantOptions multiTenantOptions;
//        //private ISiteResolver siteResolver;
//        private ISiteSettings site = null;
//        private ISiteSettings Site
//        {
//            get
//            {
//                //if(site == null)
//                //{
//                //    try
//                //    {
//                //        site = siteResolver.Resolve();
//                //    }
//                //    catch(Exception ex)
//                //    {
//                //        log.LogError("failed to resolve site", ex);
//                //    }
                    
//                //}
//                return site;
//            }
//        }
        
//        public string ResolveCookieName(string suppliedCookieName)
//        {
//            if(multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if(!multiTenantOptions.UseRelatedSitesMode)
//                {
//                    if((Site != null)&&(Site.SiteFolderName.Length > 0))
//                    {
//                        return suppliedCookieName + "-" + Site.SiteFolderName;
//                    }
//                }
//            }

//            return suppliedCookieName;
//        }

//        public string ResolveAuthScheme(string suppliedAuthScheme)
//        {
//            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if (!multiTenantOptions.UseRelatedSitesMode)
//                {
//                    if ((Site != null) && (Site.SiteFolderName.Length > 0))
//                    {
//                        return suppliedAuthScheme + "-" + Site.SiteFolderName;
//                    }

//                }
//            }

//            return suppliedAuthScheme;
//        }

//        public string ResolveLoginPath(string suppliedLoginPath)
//        {
//            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if (!multiTenantOptions.UseRelatedSitesMode)
//                {
//                    if ((Site != null) && (Site.SiteFolderName.Length > 0))
//                    {
//                        return "/" + Site.SiteFolderName + suppliedLoginPath;
//                    }
//                }
//            }

//            return suppliedLoginPath;
//        }

//        public string ResolveLogoutPath(string suppliedLogoutPath)
//        {
//            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if (!multiTenantOptions.UseRelatedSitesMode)
//                {
//                    if ((Site != null) && (Site.SiteFolderName.Length > 0))
//                    {
//                        return "/" + Site.SiteFolderName + suppliedLogoutPath;
//                    }
//                }
//            }

//            return suppliedLogoutPath;
//        }

//        public string ResolveReturnUrlParameter(string suppliedReturnUrlParameter)
//        {
//            if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                if (!multiTenantOptions.UseRelatedSitesMode)
//                {
//                    if ((Site != null) && (Site.SiteFolderName.Length > 0))
//                    {
//                        //TODO: is this right? what does return url param look like?
//                        //return "/" + Site.SiteFolderName + suppliedReturnUrlParameter;
//                    }
//                }
//            }

//            return suppliedReturnUrlParameter;
//        }
  

//    }
//}
