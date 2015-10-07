// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-07
//	Last Modified:              2015-10-07
//

using System;
using Microsoft.Framework.OptionsModel;
using Microsoft.AspNet.Mvc;

namespace cloudscribe.Core.Web.Razor
{
    public class FixedLayoutSelector : ILayoutSelector
    {
        public FixedLayoutSelector(IOptions<LayoutSelectorOptions> layoutOptionsAccesor)
        {
            if (layoutOptionsAccesor == null) { throw new ArgumentNullException(nameof(layoutOptionsAccesor)); }

            options = layoutOptionsAccesor.Options;
        }

        private LayoutSelectorOptions options;

        public string GetLayoutName(ViewContext viewContext)
        {
            return options.DefaultLayout;
        }
    }
}
