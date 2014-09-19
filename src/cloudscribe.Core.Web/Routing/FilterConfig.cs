using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace cloudscribe.Web.Routing
{
    public class FilterConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FilterConfig));

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            

            

            try
            {
                RoutesConfig registrarConfig = RoutesConfig.GetConfig();

                foreach (IRegisterRoutes registrar in registrarConfig.RouteRegistrars)
                {
                    try
                    {
                        registrar.RegisterGlobalFilters(filters);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }
    }
}