// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-06-08
// Last Modified:           2016-06-08
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    /// <summary>
    /// cscsr : cloudscribe core static resources 
    /// </summary>
    public class cscsrController : Controller
    {
        //TODO: caching, what are best practices for caching static resources ?
        // seems like for embedded we could set long cache in production since it cannot change
        //https://docs.asp.net/en/latest/performance/caching/response.html

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickerjs()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.bootstrap-datetimepicker.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickerjsmin()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.bootstrap-datetimepicker.min.js",
                "text/javascript");
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public ContentResult jquerycookie()
        //{
        //    return GetContentResult(
        //        "cloudscribe.Core.Web.js.jquery.cookie.js",
        //        "text/javascript");
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public ContentResult jquerycookiemin()
        //{
        //    return GetContentResult(
        //        "cloudscribe.Core.Web.js.jquery.cookie.min.js",
        //        "text/javascript");
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public ContentResult jqueryhotkeys()
        //{
        //    return GetContentResult(
        //        "cloudscribe.Core.Web.js.jquery.hotkeys.js",
        //        "text/javascript");
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public ContentResult jqueryhotkeysmin()
        //{
        //    return GetContentResult(
        //        "cloudscribe.Core.Web.js.jquery.hotkeys.min.js",
        //        "text/javascript");
        //}

        private ContentResult GetContentResult(string resourceName, string contentType)
        {
            var assembly = typeof(cscsrController).GetTypeInfo().Assembly;
            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            string payload;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                payload = reader.ReadToEnd();
            }

            return new ContentResult
            {
                ContentType = contentType,
                Content = payload,
                StatusCode = 200
            };
        }



    }
}
