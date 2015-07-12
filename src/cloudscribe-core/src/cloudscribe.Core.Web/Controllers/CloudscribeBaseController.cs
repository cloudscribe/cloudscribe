// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2015-06-11
// 

using Microsoft.AspNet.Mvc;

namespace cloudscribe.Core.Web.Controllers
{
    /// <summary>
    /// do we need or want a base controller?
    /// several arguments against it here:
    /// http://stackoverflow.com/questions/6119206/what-are-good-candidates-for-base-controller-class-in-asp-net-mvc
    /// </summary>
    public class CloudscribeBaseController : Controller
    {
        public CloudscribeBaseController()
        {

        }


        ///// <summary>
        ///// been thinking lately that instead of using a base controller class
        ///// it might be cleaner to just use extension methods
        ///// this now just uses the extension method inside the property getter
        ///// not much need for this base class
        ///// </summary>
        //protected ISiteContext Site
        //{
        //    get
        //    {
        //        return this.GetSiteContext();
        //        //return HttpContext.GetOwinContext().Get<ISiteContext>(); 
        //    }

        //}
    }
}
