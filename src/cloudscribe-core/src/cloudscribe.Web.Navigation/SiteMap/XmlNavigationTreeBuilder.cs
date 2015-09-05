// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-09-05
// 

using cloudscribe.Web.Navigation.Helpers;
using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Logging;
using Microsoft.Dnx.Runtime;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cloudscribe.Web.Navigation
{
    public class XmlNavigationTreeBuilder : INavigationTreeBuilder
    {
        public XmlNavigationTreeBuilder(
            IApplicationEnvironment appEnv,
            IOptions<NavigationOptions> navigationOptionsAccessor,
            ILoggerFactory loggerFactory,
            IDistributedCache cache)
        {
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (navigationOptionsAccessor == null) { throw new ArgumentNullException(nameof(navigationOptionsAccessor)); }

            this.appEnv = appEnv;
            navOptions = navigationOptionsAccessor.Options;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(XmlNavigationTreeBuilder).FullName);
            this.cache = cache;

        }

        private IApplicationEnvironment appEnv;
        private NavigationOptions navOptions;
        private ILoggerFactory logFactory;
        private ILogger log;
        private TreeNode<NavigationNode> rootNode = null;
        private IDistributedCache cache;
        private const string cacheKey = "navxmlbuild";

        public async Task<TreeNode<NavigationNode>> GetTree()
        {
            // ultimately we will need to cache sitemap per site

            if (rootNode == null)
            {
                NavigationTreeXmlConverter converter = new NavigationTreeXmlConverter();

                await cache.ConnectAsync();
                byte[] bytes = await cache.GetAsync(cacheKey);
                if (bytes != null)
                {
                    string xml = Encoding.UTF8.GetString(bytes);
                    XDocument doc = XDocument.Parse(xml);
                    
                    rootNode = converter.FromXml(doc);
                }
                else
                {
                    rootNode = await BuildTree();
                    string xml2 = converter.ToXmlString(rootNode);

                    await cache.SetAsync(
                                        cacheKey,
                                        Encoding.UTF8.GetBytes(xml2),
                                        new DistributedCacheEntryOptions().SetSlidingExpiration(
                                            TimeSpan.FromSeconds(100))
                                            );
                                        }

                
            }

            return rootNode;
        }

        private string ResolveFilePath()
        {
            string filePath = appEnv.ApplicationBasePath + Path.DirectorySeparatorChar
                + navOptions.NavigationMapXmlFileName;

            return filePath;
        }

        private async Task<TreeNode<NavigationNode>> BuildTree()
        {
            string filePath = ResolveFilePath();

            if (!File.Exists(filePath))
            {
                log.LogError("unable to build navigation tree, could not find the file " + filePath);

                return null;
            }

            string xml;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    xml = await streamReader.ReadToEndAsync();
                }
            }

            XDocument doc = XDocument.Parse(xml);

            NavigationTreeXmlConverter converter = new NavigationTreeXmlConverter();

            TreeNode<NavigationNode> result = converter.FromXml(doc);

            return result;

        }

    }
}
