// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-11
// 

using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    public class SiteMenuViewComponent : ViewComponent
    {
        public SiteMenuViewComponent(
            SiteMapTreeBuilder siteMapTreeBuilder,
            INavigationNodePermissionResolver permissionResolver)
        {
            builder = siteMapTreeBuilder;
            this.permissionResolver = permissionResolver;
        }

        private SiteMapTreeBuilder builder;
        INavigationNodePermissionResolver permissionResolver;

        public async Task<IViewComponentResult> InvokeAsync(string viewName, bool fromCurrent)
        {
            // TODO: await something
            // make builder async
            SiteMenuViewModel model = new SiteMenuViewModel();
            model.ShouldAllowView = permissionResolver.ShouldAllowView;
            model.RootNode = builder.GetTree();
            if(fromCurrent)
            {
                TreeNode<NavigationNode> current = model.RootNode.FindByUrl(Request.Path);
                if (current != null) model.RootNode = current;
            }

            
            
            return View(viewName, model);
        }


        //private bool ShouldRenderNode(TreeNode<NavigationNode> menuNode)
        //{
        //    if(menuNode.Value.ViewRoles.Length == 0) { return true; }
        //    if(menuNode.Value.ViewRoles == "All Users;") { return true; }
            
        //    if(Request.HttpContext.User.IsInRoles(menuNode.Value.ViewRoles))
        //    {
        //        return true;
        //    }

        //    return false;
        //}
        
    }
}
