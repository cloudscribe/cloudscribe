// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-10
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        public BreadcrumbsViewComponent(
            SiteMapTreeBuilder siteMapTreeBuilder,
            INavigationNodePermissionResolver permissionResolver)
        {
            builder = siteMapTreeBuilder;
            this.permissionResolver = permissionResolver;
        }

        private SiteMapTreeBuilder builder;
        INavigationNodePermissionResolver permissionResolver;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // TODO: await something
            // make builder async
            BreadcrumbsViewModel model = new BreadcrumbsViewModel();
            model.ShouldAllowView = permissionResolver.ShouldAllowView;
            model.RootNode = builder.GetTree();
            model.CurrentNode = model.RootNode.FindByUrl(Request.Path);
            if(model.CurrentNode != null)
            {
                model.ParentChain = model.CurrentNode.GetParentNodeChain(true, true);
            }
            

            return View(model);
        }

    }
}
