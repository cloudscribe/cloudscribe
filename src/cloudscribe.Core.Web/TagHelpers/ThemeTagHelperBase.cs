// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-12
// Last Modified:			2017-05-13
// 


using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.IO;

namespace cloudscribe.Core.Web.TagHelpers
{
    public abstract class ThemeTagHelperBase : TagHelper
    {
        protected const string CloudscribeTenantAttributeName = "cs-tenant";
        protected const string SharedThemeAttributeName = "cs-shared-theme";
        protected const string ResolveThemeResourceAttributeName = "cs-resolve-theme-resource";

        // implemented ThemeFileVersionProvider used below to solve this
        // http://stackoverflow.com/questions/42775181/asp-net-cores-asp-append-version-attribute-not-working-for-static-files-outside
        protected const string AppendVersionAttributeName = "asp-append-version";

        public ThemeTagHelperBase(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IHostingEnvironment hostingEnvironment,
            IMemoryCache cache,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            )
        {
            options = multiTenantOptionsAccessor.Value;
            this.urlHelperFactory = urlHelperFactory;
            this.actionContextAccesor = actionContextAccessor;
            HostingEnvironment = hostingEnvironment;
            Cache = cache;
        }

        private ThemeFileVersionProvider fileVersionProvider;
        protected MultiTenantOptions options;
        protected IUrlHelperFactory urlHelperFactory;
        protected IActionContextAccessor actionContextAccesor;
        protected IHostingEnvironment HostingEnvironment { get; }
        protected IMemoryCache Cache { get; }


        [HtmlAttributeName(AppendVersionAttributeName)]
        public bool? AppendVersion { get; set; }

        [HtmlAttributeName(ResolveThemeResourceAttributeName)]
        public bool ResolveThemeResource { get; set; }

        [HtmlAttributeName(SharedThemeAttributeName)]
        public bool SharedTheme { get; set; }

        [HtmlAttributeName(CloudscribeTenantAttributeName)]
        public ISiteContext Tenant { get; set; } = null;

        protected void EnsureFileVersionProvider()
        {
            if (fileVersionProvider == null)
            {
                string themePath;
                string pathBase;
                if (SharedTheme && options.UseSharedThemes)
                {
                    themePath = Path.Combine(HostingEnvironment.ContentRootPath,
                        options.SharedThemesFolderName,
                        Tenant.Theme,
                        options.ThemeStaticFilesFolderName);
                }
                else
                {
                    themePath = Path.Combine(HostingEnvironment.ContentRootPath,
                        options.SiteFilesFolderName,
                        Tenant.AliasId,
                        options.SiteThemesFolderName,
                        Tenant.Theme,
                        options.ThemeStaticFilesFolderName);
                }

                pathBase = "/" + Tenant.SiteFolderName + "/" + Tenant.Theme;

                var fileProvider = new PhysicalFileProvider(themePath);
                fileVersionProvider = new ThemeFileVersionProvider(
                    fileProvider,
                    Cache,
                    pathBase);
            }
        }

        protected string ResolveResourceUrl(string inputUrl)
        {
            if (string.IsNullOrEmpty(inputUrl)) return null;

            var tenantSegment = "";
            if (options.Mode == MultiTenantMode.FolderName && !string.IsNullOrEmpty(Tenant.SiteFolderName))
            {
                tenantSegment = Tenant.SiteFolderName + "/";
            }
            string result = null;
            if (inputUrl.StartsWith("/"))
            {
                result = "/" + tenantSegment + Tenant.Theme + inputUrl;
            }
            if (inputUrl.StartsWith("~/"))
            {
                result = "~/" + tenantSegment + Tenant.Theme + inputUrl.Substring(1);
            }

            if(AppendVersion == true)
            {
                result = AppendVersionToUrl(result, inputUrl.Replace("~",""));
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var url = urlHelper.Content(result);

            return url;
        }

        private string AppendVersionToUrl(string resolvedUrl, string originalUrl)
        {
            EnsureFileVersionProvider();
            return fileVersionProvider.AddFileVersionToPath(resolvedUrl, originalUrl);
        }



    }
}
