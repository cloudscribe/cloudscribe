// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-09-16
// 

using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.WebUtilities;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;
using cloudscribe.Web.Navigation.Helpers;

namespace cloudscribe.Web.Navigation
{
    public class NavigationViewModel
    {
        public NavigationViewModel(
            string startingNodeKey,
            string navigationFilterName,
            HttpContext context,
            IUrlHelper urlHelper,
            TreeNode<NavigationNode> rootNode,
            INavigationNodePermissionResolver permissionResolver,
            string nodeSearchUrlPrefix,
            ILogger logger)
        {
            this.navigationFilterName = navigationFilterName;
            this.nodeSearchUrlPrefix = nodeSearchUrlPrefix;
            this.context = context;
            this.RootNode = rootNode;
            this.permissionResolver = permissionResolver;
            this.urlHelper = urlHelper;
            this.startingNodeKey = startingNodeKey;
            log = logger;

            removalFilters.Add(FilterIsAllowed);
            removalFilters.Add(permissionResolver.ShouldAllowView);
            removalFilters.Add(IsAllowedByAdjuster);



        }

        private ILogger log;
        private string startingNodeKey;
        private string navigationFilterName;
        private string nodeSearchUrlPrefix;
        private HttpContext context;
        private IUrlHelper urlHelper;
        private INavigationNodePermissionResolver permissionResolver;
        private List<Func<TreeNode<NavigationNode>, bool>> removalFilters = new List<Func<TreeNode<NavigationNode>, bool>>();

        public TreeNode<NavigationNode> RootNode { get; private set; }

        /// <summary>
        /// a place to temporarily stash a node for processing by a template
        /// </summary>
        public TreeNode<NavigationNode> TempNode { get; private set; } = null;

        private TreeNode<NavigationNode> startingNode = null;

        public TreeNode<NavigationNode> StartingNode
        {
            // lazy load
            get
            {
                if (startingNode == null)
                {
                    if (startingNodeKey.Length > 0)
                    {
                        startingNode = RootNode.FindByKey(startingNodeKey);
                        if (startingNode == null)
                        {
                            log.LogWarning("could not find navigation node for starting node key "
                                + startingNodeKey
                                + " will fallback to RootNode.");
                        }
                    }

                    return RootNode;
                }

                return startingNode;
            }
        }

        private TreeNode<NavigationNode> currentNode = null;
        /// <summary>
        /// the node corresponding to the current request url
        /// </summary>
        public TreeNode<NavigationNode> CurrentNode
        {
            // lazy load
            get
            {
                if (currentNode == null)
                {
                    //log.LogInformation("currentNode was null so lazy loading it");
                    
                    if (currentNode == null)
                    {
                        currentNode = RootNode.FindByUrl(context.Request.Path, nodeSearchUrlPrefix);
                    }
                        
                    
                }
                return currentNode;
            }
        }

        public TreeNode<NavigationNode> ParentNode
        {
            // lazy load
            get
            {
                if (CurrentNode.Parent != null)
                {
                    return CurrentNode.Parent;
                }
                return null;
            }
        }

        private List<TreeNode<NavigationNode>> parentChain = null;
        public List<TreeNode<NavigationNode>> ParentChain
        {
            // lazy load
            get {
                if (parentChain == null) { parentChain = CurrentNode.GetParentNodeChain(true, true); }
                return parentChain;
            }
        } 

        public string AdjustText(TreeNode<NavigationNode> node)
        {
            string key = NavigationNodeAdjuster.KeyPrefix + node.Value.Key;
            if (context.Items[key] != null)
            {
                NavigationNodeAdjuster adjuster = (NavigationNodeAdjuster)context.Items[key];
                if(adjuster.ViewFilterName == navigationFilterName)
                {
                    if (adjuster.AdjustedText.Length > 0) { return adjuster.AdjustedText; }
                }
                
            }

            return node.Value.Text;
        }

        public string AdjustUrl(TreeNode<NavigationNode> node)
        {
            string urlToUse = string.Empty;
            if ((node.Value.Action.Length > 0)&&(node.Value.Controller.Length > 0))
            {
                if(node.Value.PreservedRouteParameters.Length > 0)
                {
                    List<string> preservedParams = node.Value.PreservedRouteParameters.SplitOnChar(',');
                    //var queryBuilder = new QueryBuilder();
                    //var routeParams = new { };
                    var queryStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    foreach (string p in preservedParams)
                    {
                        if(context.Request.Query.ContainsKey(p))
                        {
                            queryStrings.Add(p, context.Request.Query[p]);
                        }
                    }
                    
                    urlToUse = urlHelper.Action(node.Value.Action, node.Value.Controller);
                    if((urlToUse != null)&&(queryStrings.Count > 0))
                    {
                        urlToUse = QueryHelpers.AddQueryString(urlToUse, queryStrings);
                    }
                    
                }
                else
                {
                    urlToUse = urlHelper.Action(node.Value.Action, node.Value.Controller);
                }
                
            }

            string key = NavigationNodeAdjuster.KeyPrefix + node.Value.Key;

            if (context.Items[key] != null)
            {
                NavigationNodeAdjuster adjuster = (NavigationNodeAdjuster)context.Items[key];
                if (adjuster.ViewFilterName == navigationFilterName)
                {
                    if (adjuster.AdjustedUrl.Length > 0) { return adjuster.AdjustedUrl; }
                }
            }

            if(string.IsNullOrEmpty(urlToUse)) { return node.Value.Url; }

            //if(urlToUse.Length > 0) { return urlToUse; }
       
            return urlToUse;
        }
        
        public bool ShouldAllowView(TreeNode<NavigationNode> node)
        {
            foreach(var filter in removalFilters)
            {
                if (!filter.Invoke(node)) { return false; }
            }

            return true;
        }

        public bool HasVisibleChildren(TreeNode<NavigationNode> node)
        {
            if(node == null) { return false; }

            foreach(var childNode in node.Children)
            {
                if(ShouldAllowView(childNode)) { return true; }
            }

            return false;
        }

        public string UpdateTempNode(TreeNode<NavigationNode> node)
        {
            TempNode = node;

            return string.Empty;
        }

        private bool IsAllowedByAdjuster(TreeNode<NavigationNode> node)
        {
            string key = NavigationNodeAdjuster.KeyPrefix + node.Value.Key;
            if (context.Items[key] != null)
            {
                NavigationNodeAdjuster adjuster = (NavigationNodeAdjuster)context.Items[key];
                if (adjuster.ViewFilterName == navigationFilterName)
                {
                    if(adjuster.AdjustRemove) { return false; }
                }
            }

            return true;
        }

        private bool FilterIsAllowed(TreeNode<NavigationNode> node)
        {
            if (node.Value.ComponentVisibility.Length == 0) { return true; }
            if (navigationFilterName.Length == 0) { return false; }
            if (node.Value.ComponentVisibility.Contains(navigationFilterName)) { return true; }
           
            return false;
        }




    }
}
