// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-03-03
// 

//using cloudscribe.Configuration.DataAnnotations;
//using cloudscribe.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc.Rendering;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteBasicSettingsViewModel
    {
        public SiteBasicSettingsViewModel()
        {
            allTimeZones = new List<SelectListItem>();
            availableLayouts = new List<SelectListItem>();
        }

        private int siteID = -1;

        [Display(Name = "SiteId")]
        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        private Guid siteGuid = Guid.Empty;

        [Display(Name = "SiteGuid")]
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }

        }

        private string siteName = string.Empty;

        //[Required(ErrorMessageResourceName = "SiteNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "SiteName", ResourceType = typeof(CommonResources))]
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        private string siteFolderName = string.Empty;

        //[Display(Name = "SiteFolderName", ResourceType = typeof(CommonResources))]
        public string SiteFolderName
        {
            get { return siteFolderName; }
            set { siteFolderName = value; }
        }

        private string hostName = string.Empty;
        //[Display(Name = "PreferredHostName", ResourceType = typeof(CommonResources))]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private string timeZoneId = "Eastern Standard Time";

        [Required]
        //[Display(Name = "TimeZone", ResourceType = typeof(CommonResources))]
        public string TimeZoneId
        {
            get { return timeZoneId; }
            set { timeZoneId = value; }
        }

        public string Theme { get; set; } = string.Empty;

        private List<SelectListItem> availableLayouts = null;
        public List<SelectListItem> AvailableLayouts
        {
            get { return availableLayouts; }
            set { availableLayouts = value; }
        }

        private IEnumerable<SelectListItem> allTimeZones = null;
        public IEnumerable<SelectListItem> AllTimeZones
        {
            get { return allTimeZones; }
            set { allTimeZones = value; }
        }
        
        private int returnPageNumber = 1;

        public int ReturnPageNumber
        {
            get { return returnPageNumber; }
            set { returnPageNumber = value; }
        }

        private bool showDelete = false;

        public bool ShowDelete
        {
            get { return showDelete; }
            set { showDelete = value; }
        }
        
        //[AllowHtml]
        //[Display(Name = "ClosedMessage", ResourceType = typeof(CommonResources))]
        public string ClosedMessage { get; set; } = string.Empty;
        
        //[Display(Name = "IsClosed", ResourceType = typeof(CommonResources))]
        public bool IsClosed { get; set; } = false;
        
        
    }
}
