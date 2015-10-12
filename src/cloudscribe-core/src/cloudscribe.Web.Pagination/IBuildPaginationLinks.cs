// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-06
// Last Modified:			2015-07-15
//

using System;
using System.Collections.Generic;

namespace cloudscribe.Web.Pagination
{
    public interface IBuildPaginationLinks
    {
        List<PaginationLink> BuildPaginationLinks(
            PaginationSettings paginationSettings,
            Func<int, string> generateUrl,
            string firstPageText,
            string firstPageTitle,
            string previousPageText,
            string previousPageTitle,
            string nextPageText,
            string nextPageTitle,
            string lastPageText,
            string lastPageTitle,
            string spacerText = "...");

    }
}
