//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
////  Author:                     Joe Audette
////  Created:                    2015-10-07
////	Last Modified:              2016-02-04
////

//using cloudscribe.Core.Models;
//using cloudscribe.Web.Common.Razor;
//using Microsoft.AspNet.Mvc.Rendering;
//using Microsoft.AspNet.Mvc.Razor;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.OptionsModel;
//using System;
//using System.Globalization;

//namespace cloudscribe.Core.Web.Razor
//{
//    public class TenantLayoutSelector : ILayoutSelector
//    {
//        public TenantLayoutSelector(
//            IRazorViewEngine viewEngine,
//            SiteSettings currentSite,
//            IOptions<LayoutSelectorOptions> layoutOptionsAccesor,
//            ILogger<TenantLayoutSelector> logger)
//        {
//            if (viewEngine == null) { throw new ArgumentNullException(nameof(viewEngine)); }
//            if (currentSite == null) { throw new ArgumentNullException(nameof(currentSite)); }
//            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
//            if (layoutOptionsAccesor == null) { throw new ArgumentNullException(nameof(layoutOptionsAccesor)); }

//            this.viewEngine = viewEngine;
//            site = currentSite;
//            options = layoutOptionsAccesor.Value;
//            log = logger;
//        }

//        private IRazorViewEngine viewEngine;
//        private ISiteSettings site = null;
//        private ILogger log;
//        private LayoutSelectorOptions options;

//        public string GetLayoutName(ViewContext viewContext)
//        {
//            //ISiteSettings site = siteResolver.Resolve();
//            if (site == null) return options.DefaultLayout;

//            string layout = options.DefaultLayout.Replace(".cshtml", string.Empty); // "Default_Layout"

//            // resolve tenant specific layout file name
            
//            if (options.SelectionMode == LayoutSelectionMode.Convention) 
//            {
//                // with this mode layouts are not shown in a dropdown list in site settings
//                // so the layout cannot be changed from the UI
//                // use a convention like Site1Layout.cshtml Site2Layout.cshtml
//                // based on siteid
//                layout = string.Format(CultureInfo.InvariantCulture, 
//                    options.ConventionFormat, 
//                    site.SiteId.ToInvariantString());
//            }
//            else
//            {
//                // LayoutSelectionMode.Browsing -- this is the default
//                // in this mode a dropdown list of available layouts is shown
//                // and the layout can be chosen from the UI
//                // the list is filtered per tenant using file naming conventions
//                // where the SiteID is part of the filename format
//                // ie you could name files like this:
//                // Site1_dark_Layout.cshtml
//                // Site1_light_Layout.cshtml
//                // Site2_supercool_Layout.cshtml
//                // Site2_autumn_Layout.cshtml
//                // ...
                
//                if (site.Layout.Length > 0)
//                {
//                    layout = site.Layout.Replace(".cshtml", string.Empty);
//                }
                
//            }
            
//            // in all cases we need to determine of the layout file exists
//            // and if not log something and fallback to a known layout file

//            var layoutPageResult = viewEngine.FindPage(viewContext, layout);
//            if (layoutPageResult.Page == null)
//            {
//                log.LogError("could not find the layout " + layout);

//                return options.DefaultLayout.Replace(".cshtml", string.Empty);
//            }
            
//            log.LogDebug("using the layout " + layout);
//            return layout;
            

                
//        }
//    }
//}
