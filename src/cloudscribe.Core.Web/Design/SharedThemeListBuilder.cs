// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-11-14
//	Last Modified:              2016-11-14
//

using cloudscribe.Web.Common.Razor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.IO;

namespace cloudscribe.Core.Web.Components
{
    public class SharedThemeListBuilder : IThemeListBuilder
    {
        public SharedThemeListBuilder(IHostingEnvironment hostingEnvironment)
        {
            appBasePath = hostingEnvironment.ContentRootPath;
        }

        private string appBasePath;

        public List<SelectListItem> GetAvailableThemes(string aliasId = null)
        {
            List<SelectListItem> layouts = new List<SelectListItem>();

            string pathToViews = Path.Combine(appBasePath, "Themes");
            
            if (Directory.Exists(pathToViews))
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
        
    }
}
