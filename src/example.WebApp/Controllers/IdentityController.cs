// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using cloudscribe.Core.Web.Components;

namespace example.WebApp.Controllers
{
    [Route("{sitefolder}/api/[controller]")]
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        //[Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        //[Authorize(Policy = "AdminPolicy")]
        //[HttpPost]
        //public IActionResult Post()
        //{
        //    return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        //}
    }
}