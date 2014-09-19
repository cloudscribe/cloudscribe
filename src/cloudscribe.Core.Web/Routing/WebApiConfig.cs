using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using log4net;

namespace cloudscribe.Web.Routing
{
    public class WebApiConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WebApiConfig));

        public static void Register(HttpConfiguration config)
        {
            
            // enable attribute routing
            config.MapHttpAttributeRoutes();

            try
            {
                RoutesConfig registrarConfig = RoutesConfig.GetConfig();

                foreach (IRegisterRoutes registrar in registrarConfig.RouteRegistrars)
                {
                    try
                    {
                        registrar.Register(config);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    
                }

            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}