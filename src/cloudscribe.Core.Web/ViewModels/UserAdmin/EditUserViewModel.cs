// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2017-06-09
// 
// TODO: support custom profile properties that are required for registration ?

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace cloudscribe.Core.Web.ViewModels.Account
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            AllTimeZones = new List<SelectListItem>();
            UserRoles = new List<string>();
            UserClaims = new List<Claim>();
        }
        
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid SiteId { get; set; } = Guid.Empty;

        public bool AccountApproved { get; set; }
        public bool SendApprovalEmail { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email has a maximum length of 100 characters")]
        [EmailAddress(ErrorMessage = "The email address does not appear as valid")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool IsLockedOut { get; set; }
        
        public bool Trusted { get; set; }


        //[Remote("UsernameAvailable", "Account", AdditionalFields = "UserId",  ErrorMessage = "Username not available, please try another value", HttpMethod = "Get")]
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username has a maximum length of 50 characters")]
        public string Username { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Display Name has a maximum length of 100 characters")]
        [Required(ErrorMessage = "Display Name is required")]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "First Name has a maximum length of 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Last Name has a maximum length of 100 characters")]
        public string LastName { get; set; } = string.Empty;
        
        public DateTime? DateOfBirth { get; set; }

        [Url(ErrorMessage = "You must provide a valid URL")]
        [StringLength(300, ErrorMessage = "Website Url has a maximum length of 300 characters")]
        public string WebSiteUrl { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string TimeZoneId { get; set; }

        public IEnumerable<SelectListItem> AllTimeZones { get; set; } = null;
        
        public string Comment { get; set; }

        public IList<string> UserRoles { get; set; }
        public IList<Claim> UserClaims { get; set; }
       
        
    }
}
