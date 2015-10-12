// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-06
// Last Modified:			2015-07-15
//

namespace cloudscribe.Web.Pagination
{
    public class PaginationLink
    {
        public bool Active { get; set; } = true;

        public bool IsCurrent { get; set; } = false;

        public int PageNumber { get; set; } = -1;

        public string Text { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public bool IsSpacer { get; set; } = false;
    }
}
