// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-01-02
// Last Modified:			2016-10-08
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    /// <summary>
    /// to use extensions from inside a controller you have to use the this keyword to call the extension method
    /// http://stackoverflow.com/questions/12105869/controller-extension-method-without-this
    /// </summary>
    public static class Extensions
    {
        
        public static RedirectResult RedirectToSiteRoot(this Controller controller, ISiteContext site)
        {
            if(!string.IsNullOrWhiteSpace(site.SiteFolderName))
            {
                return controller.Redirect("/" + site.SiteFolderName);
            }

            return controller.Redirect("/");
        }

        

        //public static IActionResult RedirectToLocal(this Controller controller, string returnUrl)
        //{
        //    if (controller.Url.IsLocalUrl(returnUrl))
        //    {
        //        return controller.Redirect(returnUrl);
        //    }
        //    else
        //    {
        //        return controller.RedirectToAction("Index", "Home");
        //    }
        //}   

    }
}
