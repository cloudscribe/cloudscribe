// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-07
//	Last Modified:              2015-10-07
//

using cloudscribe.Core.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using System;
using System.Globalization;

namespace cloudscribe.Core.Web.Razor
{
    public class TenantLayoutSelector : ILayoutSelector
    {
        public TenantLayoutSelector(
            IRazorViewEngine viewEngine,
            ISiteResolver siteResolver,
            IOptions<LayoutSelectorOptions> layoutOptionsAccesor,
            ILogger<TenantLayoutSelector> logger)
        {
            if (viewEngine == null) { throw new ArgumentNullException(nameof(viewEngine)); }
            if (siteResolver == null) { throw new ArgumentNullException(nameof(siteResolver)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (layoutOptionsAccesor == null) { throw new ArgumentNullException(nameof(layoutOptionsAccesor)); }

            this.viewEngine = viewEngine;
            this.siteResolver = siteResolver;
            options = layoutOptionsAccesor.Options;
            log = logger;
        }

        private IRazorViewEngine viewEngine;
        private ISiteResolver siteResolver;
        private ILogger log;
        private LayoutSelectorOptions options;

        public string GetLayoutName(ViewContext viewContext)
        {
            
            ISiteSettings site = siteResolver.Resolve();
            if (site == null) return options.DefaultLayout;

            string layout = options.DefaultLayout; //"_Layout"

            // resolve tenant specific layout file name
            
            if (options.SelectionMode == LayoutSelectionMode.Convention) // this is the default for now
            {
                // we could use a convention like Site1Layout.cshtml Site2Layout.cshtml
                // based on siteid
                layout = string.Format(CultureInfo.InvariantCulture, options.ConventionFormat, site.SiteId.ToInvariantString());
            }
            else
            {
                // we could store a layout name in ISiteSettings.Skin
                // in that case we would need a way to browse layout files from the UI
                // and tricky logic or conventions to filter to only layout files so no other views can be chosen
                // then if we do that would we need a way to make each tenant not be able to select
                // a layout that belongs to another tenant?
                // generally I think of multi-tenant for multiple sites owned by the same customer
                // so in that case it is not a big issue for one tenant to see the other tenant layouts
                // need a way to enumerate layout views in the file system to populate a dropdown

                // currently there is no ui for selecting a layout so you would have to set it in the db
                // in mp_Sites.Skin field
                if(site.Skin.Length > 0)
                {
                    layout = site.Skin.Replace(".cshtml", string.Empty);
                }
                

            }
            
            // in all cases we need to determine of the layout file exists
            // and if not log something and fallback to a known layout file

            var layoutPageResult = viewEngine.FindPage(viewContext, layout);
            if (layoutPageResult.Page == null)
            {
                log.LogError("could not find the layout " + layout);

                return options.DefaultLayout;
            }
            else
            {
                //log.LogInformation("found the layout " + layout);
                return layout;
            }

                
        }
    }
}
