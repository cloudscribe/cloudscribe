// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-07-14
// 

using cloudscribe.Configuration;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Runtime;
using Newtonsoft.Json;
using System;
using System.IO;

namespace cloudscribe.Core.Web.Navigation
{
    public class JsonNavigationTreeBuilder : INavigationTreeBuilder
    {
        public JsonNavigationTreeBuilder(
            IApplicationEnvironment appEnv,
            IConfiguration configuration,
            ILoggerFactory loggerFactory)
        {
            if(appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (configuration == null) { throw new ArgumentNullException(nameof(configuration)); }

            this.appEnv = appEnv;
            config = configuration;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(JsonNavigationTreeBuilder).FullName);
        }

        private IApplicationEnvironment appEnv;
        private IConfiguration config;
        private ILoggerFactory logFactory;
        private ILogger log;
        private TreeNode<NavigationNode> rootNode = null;

        public TreeNode<NavigationNode> GetTree()
        {
            // ultimately we will need to cache sitemap per site

            if (rootNode == null)
            {
                rootNode = BuildTree();
            }

            return rootNode;
        }

        private TreeNode<NavigationNode> BuildTree()
        {
            string filePath = ResolveFilePath();

            if(!File.Exists(filePath))
            {
                log.LogError("unable to build navigation tree, could not find the file " + filePath);

                return null;
            }

            string json;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    json = streamReader.ReadToEnd();
                }
            }

            return BuildTreeFromJson(json);
            
        }

        private string ResolveFilePath()
        {
            string filePath = appEnv.ApplicationBasePath + Path.DirectorySeparatorChar
                + config.GetOrDefault("AppSettings:NavigationJsonFileName", "navigation.json");

            return filePath;
        }

       
        public TreeNode<NavigationNode> BuildTreeFromJson(string jsonString)
        {
            TreeNode<NavigationNode> rootNode =
                    JsonConvert.DeserializeObject<TreeNode<NavigationNode>>(jsonString, new NavigationTreeJsonConverter());


            return rootNode;

        }
    }
}
