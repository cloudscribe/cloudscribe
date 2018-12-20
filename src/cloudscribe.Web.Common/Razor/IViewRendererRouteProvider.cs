using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common.Razor
{
    public interface IViewRendererRouteProvider
    {
        void AddRoutes(RouteBuilder routes);
    }

    public class DefaultViewRendererRouteProvider : IViewRendererRouteProvider
    {
        public void AddRoutes(RouteBuilder routes)
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}"
            );
        }
    }

}
