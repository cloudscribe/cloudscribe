// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-09
//	Last Modified:              2016-06-12
//

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace cloudscribe.Web.Common.Razor
{
    public interface IThemeListBuilder
    {
        List<SelectListItem> GetAvailableThemes(string aliasId = null);
    }
}
