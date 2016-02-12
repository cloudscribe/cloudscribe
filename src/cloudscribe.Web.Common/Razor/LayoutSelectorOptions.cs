// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2015-10-07
//	Last Modified:              2015-10-07
//


namespace cloudscribe.Web.Common.Razor
{
    public class LayoutSelectorOptions
    {
        public string DefaultLayout { get; set; } = "Default_Layout.cshtml";
        public LayoutSelectionMode SelectionMode { get; set; } = LayoutSelectionMode.Browsing;
        public string ConventionFormat { get; set; } = "Site{0}Layout";
        public string BrowseFilterFormat { get; set; } = "Site{0}_*_Layout.cshtml";
    }

    public enum LayoutSelectionMode
    {
        Convention,
        Browsing
    }
}
