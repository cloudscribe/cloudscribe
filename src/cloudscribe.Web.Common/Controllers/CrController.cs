// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. 
// Author:                  Joe Audette
// Created:                 2017-06-07
// Last Modified:           2017-06-08
// 

// this implementation worked great but using mvc for serving static files means that the authorization middleware runs for static resource requests
// it is more ideal to serve embedded resources using static files middleware which runs before authentication/authorization middelware
// however to get it working I had to implement a custom IFileProvider, EmbeddedFileResolvingProvider
// because the built in EmbeddedFileProvider doesn't correctly map folder names with - to _
// I am keeping this implementation here in comments as a reference in case I need to use this kind of solution elsewhere

//using cloudscribe.Web.Common.Helpers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.IO;
//using System.Reflection;

//namespace cloudscribe.Web.Common.Controllers
//{
//    /// <summary>
//    /// common js, css, fonts, etc
//    /// as I build more projects I find I need the same resources that I have been embedding in each project over and over
//    /// so I decided to put the commonly used things in one place in this common project
//    /// examples: bootstrap js, ckeditor, momentjs
//    /// 
//    /// </summary>
//    public class CrController : Controller
//    {
//        public CrController(
//            IResourceHelper resourceHelper,
//            ILogger<CrController> logger
//            )
//        {
//            this.resourceHelper = resourceHelper;
//            log = logger;

//        }

//        private IResourceHelper resourceHelper;
//        private ILogger log;

//        private IActionResult GetResult(string resourceName, string contentType)
//        {
//            var assembly = typeof(CrController).GetTypeInfo().Assembly;
//            resourceName = resourceHelper.ResolveResourceIdentifier(resourceName);
//            var resourceStream = assembly.GetManifestResourceStream(resourceName);
//            if (resourceStream == null)
//            {
//                log.LogError("resource not found for " + resourceName);
//                return NotFound();  
//            }

//            log.LogDebug("resource found for " + resourceName);

//            return new FileStreamResult(resourceStream, contentType);
//        }

//        // /cr/js/
//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult js()
//        {
//            var baseSegment = "cloudscribe.Web.Common.js.";
//            var requestPath = HttpContext.Request.Path.Value;
//            log.LogDebug(requestPath + " requested");

//            if (requestPath.Length < "/cr/js/".Length) return NotFound();

//            var seg = requestPath.Substring("/cr/js/".Length);
//            var ext = Path.GetExtension(requestPath);
//            var mimeType = resourceHelper.GetMimeType(ext);

//            return GetResult(baseSegment + seg, mimeType);
//        }

//        // /cr/css/
//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult css()
//        {
//            var baseSegment = "cloudscribe.Web.Common.css.";
//            var requestPath = HttpContext.Request.Path.Value;
//            log.LogDebug(requestPath + " requested");

//            if (requestPath.Length < "/cr/css/".Length) return NotFound();

//            var seg = requestPath.Substring("/cr/css/".Length);
//            var ext = Path.GetExtension(requestPath);
//            var mimeType = resourceHelper.GetMimeType(ext);

//            return GetResult(baseSegment + seg, mimeType);
//        }

//        // /cr/fonts/
//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult fonts()
//        {
//            var baseSegment = "cloudscribe.Web.Common.fonts.";
//            var requestPath = HttpContext.Request.Path.Value;
//            log.LogDebug(requestPath + " requested");

//            if (requestPath.Length < "/cr/fonts/".Length) return NotFound();

//            var seg = requestPath.Substring("/cr/fonts/".Length);
//            var ext = Path.GetExtension(requestPath);
//            var mimeType = resourceHelper.GetMimeType(ext);

//            return GetResult(baseSegment + seg, mimeType);
//        }

//        // /cr/images/
//        [HttpGet]
//        [AllowAnonymous]
//        public IActionResult images()
//        {
//            var baseSegment = "cloudscribe.Web.Common.images.";
//            var requestPath = HttpContext.Request.Path.Value;
//            log.LogDebug(requestPath + " requested");

//            if (requestPath.Length < "/cr/images/".Length) return NotFound();

//            var seg = requestPath.Substring("/cr/images/".Length);
//            var ext = Path.GetExtension(requestPath);
//            var mimeType = resourceHelper.GetMimeType(ext);

//            return GetResult(baseSegment + seg, mimeType);
//        }

//    }
//}
