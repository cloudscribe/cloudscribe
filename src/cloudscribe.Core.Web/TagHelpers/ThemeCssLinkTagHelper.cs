// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-05-12
// Last Modified:			2017-05-12
// 


using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc.TagHelpers.Internal;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace cloudscribe.Core.Web.TagHelpers
{
    //https://github.com/aspnet/Mvc/blob/f222fa4349ff0aeff80ac1dbde6cac6429d990cf/src/Microsoft.AspNetCore.Mvc.TagHelpers/LinkTagHelper.cs
    //https://github.com/aspnet/Mvc/blob/cd9899363f7429ddc8cb1589f3d441c9197c8d9f/test/Microsoft.AspNetCore.Mvc.TagHelpers.Test/LinkTagHelperTest.cs

    //https://github.com/anth12/snake-10k/blob/431a2ae3f991f7583958d0119cbe09b59190ebfe/src/Snake.Web/TagHelpers/LinkTagHelper.cs
    //https://github.com/stevetayloruk/Orchard-Harvest-2017-Demo/blob/1bc55e16ce4c8ee12318fcb730dcdf2c32f5662b/src/Orchard.ResourceManagement/TagHelpers/LinkTagHelper.cs

    // http://stackoverflow.com/questions/42775181/asp-net-cores-asp-append-version-attribute-not-working-for-static-files-outside

    [HtmlTargetElement("link", Attributes = "cs-tenant,cs-resolve-theme-resource")]
    public class ThemeCssLinkTagHelper : TagHelper
    {
        private const string CloudscribeTenantAttributeName = "cs-tenant";
        private const string ResolveThemeResourceAttributeName = "cs-resolve-theme-resource";
        private const string HrefAttributeName = "href";
        private const string RelAttributeName = "rel";
        private const string AppendVersionAttributeName = "asp-append-version";

        public ThemeCssLinkTagHelper(
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

        private MultiTenantOptions options;
        private IUrlHelperFactory urlHelperFactory;
        private IActionContextAccessor actionContextAccesor;
        private ThemeFileVersionProvider fileVersionProvider;
        protected IHostingEnvironment HostingEnvironment { get; }
        protected IMemoryCache Cache { get; }

        /// <summary>
        /// Value indicating if file version should be appended to the href urls.
        /// </summary>
        /// <remarks>
        /// If <c>true</c> then a query string "v" with the encoded content of the file is added.
        /// </remarks>
        [HtmlAttributeName(AppendVersionAttributeName)]
        public bool? AppendVersion { get; set; }

        [HtmlAttributeName(HrefAttributeName)]
        public string Href { get; set; }

        [HtmlAttributeName(RelAttributeName)]
        public string Rel { get; set; }

        [HtmlAttributeName(ResolveThemeResourceAttributeName)]
        public bool ResolveThemeResource { get; set; }

        [HtmlAttributeName(CloudscribeTenantAttributeName)]
        public ISiteContext Tenant { get; set; } = null;

        private void EnsureFileVersionProvider(string pathBase)
        {
            if (fileVersionProvider == null)
            {
                var themePath = Path.Combine(HostingEnvironment.ContentRootPath, options.SiteFilesFolderName, Tenant.AliasId, options.SiteThemesFolderName, Tenant.Theme, "static");
                var fileProvider = new PhysicalFileProvider(themePath);
                fileVersionProvider = new ThemeFileVersionProvider(
                    fileProvider,
                    Cache,
                    pathBase);
            }
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // we don't need to output these attributes 
            output.Attributes.RemoveAll(CloudscribeTenantAttributeName);
            output.Attributes.RemoveAll(ResolveThemeResourceAttributeName);

            if (!ResolveThemeResource) return;
            if (string.IsNullOrEmpty(Href)) return;
            if (string.IsNullOrEmpty(Rel)) return;
            if (Rel != "stylesheet") return;
            if (Tenant == null) return;
            if (options.Mode != MultiTenantMode.FolderName) return;
            if (string.IsNullOrEmpty(Tenant.SiteFolderName)) return;
           
            var originalHref = Href.Replace("~", "");

            var resolvedHref = ResolveHref();
            if (string.IsNullOrEmpty(resolvedHref)) return;
            
            var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccesor.ActionContext);
            var url = urlHelper.Content(resolvedHref);
            if (AppendVersion == true)
            {
                //TODO: handle shared themes in addition to tenant themes?
                EnsureFileVersionProvider("/" + Tenant.SiteFolderName + "/" + Tenant.Theme);

                var versionedPath = fileVersionProvider.AddFileVersionToPath(url, originalHref);
                url = versionedPath;
            }

            output.Attributes.RemoveAll(RelAttributeName);
            output.Attributes.Add(RelAttributeName, Rel);

            output.Attributes.RemoveAll(HrefAttributeName);
            output.Attributes.Add(HrefAttributeName, url);

            

        }

        private string ResolveHref()
        {
            //TODO: handle shared themes in addition to tenant themes?

            //prepend the tenant folder segment
            if (Href.StartsWith("/")) return "/" + Tenant.SiteFolderName + "/" + Tenant.Theme + Href;
            if (Href.StartsWith("~/")) return "~/" + Tenant.SiteFolderName + "/" + Tenant.Theme + Href.Substring(1);
            return null;
        }
    }
}
