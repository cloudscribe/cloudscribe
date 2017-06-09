// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-11
// Last Modified:			2017-06-08
// 

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class NewSiteViewModel
    {
        public NewSiteViewModel()
        {
            AllTimeZones = new List<SelectListItem>();
            AvailableThemes = new List<SelectListItem>();
        }

        public Guid SiteId { get; set; } = Guid.Empty;
        
        [Required(ErrorMessage = "Site Name is required")]
        [StringLength(255, ErrorMessage = "Site Name has a maximum length of 255 characters")]
        public string SiteName { get; set; }
        
        /// <summary>
        /// no spaces, only chars that are good for folder names
        /// this is an alternate identifier for a site whose main purpose
        /// is so we don't have to use an ugly guid string for folder name
        /// to host site specific files such as themes
        /// </summary>
        [Remote("AliasIdAvailable", "SiteAdmin", AdditionalFields = "SiteId",
           ErrorMessage = "AliasId not available, please try another value",
           HttpMethod = "Post")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "only digits, numbers, - and _ allowed, no spaces allowed")]
        [StringLength(36, ErrorMessage = "AliasId has a maximum length of 36 characters")]
        public string AliasId { get; set; } = string.Empty;


        // TODO: need to revive the RequiredWhen attribute
        // site folder name should be required when creating new sites in multitenant mode folder
        // ? or just enforce with server side or let it be empty and it won't work until populated

        [Remote("FolderNameAvailable", "SiteAdmin", AdditionalFields = "SiteId",
           ErrorMessage = "Requested Site Folder Name is not available, please try another value",
           HttpMethod = "Post")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "For Site Folder Name, only digits, numbers, - and _ allowed, no spaces allowed")]
        [StringLength(50, ErrorMessage = "Site Folder name has a maximum length of 50 characters")]
        public string SiteFolderName { get; set; } = string.Empty;

        [Remote("HostNameAvailable", "SiteAdmin", AdditionalFields = "SiteId",
           ErrorMessage = "Requested Host name is not available",
           HttpMethod = "Post")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "For Host name, only digits, numbers, - and _ allowed, no spaces allowed")]
        [StringLength(255, ErrorMessage = "Host name has a maximum length of 255 characters")]
        public string HostName { get; set; } = string.Empty;
        
        public string Theme { get; set; } = string.Empty;

        public List<SelectListItem> AvailableThemes { get; set; } = null;

        [Required(ErrorMessage = "Time zone is required")]
        public string TimeZoneId { get; set; } = "America/New_York";

        public IEnumerable<SelectListItem> AllTimeZones { get; set; } = null;
        
        public int ReturnPageNumber { get; set; } = 1;
        
        public string ClosedMessage { get; set; } = string.Empty;

        public bool IsClosed { get; set; } = false;

        // info for new site admin
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "invalid email format")]
        [StringLength(100, ErrorMessage = "Email has a maximum length of 100 characters")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match password")]
        public string ConfirmPassword { get; set; }

        
        [Required(ErrorMessage = "Login Name is required.")]
        [StringLength(50, ErrorMessage = "Login name has a maximum length of 50 characters")]
        public string LoginName { get; set; } = string.Empty;

        // TODO: do we want unique display names?
        // people can have the same real names like John Smith and why should we prevent anyone from using their real name?
        // not that we can be sure they are telling the truth about anything
        // TODO: display name is prone to abuse, like using offensive language
        // would be nice to implement validation and remote validation
        // with a way to plugin a list of rules that can evaluate the requested displayname and reject it
        // but this problem can also be mitigated by an admin editing the display name
        
        
        [Required(ErrorMessage = "Display Name is required.")]
        [StringLength(100, ErrorMessage = "Display name has a maximum length of 100 characters")]
        public string DisplayName { get; set; } = string.Empty;

        

    }
}
