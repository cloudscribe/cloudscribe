// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-06-06
// 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    // https://docs.asp.net/en/latest/fundamentals/localization.html#dataannotations-localization
    // validation error messages can be localized by adding
    // ViewModels.SiteSetting.SiteBasicSettingsViewModel.fr.resx
    // but DisplayName cannot for some reason
    // so my advice is just use IStringLocalizer within the views to localize labels
    // it would be possible to use static resources with resource type but then it would be difficult 
    // for people to override or add their own translation resx files

    public class SiteBasicSettingsViewModel
    {
        public SiteBasicSettingsViewModel()
        {
            allTimeZones = new List<SelectListItem>();
            availableLayouts = new List<SelectListItem>();
        }

        private Guid id = Guid.Empty;
        public Guid SiteId
        {
            get { return id; }
            set { id = value; }

        }

        private string siteName = string.Empty;

        [Required(ErrorMessage = "Site Name is required")]
        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        public string SiteName
        {
            get { return siteName; }
            set { siteName = value; }
        }

        private string aliasId = string.Empty;
        /// <summary>
        /// no spaces, only chars that are good for folder names
        /// this is an alternate identifier for a site whose main purpose
        /// is so we don't have to use an ugly guid string for folder name
        /// to host site specific files such as themes
        /// </summary>
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "only digits, numbers, - and _ allowed, no spaces allowed"), StringLength(36)]
        public string AliasId
        {
            get { return aliasId; }
            set { aliasId = value; }
        }

        private string siteFolderName = string.Empty;
        public string SiteFolderName
        {
            get { return siteFolderName; }
            set { siteFolderName = value; }
        }

        private string hostName = string.Empty;
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private string timeZoneId = "America/New_York";

        [Required(ErrorMessage = "Time zone is required")]
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
        public string ClosedMessage { get; set; } = string.Empty;
        
        public bool IsClosed { get; set; } = false;
        
        
    }
}
