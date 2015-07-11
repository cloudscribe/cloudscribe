// Author:					Joe Audette
// Created:					2015-07-11
// Last Modified:			2015-07-11
// 

using cloudscribe.AspNet.Identity;
using Microsoft.AspNet.Hosting;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationNodePermissionResolver : INavigationNodePermissionResolver
    {
        public NavigationNodePermissionResolver(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private IHttpContextAccessor httpContextAccessor;

        public bool ShouldAllowView(TreeNode<NavigationNode> menuNode)
        {
            if (menuNode.Value.ViewRoles.Length == 0) { return true; }
            if (menuNode.Value.ViewRoles == "All Users;") { return true; }

            if (httpContextAccessor.HttpContext.User.IsInRoles(menuNode.Value.ViewRoles))
            {
                return true;
            }

            return false;
        }
    }
}
