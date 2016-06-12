// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-09
//	Last Modified:              2016-06-12
//

using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Razor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace cloudscribe.Core.Web.Components
{
    public class SiteThemeListBuilder : IThemeListBuilder
    {
        public SiteThemeListBuilder(
            IHostingEnvironment hostingEnvironment,
            IHttpContextAccessor contextAccessor
            )
        {
            if (hostingEnvironment == null) { throw new ArgumentNullException(nameof(hostingEnvironment)); }
            if (contextAccessor == null) { throw new ArgumentNullException(nameof(contextAccessor)); }
           
            appBasePath = hostingEnvironment.ContentRootPath;
            this.contextAccessor = contextAccessor;
            
        }

        private string appBasePath;
        private IHttpContextAccessor contextAccessor;

        public List<SelectListItem> GetAvailableThemes(string aliasId = null)
        {
            List<SelectListItem> layouts = new List<SelectListItem>();

            string pathToViews = GetPathToViews(aliasId);
            if(Directory.Exists(pathToViews))
            {
                var directoryInfo = new DirectoryInfo(pathToViews);
                var folders = directoryInfo.GetDirectories();
                foreach (DirectoryInfo f in folders)
                {
                    SelectListItem layout = new SelectListItem
                    {
                        Text = f.Name,
                        Value = f.Name
                    };
                    layouts.Add(layout);
                }
            }
            
            layouts.Add(new SelectListItem
            {
                Text = "default",
                Value = ""
            });
           
            return layouts;

        }

        private string GetPathToViews(string aliasId = null)
        {
            if(!string.IsNullOrEmpty(aliasId))
            {
                return appBasePath + "/sitefiles/"
               + aliasId
               + "/themes/".Replace("/", Path.DirectorySeparatorChar.ToString());
            }

            var tenant = contextAccessor.HttpContext.GetTenant<SiteSettings>();
            // TODO: more configurable?
            return appBasePath + "/sitefiles/" 
                + tenant.AliasId
                + "/themes/".Replace("/", Path.DirectorySeparatorChar.ToString());
        }

    }
}
