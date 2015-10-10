// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-09
//	Last Modified:              2015-10-10
//


using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace cloudscribe.Core.Web.Razor
{
    public class LayoutFileListBuilder : ILayoutFileListBuilder
    {
        public LayoutFileListBuilder(
            IApplicationEnvironment appEnvironment,
            IOptions<LayoutSelectorOptions> layoutOptionsAccessor,
            ILayoutFileDisplayNameFilter layoutDisplayFilter
            )
        {
            if (appEnvironment == null) { throw new ArgumentNullException(nameof(appEnvironment)); }
            if (layoutOptionsAccessor == null) { throw new ArgumentNullException(nameof(layoutOptionsAccessor)); }
            if (layoutDisplayFilter == null) { throw new ArgumentNullException(nameof(layoutDisplayFilter)); }

            appBasePath = appEnvironment.ApplicationBasePath;
            options = layoutOptionsAccessor.Options;
            this.layoutDisplayFilter = layoutDisplayFilter;
        }

        private string appBasePath;
        private LayoutSelectorOptions options;
        private ILayoutFileDisplayNameFilter layoutDisplayFilter;

        public List<SelectListItem> GetAvailableLayouts(int siteId)
        {
            List<SelectListItem> layouts = new List<SelectListItem>();

            string pathToViews = GetPathToViews();

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToViews);

            string filterPattern = string.Format(options.BrowseFilterFormat, siteId.ToString());

            FileInfo[] files = directoryInfo.GetFiles(filterPattern);
            foreach(FileInfo f in files)
            {
                SelectListItem layout = new SelectListItem
                {
                    Text = layoutDisplayFilter.FilterDisplayName(siteId,f.Name),
                    Value = f.Name
                };
                layouts.Add(layout);
            }

            if(layouts.Count == 0)
            {
                layouts.Add(new SelectListItem
                {
                    Text = layoutDisplayFilter.FilterDisplayName(siteId, options.DefaultLayout),
                    Value = options.DefaultLayout
                });
            }

            return layouts;

        }
        
        private string GetPathToViews()
        {
            return appBasePath + "/Views/Shared".Replace("/", Path.DirectorySeparatorChar.ToString());
        }
    }
}
