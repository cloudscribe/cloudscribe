// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-12
// 

using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationViewModel
    {
        public NavigationViewModel(
            string navigationFilterName,
            HttpContext context,
            TreeNode<NavigationNode> rootNode,
            INavigationNodePermissionResolver permissionResolver)
        {
            this.navigationFilterName = navigationFilterName;
            this.context = context;
            this.RootNode = rootNode;
            this.permissionResolver = permissionResolver;

            removalFilters.Add(FilterIsAllowed);
            removalFilters.Add(permissionResolver.ShouldAllowView);

            
            
        }

        private string navigationFilterName;
        private HttpContext context;
        private string requestPath;
        private INavigationNodePermissionResolver permissionResolver;
        private List<Func<TreeNode<NavigationNode>, bool>> removalFilters = new List<Func<TreeNode<NavigationNode>, bool>>();

        public TreeNode<NavigationNode> RootNode { get; private set; }

        private TreeNode<NavigationNode> currentNode = null;
        public TreeNode<NavigationNode> CurrentNode
        {
            // lazy load
            get {
                if (currentNode == null) { currentNode = RootNode.FindByUrl(context.Request.Path); }
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
            string key = "breadcrumb-" + node.Value.Key;
            if (context.Items[key] != null)
            {
                BreadcrumbAdjuster adjuster = (BreadcrumbAdjuster)context.Items[key];
                if (adjuster.AdjustedText.Length > 0) { return adjuster.AdjustedText; }
            }

            return node.Value.Text;
        }

        public string AdjustUrl(TreeNode<NavigationNode> node)
        {
            string key = "breadcrumb-" + node.Value.Key;
            if (context.Items[key] != null)
            {
                BreadcrumbAdjuster adjuster = (BreadcrumbAdjuster)context.Items[key];
                if (adjuster.AdjustedUrl.Length > 0) { return adjuster.AdjustedUrl; }
            }
       
            return node.Value.Url;
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

        private bool FilterIsAllowed(TreeNode<NavigationNode> node)
        {
            if (node.Value.ComponentVisibility.Length == 0) { return true; }
            if (navigationFilterName.Length == 0) { return false; }
            if (node.Value.ComponentVisibility.Contains(navigationFilterName)) { return true; }
           
            return false;
        }




    }
}
