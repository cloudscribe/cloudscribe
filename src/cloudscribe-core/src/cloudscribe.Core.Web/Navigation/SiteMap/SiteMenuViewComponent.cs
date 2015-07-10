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
    public class SiteMenuViewComponent : ViewComponent
    {
        public SiteMenuViewComponent(SiteMapTreeBuilder siteMapTreeBuilder)
        {
            builder = siteMapTreeBuilder;
        }

        private SiteMapTreeBuilder builder;

        public async Task<IViewComponentResult> InvokeAsync(string viewName, bool fromCurrent)
        {
            // TODO: await something
            // make builder async
            SiteMenuViewModel model = new SiteMenuViewModel();
            model.RootNode = builder.GetTree();
            if(fromCurrent)
            {
                TreeNode<NavigationNode> current = model.RootNode.Find(IsMatch);
                if (current != null) model.RootNode = current;
            }

            //this.Request.HttpContext
            
                
            return View(viewName, model);
        }

        private bool IsMatch(TreeNode<NavigationNode> node)
        {
            if (node.Value.Key == "SiteAdmin") return true;

            return false;
        }
    }
}
