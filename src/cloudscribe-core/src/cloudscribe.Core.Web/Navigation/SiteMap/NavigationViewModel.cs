// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-12
// 

using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationViewModel
    {
        public NavigationViewModel(
            string requestPath,
            TreeNode<NavigationNode> rootNode,
            INavigationNodePermissionResolver permissionResolver)
        {
            this.requestPath = requestPath;
            this.RootNode = rootNode;
            this.permissionResolver = permissionResolver;

            ShouldAllowView = permissionResolver.ShouldAllowView;
            
        }

        private string requestPath;
        private INavigationNodePermissionResolver permissionResolver;

        public TreeNode<NavigationNode> RootNode { get; private set; }

        private TreeNode<NavigationNode> currentNode = null;
        public TreeNode<NavigationNode> CurrentNode
        {
            // lazy load
            get {
                if (currentNode == null) { currentNode = RootNode.FindByUrl(requestPath); }
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


        public Func<TreeNode<NavigationNode>, bool> ShouldAllowView { get; private set; } = null;

        

    }
}
