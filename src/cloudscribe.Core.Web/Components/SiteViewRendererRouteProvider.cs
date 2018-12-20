//using cloudscribe.Core.Models;
//using cloudscribe.Web.Common.Razor;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.Extensions.Options;

//namespace cloudscribe.Core.Web.Components
//{
//    public class SiteViewRendererRouteProvider : IViewRendererRouteProvider
//    {
//        public SiteViewRendererRouteProvider(
//            IOptions<MultiTenantOptions> multiTenantOptionsAccessor
//            )
//        {
//            _tenantOptions = multiTenantOptionsAccessor.Value;
//        }

//        private readonly MultiTenantOptions _tenantOptions;

//        public void AddRoutes(RouteBuilder routes)
//        {
//            if(_tenantOptions.Mode == MultiTenantMode.FolderName)
//            {
//                routes.MapRoute(
//                    name: "folderdefault",
//                    template: "{sitefolder}/{controller}/{action}/{id?}",
//                    defaults: new { controller = "Home", action = "Index" },
//                    constraints: new { sitefolder = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() });
//            }

//            routes.MapRoute(
//                name: "default",
//                template: "{controller=Home}/{action=Index}/{id?}"
//            );
//        }

//    }
//}
