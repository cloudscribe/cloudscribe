// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2016-03-03
//	Last Modified:              2018-08-17
//

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;


namespace cloudscribe.Core.Web.Components
{
    public class SiteViewLocationExpander : IViewLocationExpander
    {
        
        private const string THEME_KEY = "theme", TENANT_KEY = "tenant", OPTIONS_KEY = "multitenantoptions";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if(context.ActionContext != null && context.ActionContext.HttpContext != null)
            {
                var tenant = context.ActionContext.HttpContext.GetTenant<SiteContext>();
                if(tenant != null)
                {
                    context.Values[THEME_KEY] = tenant.Theme;
                    var tenantKey = tenant.AliasId;
                    context.Values[TENANT_KEY] = tenantKey;
                }
            }
            
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {

            if (context.ActionContext == null) { return viewLocations; }
            if (context.ActionContext.HttpContext == null) { return viewLocations; }
            if (context.ActionContext.HttpContext.RequestServices == null) { return viewLocations; }

            // could not avoid the servicelocator pattern here because of the way ViewLocationExpander is added in startup
            var optionsAccessor = context.ActionContext.HttpContext.RequestServices.GetService(typeof(IOptions<MultiTenantOptions>)) as IOptions<MultiTenantOptions>;
            var options = optionsAccessor.Value;

            if (context.Values.TryGetValue(THEME_KEY, out string theme))
            {
                if (context.Values.TryGetValue(TENANT_KEY, out string tenant))
                {
                    if (options != null)
                    {
                        IEnumerable<string> tenantLocations = new[]
                        {
                            $"/{options.SiteFilesFolderName}/{tenant}/Views/{{1}}/{{0}}.cshtml",
                            $"/{options.SiteFilesFolderName}/{tenant}/Views/Shared/{{0}}.cshtml",
                            $"/{options.SiteFilesFolderName}/{tenant}/Views/EmailTemplates/{{0}}.cshtml"
                        };
                        viewLocations = tenantLocations.Concat(viewLocations);

                        IEnumerable<string> themeLocations = new[]
                        {
                            $"/{options.SiteFilesFolderName}/{tenant}/{options.SiteThemesFolderName}/{theme}/{{1}}/{{0}}.cshtml",
                            $"/{options.SiteFilesFolderName}/{tenant}/{options.SiteThemesFolderName}/{theme}/Shared/{{0}}.cshtml",
                            $"/{options.SiteFilesFolderName}/{tenant}/{options.SiteThemesFolderName}/{theme}/EmailTemplates/{{0}}.cshtml"
                            
                        };

                        viewLocations = themeLocations.Concat(viewLocations);

                        if(options.UseSharedThemes)
                        {
                            IEnumerable<string> sharedThemeLocations = new[]
                            {
                                $"/{options.SharedThemesFolderName}/{theme}/{{1}}/{{0}}.cshtml",
                                $"/{options.SharedThemesFolderName}/{theme}/Shared/{{0}}.cshtml",
                                $"/{options.SharedThemesFolderName}/{theme}/EmailTemplates/{{0}}.cshtml"
                            };

                            viewLocations = sharedThemeLocations.Concat(viewLocations);
                        }
                        
                    }
                    
                }
                
            }

            return viewLocations;
        }
        
    }
}
