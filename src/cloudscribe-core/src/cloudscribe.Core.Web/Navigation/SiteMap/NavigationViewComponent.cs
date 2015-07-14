// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-14
// 

using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationViewComponent : ViewComponent
    {
        public NavigationViewComponent(
            INavigationTreeBuilder siteMapTreeBuilder,
            INavigationNodePermissionResolver permissionResolver)
        {
            builder = siteMapTreeBuilder;
            this.permissionResolver = permissionResolver;
        }

        private INavigationTreeBuilder builder;
        INavigationNodePermissionResolver permissionResolver;

        public async Task<IViewComponentResult> InvokeAsync(string viewName, string filterName)
        {
            // TODO: await something
            // make builder async

            TreeNode<NavigationNode> rootNode = builder.GetTree();

            NavigationViewModel model = new NavigationViewModel(
                filterName,
                Request.HttpContext,
                rootNode,
                permissionResolver);
           
            return View(viewName, model);
        }


        
    }
}
