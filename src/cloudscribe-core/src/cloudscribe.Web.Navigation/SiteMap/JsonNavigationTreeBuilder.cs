// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-10-17
// 

using cloudscribe.Web.Navigation.Helpers;
using Microsoft.Framework.Caching.Distributed;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Logging;
using Microsoft.Dnx.Runtime;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Web.Navigation
{
    public class JsonNavigationTreeBuilder : INavigationTreeBuilder
    {
        public JsonNavigationTreeBuilder(
            IApplicationEnvironment appEnv,
            IOptions<NavigationOptions> navigationOptionsAccessor,
            ILoggerFactory loggerFactory,
            IDistributedCache cache)
        {
            if(appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (loggerFactory == null) { throw new ArgumentNullException(nameof(loggerFactory)); }
            if (navigationOptionsAccessor == null) { throw new ArgumentNullException(nameof(navigationOptionsAccessor)); }

            this.appEnv = appEnv;
            navOptions = navigationOptionsAccessor.Value;
            logFactory = loggerFactory;
            log = loggerFactory.CreateLogger(typeof(JsonNavigationTreeBuilder).FullName);
            this.cache = cache;
        }

        private IDistributedCache cache;
        private const string cacheKey = "navjsonbuild";
        private IApplicationEnvironment appEnv;
        private NavigationOptions navOptions;
        private ILoggerFactory logFactory;
        private ILogger log;
        private TreeNode<NavigationNode> rootNode = null;

        public async Task<TreeNode<NavigationNode>> GetTree()
        {
            // ultimately we will need to cache sitemap per site

            if (rootNode == null)
            {
                
                await cache.ConnectAsync();
                byte[] bytes = await cache.GetAsync(cacheKey);
                if (bytes != null)
                {
                    string json = Encoding.UTF8.GetString(bytes);
                    rootNode = BuildTreeFromJson(json);
                }
                else
                {
                    rootNode = await BuildTree();
                    string json = rootNode.ToJsonCompact();

                    await cache.SetAsync(
                                        cacheKey,
                                        Encoding.UTF8.GetBytes(json),
                                        new DistributedCacheEntryOptions().SetSlidingExpiration(
                                            TimeSpan.FromSeconds(100))
                                            );
                }
            }

            return rootNode;
        }

        private async Task<TreeNode<NavigationNode>> BuildTree()
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
                    json = await streamReader.ReadToEndAsync();
                }
            }

            TreeNode<NavigationNode> result = BuildTreeFromJson(json);

            return result;
            
        }

        private string ResolveFilePath()
        {
            string filePath = appEnv.ApplicationBasePath + Path.DirectorySeparatorChar
                + navOptions.NavigationMapJsonFileName;

            return filePath;
        }


        // started to make this async since there are async methods of deserializeobject
        // but found this thread which says not to use them as there is no benefit
        //https://github.com/JamesNK/Newtonsoft.Json/issues/66
        public TreeNode<NavigationNode> BuildTreeFromJson(string jsonString)
        {
            TreeNode<NavigationNode> rootNode =
                    JsonConvert.DeserializeObject<TreeNode<NavigationNode>>(jsonString, new NavigationTreeJsonConverter());


            return rootNode;

        }
    }
}
