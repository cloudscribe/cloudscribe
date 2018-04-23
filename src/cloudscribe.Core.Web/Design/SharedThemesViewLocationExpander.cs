// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2016-11-14
//	Last Modified:              2016-11-14
//

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Core.Web.Components
{
    /// <summary>
    /// this is an alternative to SiteViewLocationExpander, that one keeps themese separate per tenant, this one does not.
    /// If you use this, you should also use SharedThemeListBuilder which populates the dropdown list of themes.
    /// </summary>
    public class SharedThemesViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "theme";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            context.Values[THEME_KEY]
                = context.ActionContext.HttpContext.GetTenant<SiteContext>()?.Theme;

        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            string theme = null;

            if (context.Values.TryGetValue(THEME_KEY, out theme))
            {
                IEnumerable<string> themeLocations = new[]
                {
                    $"/Themes/{theme}/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Shared/{{0}}.cshtml",
                    $"/Themes/{theme}/EmailTemplates/{{0}}.cshtml"
                };

                viewLocations = themeLocations.Concat(viewLocations);
            }

            return viewLocations;
        }
  
    }
}
