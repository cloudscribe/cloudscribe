// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-09
//	Last Modified:              2016-03-03
//

using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Razor;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;

namespace cloudscribe.Core.Web.Components
{
    public class SiteThemeListBuilder : IThemeListBuilder
    {
        public SiteThemeListBuilder(
            IApplicationEnvironment appEnvironment,
            IHttpContextAccessor contextAccessor
            )
        {
            if (appEnvironment == null) { throw new ArgumentNullException(nameof(appEnvironment)); }
            if (contextAccessor == null) { throw new ArgumentNullException(nameof(contextAccessor)); }
           
            appBasePath = appEnvironment.ApplicationBasePath;
            this.contextAccessor = contextAccessor;
            
        }

        private string appBasePath;
        private IHttpContextAccessor contextAccessor;

        public List<SelectListItem> GetAvailableThemes()
        {
            List<SelectListItem> layouts = new List<SelectListItem>();

            string pathToViews = GetPathToViews();

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToViews);
            
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

            if (layouts.Count == 0)
            {
                layouts.Add(new SelectListItem
                {
                    Text = "default",
                    Value = ""
                });
            }

            return layouts;

        }

        private string GetPathToViews()
        {
            var tenant = contextAccessor.HttpContext.GetTenant<SiteSettings>();
            // TODO: more configurable?
            return appBasePath + "/tenantfiles/tenant-" 
                + tenant.SiteId.ToInvariantString() 
                + "/themes/".Replace("/", Path.DirectorySeparatorChar.ToString());
        }

    }
}
