//  Author:                     Joe Audette
//  Created:                    2014-10-09
//	Last Modified:              2014-10-09
// 

using log4net;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace cloudscribe.Configuration
{
    public class VersionProviderConfig
    {
        private static readonly ILog log
            = LogManager.GetLogger(typeof(VersionProviderConfig));


        private ProviderSettingsCollection providerSettingsCollection
            = new ProviderSettingsCollection();

        public ProviderSettingsCollection Providers
        {
            get { return providerSettingsCollection; }
        }

        public static VersionProviderConfig GetConfig()
        {
            try
            {
                if (
                    (HttpRuntime.Cache["VersionProviderConfig"] != null)
                    && (HttpRuntime.Cache["VersionProviderConfig"] is VersionProviderConfig)
                )
                {
                    return (VersionProviderConfig)HttpRuntime.Cache["VersionProviderConfig"];
                }

                VersionProviderConfig config
                    = new VersionProviderConfig();

                String configFolderName = "~/Config/CodeVersionProviders/";

                string pathToConfigFolder
                    = HttpContext.Current.Server.MapPath(configFolderName);


                if (!Directory.Exists(pathToConfigFolder)) return config;

                DirectoryInfo directoryInfo
                    = new DirectoryInfo(pathToConfigFolder);

                FileInfo[] configFiles = directoryInfo.GetFiles("*.config");

                foreach (FileInfo fileInfo in configFiles)
                {
                    XmlDocument configXml = new XmlDocument();
                    configXml.Load(fileInfo.FullName);
                    config.LoadValuesFromConfigurationXml(configXml.DocumentElement);

                }

                AggregateCacheDependency aggregateCacheDependency
                    = new AggregateCacheDependency();

                string pathToWebConfig
                    = HttpContext.Current.Server.MapPath("~/Web.config");

                aggregateCacheDependency.Add(new CacheDependency(pathToWebConfig));

                System.Web.HttpRuntime.Cache.Insert(
                    "VersionProviderConfig",
                    config,
                    aggregateCacheDependency,
                    DateTime.Now.AddYears(1),
                    TimeSpan.Zero,
                    System.Web.Caching.CacheItemPriority.Default,
                    null);

                return (VersionProviderConfig)HttpRuntime.Cache["VersionProviderConfig"];

            }
            catch (HttpException ex)
            {
                log.Error(ex);

            }
            catch (System.Xml.XmlException ex)
            {
                log.Error(ex);

            }
            catch (ArgumentException ex)
            {
                log.Error(ex);

            }
            catch (NullReferenceException ex)
            {
                log.Error(ex);

            }

            return null;


        }

        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "providers")
                {
                    foreach (XmlNode providerNode in child.ChildNodes)
                    {
                        if (
                            (providerNode.NodeType == XmlNodeType.Element)
                            && (providerNode.Name == "add")
                            )
                        {
                            if (
                                (providerNode.Attributes["name"] != null)
                                && (providerNode.Attributes["type"] != null)
                                )
                            {
                                ProviderSettings providerSettings
                                    = new ProviderSettings(
                                    providerNode.Attributes["name"].Value,
                                    providerNode.Attributes["type"].Value);

                                providerSettingsCollection.Add(providerSettings);
                            }

                        }
                    }

                }
            }
        }

    }
}
