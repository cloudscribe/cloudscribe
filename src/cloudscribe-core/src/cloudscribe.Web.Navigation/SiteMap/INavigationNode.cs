// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-09
// Last Modified:			2015-07-15
// 

using System;

namespace cloudscribe.Web.Navigation
{
    public interface INavigationNode
    {
        string Key { get; set; }
        string ParentKey { get; set; }
        string Text { get; set; }
        string Title { get; set; }
        string Url { get; set; } 
        string Controller { get; set; } 
        string Action { get; set; }
        //could we add route param dictionary?
        bool IsRootNode { get; set; }
    }

    public interface INavigationNodeLocalization
    {
        string ResourceName { get; set; } 
        string ResourceTextKey { get; set; }
        string ResourceTitleKey { get; set; }
    }

    public interface INavigationNodeRenderSettings
    {
        bool IncludeAmbientValuesInUrl { get; set; }
        string PreservedRouteParameters { get; set; }
        string ComponentVisibility { get; set; }
        string ViewRoles { get; set; }
    }

    public interface INavigationNodeSeoSettings
    {
        PageChangeFrequency ChangeFrequency { get; set; } 
        string SiteMapPriority { get; set; }
    }

    public interface INavigationNodeEditPermissionMeta
    {
        string CreateChildPageRoles { get; set; } 
        string DraftEditRoles { get; set; }
        string EditRoles { get; set; }
    }

    public interface INavigationNodeDesignMeta
    {
        string DepthIndicator { get; set; } 
        bool ExpandOnSiteMap { get; set; } 
        bool HideAfterLogin { get; set; }
        bool IncludeInMenu { get; set; } 
        bool IncludeInSiteMap { get; set; }
        bool IncludeInChildSiteMap { get; set; } 
        bool IncludeInSearchEngineSiteMap { get; set; }
        bool IsClickable { get; set; }
        string LinkRel { get; set; }
        string MenuImageUrl { get; set; }
        string MenuCssClass { get; set; }
        string MenuDescription { get; set; }
        bool OpenInNewWindow { get; set; }
    }

    public interface INavigationNodePublisingMeta
    {
        int PageId { get; set; } 
        Guid PageGuid { get; set; }
        int ParentId { get; set; }

        bool IsPending { get; set; }
        int PublishMode { get; set; }  // 0=All 1=desktopwebonnly 2=phonewebonly
        DateTime PubDateUtc { get; set; } 
        DateTime LastModifiedUtc { get; set; }
    }

}
