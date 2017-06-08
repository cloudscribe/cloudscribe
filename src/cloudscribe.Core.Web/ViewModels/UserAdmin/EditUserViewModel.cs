// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2017-06-08
// 
// TODO: support custom profile properties that are required for registration ?

using System;
using System.ComponentModel.DataAnnotations;
//using cloudscribe.Web.Common.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cloudscribe.Core.Web.ViewModels.Account
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            AllTimeZones = new List<SelectListItem>();
            UserClaims = new List<Claim>();
        }
        
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid SiteId { get; set; } = Guid.Empty;

        public bool AccountApproved { get; set; }
        public bool SendApprovalEmail { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The email address does not appear as valid")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool IsLockedOut { get; set; }
        
        public bool Trusted { get; set; }

       
        [Remote("UsernameAvailable", "Account", AdditionalFields = "UserId",
           ErrorMessage = "Username not available, please try another value",
           HttpMethod = "Post")]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;
        
       
        [Required(ErrorMessage = "Display name is required")]
        public string DisplayName { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        
        
        public string LastName { get; set; } = string.Empty;
        
        public DateTime? DateOfBirth { get; set; }
        
        public DateTime? LastLoginDate { get; set; }

        public string TimeZoneId { get; set; }

        public IEnumerable<SelectListItem> AllTimeZones { get; set; } = null;
        
        public string Comment { get; set; }

        public IList<Claim> UserClaims { get; set; }

        public string DropFileUrl { get; set; } = "/filemanager/upload";

        public string FileBrowseUrl { get; set; } = "/filemanager/ckfiledialog?type=file";

        public string ImageBrowseUrl { get; set; } = "/filemanager/ckfiledialog?type=image";


    }
}
