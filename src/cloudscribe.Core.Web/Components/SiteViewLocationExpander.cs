// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2016-03-03
//	Last Modified:              2016-03-03
//

using cloudscribe.Core.Models;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SiteViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "theme", TENANT_KEY = "tenant";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values[THEME_KEY]
                = context.ActionContext.HttpContext.GetTenant<SiteSettings>()?.Layout;

            context.Values[TENANT_KEY]
                = "tenant-" + context.ActionContext.HttpContext.GetTenant<SiteSettings>()?.SiteId.ToInvariantString();
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string tenant = null;
            string theme = null;

            if (context.Values.TryGetValue(THEME_KEY, out theme))
            {
                if (context.Values.TryGetValue(TENANT_KEY, out tenant))
                {
                    IEnumerable<string> themeLocations = new[]
                    {
                        $"/tenantfiles/{tenant}/themes/{theme}/{{1}}/{{0}}.cshtml",
                        $"/tenantfiles/{tenant}/themes/{theme}/Shared/{{0}}.cshtml"
                    };

                    viewLocations = themeLocations.Concat(viewLocations);
                }
                
            }

            return viewLocations;
        }

        private IEnumerable<string> ExpandTenantLocations(string tenant, IEnumerable<string> defaultLocations)
        {
            foreach (var location in defaultLocations)
            {
                yield return location.Replace("{0}", $"{{0}}_{tenant}");
                yield return location;
            }
        }

    }
}
