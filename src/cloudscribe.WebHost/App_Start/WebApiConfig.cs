using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using cloudscribe.Web.Routing;

namespace cloudscribe.WebHost
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // call the cloudscribe RouteRegistrar
            // which in turn calls feature sepcific
            // route registrars that implement IRegisterRoutes
            // and configured under /Config/RouteRegistrars
            RouteRegistrar.RegisterApiRoutes(config);
            


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
