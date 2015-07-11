// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-11
// 

using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationViewModel
    {
        public NavigationViewModel(
            string requestPath,
            NavigationTreeBuilder siteMapTreeBuilder,
            INavigationNodePermissionResolver permissionResolver)
        {
            this.requestPath = requestPath;
            this.siteMapTreeBuilder = siteMapTreeBuilder;
            this.permissionResolver = permissionResolver;

            ShouldAllowView = permissionResolver.ShouldAllowView;
            RootNode = this.siteMapTreeBuilder.GetTree();

        }

        private string requestPath;
        private NavigationTreeBuilder siteMapTreeBuilder;
        private INavigationNodePermissionResolver permissionResolver;

        public TreeNode<NavigationNode> RootNode { get; private set; } = null;

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
