// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-13
// 


using System;

namespace cloudscribe.Core.Web.Navigation
{
    //http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp


    public class ExtendedNavigationNode : NavigationNode, 
        INavigationNodeSeoSettings, 
        INavigationNodeEditPermissionMeta,
        INavigationNodeDesignMeta,
        INavigationNodePublisingMeta
    {

        // these props were migrated from mojoportal we may not need all of them and may remove some later

        //INavigationNodeSeoSettings
        public PageChangeFrequency ChangeFrequency { get; set; } = PageChangeFrequency.Daily;
        public string SiteMapPriority { get; set; } = "0.5";

        //INavigationNodeEditPermissionMeta
        public string CreateChildPageRoles { get; set; } = string.Empty;
        public string DraftEditRoles { get; set; } = string.Empty;
        public string EditRoles { get; set; } = string.Empty;

        // INavigationNodeDesignMeta
        public string DepthIndicator { get; set; } = string.Empty;
        public bool ExpandOnSiteMap { get; set; } = true;
        public bool HideAfterLogin { get; set; } = false;
        public bool IncludeInMenu { get; set; } = true;
        public bool IncludeInSiteMap { get; set; } = true;
        public bool IncludeInChildSiteMap { get; set; } = true;
        public bool IncludeInSearchEngineSiteMap { get; set; } = true;
        public bool IsClickable { get; set; } = true;
        public string LinkRel { get; set; } = string.Empty;
        public string MenuImageUrl { get; set; } = string.Empty;
        public string MenuCssClass { get; set; } = string.Empty;
        public string MenuDescription { get; set; } = string.Empty;
        public bool OpenInNewWindow { get; set; } = false;


        // INavigationNodePublisingMeta
        public int PageId { get; set; } = -1;
        public Guid PageGuid { get; set; } = Guid.Empty;
        public int ParentId { get; set; } = -1;

        public bool IsPending { get; set; } = false;
        public int PublishMode { get; set; } = 0; // 0=All 1=desktopwebonnly 2=phonewebonly
        public DateTime PubDateUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedUtc { get; set; } = DateTime.MinValue;


    }
}
