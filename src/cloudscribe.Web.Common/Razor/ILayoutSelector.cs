// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-07
//	Last Modified:              2015-10-17
//

using Microsoft.AspNet.Mvc.Rendering;

namespace cloudscribe.Web.Common.Razor
{
    public interface ILayoutSelector
    {
        string GetLayoutName(ViewContext viewContext);
    }
}
