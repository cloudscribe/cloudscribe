//using cloudscribe.Core.Models;
//using IdentityServer4.Endpoints;

//using Microsoft.Extensions.Options;

//namespace cloudscribe.Core.IdentityServerIntegration
//{
//    public class MultiTenantAuthorizeProtocolRouteMatcher : IMatchAuthorizeProtocolRoutePaths
//    {
//        public MultiTenantAuthorizeProtocolRouteMatcher(
//            SiteContext currentSite,
//            IOptions<MultiTenantOptions> multiTenantOptionsAccessor
//            )
//        {
//            multiTenantOptions = multiTenantOptionsAccessor.Value;
//            site = currentSite;
//        }

//        private MultiTenantOptions multiTenantOptions;
//        private SiteContext site;

//        public bool IsAuthorizePath(string requestPath)
//        {
//            if (requestPath == CustomConstants.ProtocolRoutePaths.Authorize.EnsureLeadingSlash())
//                return true;

//            if(multiTenantOptions.Mode == MultiTenantMode.FolderName && !multiTenantOptions.UseRelatedSitesMode)
//            {
//                if(!string.IsNullOrEmpty(site.SiteFolderName))
//                {
//                    if (requestPath == "/" + site.SiteFolderName + CustomConstants.ProtocolRoutePaths.Authorize.EnsureLeadingSlash())
//                        return true;
//                }
//            }

//            return false;
//        }

//        public bool IsAuthorizeCallbackPath(string requestPath)
//        {
//            if (requestPath == CustomConstants.ProtocolRoutePaths.AuthorizeCallback.EnsureLeadingSlash())
//                return true;

//            if (multiTenantOptions.Mode == MultiTenantMode.FolderName && !multiTenantOptions.UseRelatedSitesMode)
//            {
//                if (!string.IsNullOrEmpty(site.SiteFolderName))
//                {
//                    if (requestPath == "/" + site.SiteFolderName + CustomConstants.ProtocolRoutePaths.AuthorizeCallback.EnsureLeadingSlash())
//                        return true;
//                }
//            }

//            return false;
//        }

//        //public bool IsAuthorizeAfterConsentPath(string requestPath)
//        //{
//        //    if (requestPath == CustomConstants.ProtocolRoutePaths.AuthorizeAfterConsent.EnsureLeadingSlash())
//        //        return true;

//        //    if (multiTenantOptions.Mode == MultiTenantMode.FolderName && !multiTenantOptions.UseRelatedSitesMode)
//        //    {
//        //        if (!string.IsNullOrEmpty(site.SiteFolderName))
//        //        {
//        //            if (requestPath == "/" + site.SiteFolderName + CustomConstants.ProtocolRoutePaths.AuthorizeAfterConsent.EnsureLeadingSlash())
//        //                return true;
//        //        }
//        //    }

//        //    return false;
//        //}

//    }
//}
