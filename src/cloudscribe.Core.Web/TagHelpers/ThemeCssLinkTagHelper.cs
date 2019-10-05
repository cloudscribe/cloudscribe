// Licensed under the Apache License, Version 2.0
// Author:					Joe Audette
// Created:					2017-05-12
// Last Modified:			2019-09-01
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
    /// This TagHelper prepends the theme name to the css to support keeping the static resources for a them within the theme folder and serving them from there.
    /// Serving the files from there requires configuring the static files middleware as seen in our BuilderExtensions.cs
    /// For folder tenants it also prepends the site folder name first so that the tenant can be resolved by the static files middleware correctly
    /// </summary>
    [HtmlTargetElement("link", Attributes = "cs-tenant,cs-resolve-theme-resource")]
    public class ThemeCssLinkTagHelper : ThemeTagHelperBase
    {
        private const string HrefAttributeName = "href";
        private const string RelAttributeName = "rel";
        private const string StyleSheet = "stylesheet";
        
        public ThemeCssLinkTagHelper(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IWebHostEnvironment hostingEnvironment,
            IMemoryCache cache,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            ) :base(multiTenantOptionsAccessor, hostingEnvironment, cache, urlHelperFactory, actionContextAccessor)
        {
            
        }
        
        [HtmlAttributeName(HrefAttributeName)]
        public string Href { get; set; }

        [HtmlAttributeName(RelAttributeName)]
        public string Rel { get; set; }
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // we don't need to output these attributes 
            output.Attributes.RemoveAll(CloudscribeTenantAttributeName);
            output.Attributes.RemoveAll(ResolveThemeResourceAttributeName);
            output.Attributes.RemoveAll(SharedThemeAttributeName);

            if (!ResolveThemeResource) return;
            if (string.IsNullOrEmpty(Href)) return;
            if (string.IsNullOrEmpty(Rel)) return;
            if (Rel != StyleSheet) return;
            if (Tenant == null) return;
            var resolvedHref = ResolveResourceUrl(Href);
            if (string.IsNullOrEmpty(resolvedHref)) return;
            
            output.Attributes.RemoveAll(RelAttributeName);
            output.Attributes.Add(RelAttributeName, Rel);

            output.Attributes.RemoveAll(HrefAttributeName);
            output.Attributes.Add(HrefAttributeName, resolvedHref);
        } 
    }
}
