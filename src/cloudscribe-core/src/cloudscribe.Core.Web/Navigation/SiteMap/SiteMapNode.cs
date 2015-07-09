// Author:					Joe Audette
// Created:					2015-07-07
// Last Modified:			2015-07-07
// 


using cloudscribe.Core.Web.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Navigation
{
    //http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp


    public class SiteMapNode
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Controller { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;

        // these were useful props in mvcsitemapprovider 
        public bool IncludeAmbientValuesInUrl { get; set; } = true;
        public string PreservedRouteParameters { get; set; } = string.Empty;
        public string ComponentVisibility { get; set; } = string.Empty;
        public string ResourceName { get; set; } = string.Empty;
        public string ResourceKey { get; set; } = string.Empty;


        // these props were migrated from mojoportal we may not need all of them and may remove some later

        public PageChangeFrequency ChangeFrequency { get; set; } = PageChangeFrequency.Daily;
        public string CreateChildPageRoles { get; set; } = string.Empty;
        public string DepthIndicator { get; set; } = string.Empty;
        public string DraftEditRoles { get; set; } = string.Empty;
        public string EditRoles { get; set; } = string.Empty;
        public bool ExpandOnSiteMap { get; set; } = true;
        public bool HideAfterLogin { get; set; } = false;
        public bool IncludeInMenu { get; set; } = true;
        public bool IncludeInSiteMap { get; set; } = true;
        public bool IncludeInChildSiteMap { get; set; } = true;
        public bool IncludeInSearchEngineSiteMap { get; set; } = true;
        public bool IsClickable { get; set; } = true;
        public bool IsPending { get; set; } = false;
        public bool IsRootNode { get; set; } = false;
        public DateTime LastModifiedUtc { get; set; } = DateTime.MinValue;
        public string LinkRel { get; set; } = string.Empty;
        public string MenuImageUrl { get; set; } = string.Empty;
        public string MenuCssClass { get; set; } = string.Empty;
        public string MenuDescription { get; set; } = string.Empty;
        public bool OpenInNewWindow { get; set; } = false;
        public int PageId { get; set; } = -1;
        public Guid PageGuid { get; set; } = Guid.Empty;
        public int ParentId { get; set; } = -1;
        public DateTime PubDateUtc { get; set; } = DateTime.UtcNow;
        public int PublishMode { get; set; } = 0; // 0=All 1=desktopwebonnly 2=phonewebonly
        public string SiteMapPriority { get; set; } = "0.5";
        public string ViewRoles { get; set; } = string.Empty;
        

    }
}
