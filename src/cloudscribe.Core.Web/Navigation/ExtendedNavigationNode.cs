//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2015-07-07
//// Last Modified:			2015-07-15
//// 

////TODO: this file probably should be part of cms not core
//// cms will have its own CmsNavigationTreeBuilder and it could use this class
//// but we are not using it here in core

//using cloudscribe.Web.Navigation;
//using System.ComponentModel;
//using Newtonsoft.Json;
//using System;

//namespace cloudscribe.Core.Web.Navigation
//{
//    //http://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp


//    public class ExtendedNavigationNode : NavigationNode, 
//        //INavigationNodeSeoSettings, 
//        INavigationNodeEditPermissionMeta,
//        INavigationNodeDesignMeta,
//        INavigationNodePublisingMeta
//    {

//        // these props were migrated from mojoportal we may not need all of them and may remove some later

//        //INavigationNodeSeoSettings
//        //[JsonIgnore]
//        //public PageChangeFrequency ChangeFrequency { get; set; } = PageChangeFrequency.Daily;

//        [JsonIgnore]
//        public string SiteMapPriority { get; set; } = "0.5";

//        //INavigationNodeEditPermissionMeta
//        [DefaultValue("")]
//        public string CreateChildPageRoles { get; set; } = string.Empty;

//        [DefaultValue("")]
//        public string DraftEditRoles { get; set; } = string.Empty;

//        [DefaultValue("")]
//        public string EditRoles { get; set; } = string.Empty;

//        // INavigationNodeDesignMeta
//        [DefaultValue("")]
//        public string DepthIndicator { get; set; } = string.Empty;

//        [DefaultValue(true)]
//        public bool ExpandOnSiteMap { get; set; } = true;

//        [DefaultValue(false)]
//        public bool HideAfterLogin { get; set; } = false;

//        [DefaultValue(true)]
//        public bool IncludeInMenu { get; set; } = true;

//        [DefaultValue(true)]
//        public bool IncludeInSiteMap { get; set; } = true;

//        [DefaultValue(true)]
//        public bool IncludeInChildSiteMap { get; set; } = true;

//        [DefaultValue(true)]
//        public bool IncludeInSearchEngineSiteMap { get; set; } = true;

//        [DefaultValue(true)]
//        public bool IsClickable { get; set; } = true;

//        [DefaultValue("")]
//        public string LinkRel { get; set; } = string.Empty;

//        [DefaultValue("")]
//        public string MenuImageUrl { get; set; } = string.Empty;

//        [DefaultValue("")]
//        public string MenuCssClass { get; set; } = string.Empty;

//        [DefaultValue("")]
//        public string MenuDescription { get; set; } = string.Empty;

//        [DefaultValue(false)]
//        public bool OpenInNewWindow { get; set; } = false;


//        // INavigationNodePublisingMeta
//        [DefaultValue(-1)]
//        public int PageId { get; set; } = -1;

//        [JsonIgnore]
//        public Guid PageGuid { get; set; } = Guid.Empty;

//        [DefaultValue(-1)]
//        public int ParentId { get; set; } = -1;

//        [DefaultValue(false)]
//        public bool IsPending { get; set; } = false;

//        [DefaultValue(0)]
//        public int PublishMode { get; set; } = 0; // 0=All 1=desktopwebonnly 2=phonewebonly

//        [JsonIgnore]
//        public DateTime PubDateUtc { get; set; } = DateTime.UtcNow;

//        [JsonIgnore]
//        public DateTime LastModifiedUtc { get; set; } = DateTime.MinValue;


//    }
//}
