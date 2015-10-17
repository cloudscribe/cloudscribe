// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-14
// Last Modified:			2015-10-17
// 

using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
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
            IOptions<NavigationOptions> navigationOptionsAccessor,
            ILogger<XmlNavigationTreeBuilder> logger)
        {
            if (appEnv == null) { throw new ArgumentNullException(nameof(appEnv)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (navigationOptionsAccessor == null) { throw new ArgumentNullException(nameof(navigationOptionsAccessor)); }

            this.appEnv = appEnv;
            navOptions = navigationOptionsAccessor.Value;
            log = logger;
            
        }

        private IApplicationEnvironment appEnv;
        private NavigationOptions navOptions;
        private ILogger log;
        private TreeNode<NavigationNode> rootNode = null;
        
        public async Task<TreeNode<NavigationNode>> GetTree()
        {
           
            if (rootNode == null)
            { 
                rootNode = await BuildTree();  
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
