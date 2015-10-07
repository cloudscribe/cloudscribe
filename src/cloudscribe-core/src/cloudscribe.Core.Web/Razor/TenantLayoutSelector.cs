// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-07
//	Last Modified:              2015-10-07
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.Razor
{
    public class TenantLayoutSelector : ILayoutSelector
    {
        public TenantLayoutSelector(
            ISiteResolver siteResolver,
            IOptions<LayoutSelectorOptions> layoutOptionsAccesor,
            ILogger<TenantLayoutSelector> logger)
        {
            if (siteResolver == null) { throw new ArgumentNullException(nameof(siteResolver)); }
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (layoutOptionsAccesor == null) { throw new ArgumentNullException(nameof(layoutOptionsAccesor)); }

            this.siteResolver = siteResolver;
            log = logger;
        }

        private ISiteResolver siteResolver;
        private ILogger log;
        private LayoutSelectorOptions options;

        public string GetLayoutName()
        {
            //TODO: resolve tenant specific layout file name
            // we could use a convention like Site1Layout.cshtml Site2Layout.cshtml
            // based on siteid

            //or we could store a layout name in ISiteSettings.Skin
            // in that case we would need a way to browse layout files from the UI
            // and tricky logic or conventions to filter to only layout files so no other views can be chosen
            // then if we do that would we need a way to make each tenant not be able to select
            // a layout that belongs to another tenant?
            // generally I think of multi-tenant for multiple sites owned by the same customer
            // so in that case it is not a big issue for one tenant to see the other tenant layouts

            // maybe we should implement more than one solution
            // ie one can configure either a convention based approach which would not allow browsing
            // layouts and could be used to better isolate tenants
            // or they can configure one that stores the layout name in sitesettings
            // and allows browsing of files named **layout**.cshml
            // which could be more useful when the isolation is not as important

            // in all cases we need a way to determine of the layout file exists
            // and if not log something and fallback to a known layout file

            return options.DefaultLayout;
        }
    }
}
