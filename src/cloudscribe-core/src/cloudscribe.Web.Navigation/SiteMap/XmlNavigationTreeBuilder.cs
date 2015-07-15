// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-07-15
// 

using cloudscribe.Web.Navigation.Helpers;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cloudscribe.Web.Navigation
{
    public class XmlNavigationTreeBuilder : INavigationTreeBuilder
    {
        public XmlNavigationTreeBuilder(
            IApplicationEnvironment appEnv,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            this.appEnv = appEnv;
            config = configuration;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(XmlNavigationTreeBuilder).FullName);

        }

        private IApplicationEnvironment appEnv;
        private IConfiguration config;
        private ILoggerFactory logFactory;
        private ILogger log;
        private TreeNode<NavigationNode> rootNode = null;

        public async Task<TreeNode<NavigationNode>> GetTree()
        {
            // ultimately we will need to cache sitemap per site

            if (rootNode == null)
            {
                rootNode = await BuildTree();
            }

            return rootNode;
        }

        private string ResolveFilePath()
        {
            string filePath = appEnv.ApplicationBasePath + Path.DirectorySeparatorChar
                + config.GetOrDefault("AppSettings:NavigationXmlFileName", "navigation.xml");

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
