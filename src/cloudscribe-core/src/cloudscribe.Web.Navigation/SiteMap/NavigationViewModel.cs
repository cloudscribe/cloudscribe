// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-09-10
// 

using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using System;
using System.Collections.Generic;

namespace cloudscribe.Web.Navigation
{
    public class NavigationViewModel
    {
        public NavigationViewModel(
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
            log = logger;

            removalFilters.Add(FilterIsAllowed);
            removalFilters.Add(permissionResolver.ShouldAllowView);
            removalFilters.Add(IsAllowedByAdjuster);



        }

        private ILogger log;
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

                    currentNode = RootNode.FindByUrl(context.Request.Path, nodeSearchUrlPrefix);
                    if(navigationFilterName == NamedNavigationFilters.ParentTree)
                    {
                        if(currentNode.Parent != null)
                        {
                            //log.LogInformation("NamedNavigationFilters.ParentTree so currentNode set to parent");

                            currentNode = currentNode.Parent;
                        }
                        else
                        {
                            //log.LogInformation("currentNode.Parent was null");
                        }
                    }
                }
                return currentNode;
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
                urlToUse = urlHelper.Action(node.Value.Action, node.Value.Controller);
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


        //public Func<TreeNode<NavigationNode>, bool> ShouldAllowView { get; private set; } = null;

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
