// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-09-11
// Last Modified:			2015-11-18
// 

using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(int statusCode)
        {
            //this.Response.StatusCode = statusCode;

            ActionResult result;
            if (this.Request.IsAjaxRequest())
            {
                // This allows us to show errors even in partial views.
                result = this.PartialView("Error", statusCode);
            }
            else
            {
                result = this.View("Error", statusCode);
            }

            return result;
        }

    }
}
