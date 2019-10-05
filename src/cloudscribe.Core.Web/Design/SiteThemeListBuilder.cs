// Licensed under the Apache License, Version 2.0
//  Author:                     Joe Audette
//  Created:                    2015-10-09
//	Last Modified:              2019-09-01
//

using cloudscribe.Core.Models;
using cloudscribe.Web.Common.Razor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace cloudscribe.Core.Web.Components
{
    public class SiteThemeListBuilder : IThemeListBuilder
    {
        public SiteThemeListBuilder(
            IWebHostEnvironment hostingEnvironment,
            IHttpContextAccessor contextAccessor,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor
            )
        {
            if (hostingEnvironment == null) { throw new ArgumentNullException(nameof(hostingEnvironment)); }

            _appBasePath = hostingEnvironment.ContentRootPath;
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _options = multiTenantOptionsAccessor.Value;

        }

        private string _appBasePath;
        private IHttpContextAccessor _contextAccessor;
        private MultiTenantOptions _options;

        public List<SelectListItem> GetAvailableThemes(string aliasId = null)
        {
            List<SelectListItem> layouts = new List<SelectListItem>();
            if(_options.UserPerSiteThemes)
            {
                string pathToViews = GetPathToViews(aliasId);
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
            }
            

            if(_options.UseSharedThemes)
            {
                var sharedThemes = GetSharedThemes();
                layouts.AddRange(sharedThemes);
            }
            
            layouts.Add(new SelectListItem
            {
                Text = "default",
                Value = ""
            });
           
            return layouts;

        }

        private List<SelectListItem> GetSharedThemes()
        {
            List<SelectListItem> layouts = new List<SelectListItem>();

            string pathToViews = Path.Combine(_appBasePath, _options.SharedThemesFolderName);

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
            
            return layouts;

        }

        private string GetPathToViews(string aliasId = null)
        {
            if(string.IsNullOrEmpty(aliasId))
            {
                var tenant = _contextAccessor.HttpContext.GetTenant<SiteContext>();
                aliasId = tenant.AliasId;
            }
            
            return Path.Combine(_appBasePath, _options.SiteFilesFolderName, aliasId, _options.SiteThemesFolderName);
        }

    }
}
