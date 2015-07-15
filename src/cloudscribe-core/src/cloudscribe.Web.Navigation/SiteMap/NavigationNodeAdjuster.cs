// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-12
// Last Modified:			2015-07-15
// 

using Microsoft.AspNet.Http;

namespace cloudscribe.Web.Navigation
{
    public class NavigationNodeAdjuster
    {
        public NavigationNodeAdjuster(HttpContext context)
        {
            this.context = context;
        }

        public const string KeyPrefix = "nav-adjuster-";

        private HttpContext context;

        public string KeyToAdjust { get; set; } = string.Empty;

        public string AdjustedText { get; set; } = string.Empty;

        public string AdjustedUrl { get; set; } = string.Empty;

        public bool AdjustRemove { get; set; } = false;

        /// <summary>
        /// breadcrumb adjustment is the most common  expected usage scenario so that is the default value
        /// but other named navigation filters could be adjusted by changing the ViewFilterName here
        /// and adding Model.Adjust* logic in the related razor file
        /// </summary>
        public string ViewFilterName { get; set; } = NamedNavigationFilters.Breadcrumbs;

        public void AddToContext()
        {
            if(string.IsNullOrWhiteSpace(KeyToAdjust)) { return; }
            string key = KeyPrefix + KeyToAdjust;
            if (context.Items[key] == null)
            {
                context.Items[key] = this;
            }
        }
    }
}
