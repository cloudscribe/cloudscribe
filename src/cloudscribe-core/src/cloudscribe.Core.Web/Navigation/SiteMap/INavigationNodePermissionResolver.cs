// Author:					Joe Audette
// Created:					2015-07-11
// Last Modified:			2015-07-11
// 

namespace cloudscribe.Core.Web.Navigation
{
    public interface INavigationNodePermissionResolver
    {
        bool ShouldAllowView(TreeNode<NavigationNode> menuNode);
    }
}
