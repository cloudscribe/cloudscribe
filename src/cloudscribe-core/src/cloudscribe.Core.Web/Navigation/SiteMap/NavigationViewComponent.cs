// Author:					Joe Audette
// Created:					2015-07-10
// Last Modified:			2015-07-11
// 

using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    public class NavigationViewComponent : ViewComponent
    {
        public NavigationViewComponent(
            NavigationTreeBuilder siteMapTreeBuilder,
            INavigationNodePermissionResolver permissionResolver)
        {
            builder = siteMapTreeBuilder;
            this.permissionResolver = permissionResolver;
        }

        private NavigationTreeBuilder builder;
        INavigationNodePermissionResolver permissionResolver;

        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            // TODO: await something
            // make builder async
            NavigationViewModel model = new NavigationViewModel(
                Request.Path,
                builder,
                permissionResolver);
           
            return View(viewName, model);
        }


        
    }
}
