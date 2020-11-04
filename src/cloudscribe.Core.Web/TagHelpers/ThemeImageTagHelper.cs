// Licensed under the Apache License, Version 2.0
// Author:					Simon Annetts
// Created:					2020-05-07
// Last Modified:			2020-05-07
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Web.TagHelpers
{
    /// <summary>
    /// This TagHelper prepends the theme name to the image url to support keeping the static resources for a them within the theme folder and serving them from there.
    /// Serving the files from there requires configuring the static files middleware as seen in our BuilderExtensions.cs
    /// For folder tenants it also prepends the site folder name first so that the tenant can be resolved by the static files middleware correctly
    /// </summary>
    [HtmlTargetElement("img", Attributes = "cs-tenant,cs-resolve-theme-resource")]
    public class ThemeImageTagHelper : ThemeTagHelperBase
    {
        public ThemeImageTagHelper(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IWebHostEnvironment hostingEnvironment,
            IMemoryCache cache,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) : base(multiTenantOptionsAccessor, hostingEnvironment, cache, urlHelperFactory, actionContextAccessor)
        {
            
        }

        private const string SrcAttributeName = "src";

        [HtmlAttributeName(SrcAttributeName)]
        public string Src { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // we don't need to output these attributes 
            output.Attributes.RemoveAll(CloudscribeTenantAttributeName);
            output.Attributes.RemoveAll(ResolveThemeResourceAttributeName);
            output.Attributes.RemoveAll(SharedThemeAttributeName);

            if (!ResolveThemeResource) return;
            if (string.IsNullOrEmpty(Src)) return;
            if (Tenant == null) return;
            var resolvedSrc = ResolveResourceUrl(Src);
            if (string.IsNullOrEmpty(resolvedSrc)) return;
            
            output.Attributes.RemoveAll(SrcAttributeName);
            output.Attributes.Add(SrcAttributeName, resolvedSrc);
        }
        
    }
}
