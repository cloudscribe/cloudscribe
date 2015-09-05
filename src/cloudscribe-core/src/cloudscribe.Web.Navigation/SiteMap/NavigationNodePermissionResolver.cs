// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-11
// Last Modified:			2015-09-05
// 

using cloudscribe.Web.Navigation.Helpers;
using Microsoft.AspNet.Http;

namespace cloudscribe.Web.Navigation
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
