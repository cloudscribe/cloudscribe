// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-06-08
// Last Modified:           2016-06-22
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


        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickercss()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.css.bootstrap-datetimepicker.min.css",
                "text/css");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult momentwithlocalesjs()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.moment-with-locales.min.js",
                "text/javascript");
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapdatetimepickerjs()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.bootstrap-datetimepicker.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bsdpu()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-datepicker-bootstrap-unobtrusive.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult jqueryvalidate()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.jquery.validate.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult jqueryvalidationunobtrusive()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.jquery.validate.unobtrusive.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult jqdp()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-datepicker-jqui-unobtrusive.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult requiredwhenvalidator()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-validation-requiredwhen.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult enforcetruevalidator()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-validation-enforcetrue.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult bootstrapmodal()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-modaldialog-bootstrap.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult sidemenu()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-sidemenu.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult metismenu()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.metisMenu.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult cascade()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-cascade-unobtrusive.min.js",
                "text/javascript");
        }

        [HttpGet]
        [AllowAnonymous]
        public ContentResult ckunobtrusive()
        {
            return GetContentResult(
                "cloudscribe.Core.Web.js.cloudscribe-ckeditor-unobtrusive.min.js",
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





    }
}
