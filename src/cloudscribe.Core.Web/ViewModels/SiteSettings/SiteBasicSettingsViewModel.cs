// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-10-26
// Last Modified:			2016-06-12
// 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    // https://docs.asp.net/en/latest/fundamentals/localization.html#dataannotations-localization
    // validation error messages can be localized by adding
    // ViewModels.SiteSetting.SiteBasicSettingsViewModel.fr.resx
    // but [DisplayName] cannot for some reason
    // so my advice is just use IStringLocalizer within the views to localize labels
    // it would be possible to use static resources with resource type but then it would be difficult 
    // for people to override or add their own translation resx files

    public class SiteBasicSettingsViewModel
    {
        public SiteBasicSettingsViewModel()
        {
            allTimeZones = new List<SelectListItem>();
            availableThemes = new List<SelectListItem>();
        }

        private Guid id = Guid.Empty;
        public Guid SiteId
        {
            get { return id; }
            set { id = value; }

        }
        
        [Required(ErrorMessage = "Site Name is required")]
        [StringLength(255,ErrorMessage ="Site Name has a maximum length of 255 characters")]
        public string SiteName { get; set; }



        /// <summary>
        /// no spaces, only chars that are good for folder names
        /// this is an alternate identifier for a site whose main purpose
        /// is so we don't have to use an ugly guid string for folder name
        /// to host site specific files such as themes
        /// TODO: need to validate this so that one site cannaot change their aliasid to match a different site
        /// and thus steal use of their themes, unless multi-tenant options allow it
        /// </summary>
        [Remote("AliasIdAvailable", "SiteAdmin", AdditionalFields = "SiteId",
           ErrorMessage = "AliasId not available, please try another value",
           HttpMethod = "Post")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "For AliasId, only digits, numbers, - and _ allowed, no spaces allowed")]
        [StringLength(36, ErrorMessage = "AliasId has a maximum length of 36 characters")]
        public string AliasId { get; set; }


        [Remote("FolderNameAvailable", "SiteAdmin", AdditionalFields = "SiteId",
           ErrorMessage = "Requested Site Folder Name is not available, please try another value",
           HttpMethod = "Post")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "For Site Folder Name, only digits, numbers, - and _ allowed, no spaces allowed")]
        [StringLength(50, ErrorMessage ="Site Folder name has a maximum length of 50 characters")]
        public string SiteFolderName { get; set; }
        
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "For Host name, only digits, numbers, - and _ allowed, no spaces allowed")]
        [StringLength(255, ErrorMessage = "Host name has a maximum length of 255 characters")]
        public string HostName { get; set; }
        
        [Required(ErrorMessage = "Time zone is required")]
        public string TimeZoneId { get; set; } = "America/New_York";
        
        public string Theme { get; set; } = string.Empty;

        private List<SelectListItem> availableThemes = null;
        public List<SelectListItem> AvailableThemes
        {
            get { return availableThemes; }
            set { availableThemes = value; }
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
        
        public string ClosedMessage { get; set; } = string.Empty;
        
        public bool IsClosed { get; set; } = false;
        
        
    }
}
