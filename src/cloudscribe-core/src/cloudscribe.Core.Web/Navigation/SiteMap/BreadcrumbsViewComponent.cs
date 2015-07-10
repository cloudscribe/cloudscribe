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
        public BreadcrumbsViewComponent(SiteMapTreeBuilder siteMapTreeBuilder)
        {
            builder = siteMapTreeBuilder;
        }

        private SiteMapTreeBuilder builder;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // TODO: await something
            // make builder async
            BreadcrumbsViewModel model = new BreadcrumbsViewModel();
            model.RootNode = builder.GetTree();

            return View(model);
        }

    }
}
