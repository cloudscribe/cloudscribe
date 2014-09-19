using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using cloudscribe.Configuration;

namespace cloudscribe.Web.Routing
{
    public class RoutesConfig
    {
        private List<IRegisterRoutes> routeRegistrars
            = new List<IRegisterRoutes>();

        public List<IRegisterRoutes> RouteRegistrars
        {
            get
            {
                return routeRegistrars;
            }
        }


        public static RoutesConfig GetConfig()
        {
            RoutesConfig config = new RoutesConfig();


            if (
                (HttpRuntime.Cache["RoutesConfig"] != null)
                && (HttpRuntime.Cache["RoutesConfig"] is RoutesConfig)
            )
            {
                return (RoutesConfig)HttpRuntime.Cache["RoutesConfig"];
            }


            String configFolderName = AppSettings.RouteConfigPath; //"~/Config/RouteRegistrars/";

            string pathToConfigFolder = System.Web.Hosting.HostingEnvironment.MapPath(configFolderName);

            if (!Directory.Exists(pathToConfigFolder)) return config;

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToConfigFolder);

            FileInfo[] routeFiles = directoryInfo.GetFiles("*.config");

            foreach (FileInfo fileInfo in routeFiles)
            {
                XmlDocument routeConfigFile = new XmlDocument();
                routeConfigFile.Load(fileInfo.FullName);
                LoadRoutes(config, routeConfigFile.DocumentElement);

            }


             //cache can be cleared by touching Web.config
            CacheDependency cacheDependency
                = new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config"));


            HttpRuntime.Cache.Insert(
                "RoutesConfig",
                config,
                cacheDependency,
                DateTime.Now.AddMinutes(5),
                TimeSpan.Zero,
                CacheItemPriority.Default,
                null);

            return config;

            

        }

        private static void LoadRoutes(
            RoutesConfig config,
            XmlNode documentElement
            )
        {

            if (documentElement.Name != "Routes") return;

            foreach (XmlNode node in documentElement.ChildNodes)
            {
                if (node.Name == "IRegisterRoutes")
                {

                    XmlAttributeCollection attributeCollection
                        = node.Attributes;

                    if (attributeCollection["type"] != null 
                        && typeof(IRegisterRoutes).IsAssignableFrom(Type.GetType(attributeCollection["type"].Value)))
                    {
                        IRegisterRoutes registrar = Activator.CreateInstance(Type.GetType(attributeCollection["type"].Value)) as IRegisterRoutes;
                        config.RouteRegistrars.Add(registrar);
                    }

                }
            }

        }

    }
}