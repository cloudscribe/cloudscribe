// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-09
//	Last Modified:              2015-10-09
//

namespace cloudscribe.Web.Common.Razor
{
    public class TenantLayoutFileDisplayNameFilter : ILayoutFileDisplayNameFilter
    {
        public string FilterDisplayName(string tenantId, string layoutFileName)
        {
            string frontSegment = "Site" + tenantId + "_";
            string endsegment = "_Layout.cshtml";
            return layoutFileName.Replace(frontSegment, string.Empty).Replace(endsegment, string.Empty);
        }
    }
}
