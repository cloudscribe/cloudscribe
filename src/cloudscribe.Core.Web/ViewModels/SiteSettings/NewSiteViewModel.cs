// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-11
// Last Modified:			2016-06-11
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

        // TODO: need to revive the RequiredWhen attribute
        // site folder name should be required when creating new sites in multitenat mode folder

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

        // info for new site admin
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "invalid email format")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        //[Display(Name = "Password", ResourceType = typeof(CommonResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match password")]
        public string ConfirmPassword { get; set; }

        // http://stackoverflow.com/questions/36033022/using-remote-validation-wit-asp-net-core
        // //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        
        [Required(ErrorMessage = "Login Name is required.")]
        public string LoginName { get; set; } = string.Empty;

        // TODO do we want unique display names?
        // people can have the same real names like John Smith and why should we prevent anyone from using their real name?
        // not that we can be sure they are telling the truth about anything

        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Required(ErrorMessage = "Display Name is required.")]
        public string DisplayName { get; set; } = string.Empty;

    }
}
