// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-07
//	Last Modified:              2015-11-18
//

using System;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace cloudscribe.Core.Web.Razor
{
    public class FixedLayoutSelector : ILayoutSelector
    {
        public FixedLayoutSelector(IOptions<LayoutSelectorOptions> layoutOptionsAccesor)
        {
            if (layoutOptionsAccesor == null) { throw new ArgumentNullException(nameof(layoutOptionsAccesor)); }

            options = layoutOptionsAccesor.Value;
        }

        private LayoutSelectorOptions options;

        public string GetLayoutName(ViewContext viewContext)
        {
            return options.DefaultLayout.Replace(".cshtml", string.Empty);
        }
    }
}
