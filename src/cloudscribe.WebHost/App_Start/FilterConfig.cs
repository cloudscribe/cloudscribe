using System.Web;
using System.Web.Mvc;
using cloudscribe.Web.Routing;

namespace cloudscribe.WebHost
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // call the cloudscribe RouteRegister
            // which in turn calls feature sepcific
            // route registrars that implement IRegisterRoutes
            // and configured under /Config/RouteRegistrars
            RouteRegistrar.RegisterGlobalFilters(filters);

            filters.Add(new cloudscribe.Core.Web.Filters.SiteAuthFilter());
            filters.Add(new cloudscribe.Core.Web.Filters.SiteContextFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
