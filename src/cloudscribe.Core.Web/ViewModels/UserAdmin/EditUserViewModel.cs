// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2016-06-06
// 
// TODO: support custom profile properties that are required for registration ?

using System;
using System.ComponentModel.DataAnnotations;
//using cloudscribe.Web.Common.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
//using ExpressiveAnnotations.Attributes;

namespace cloudscribe.Core.Web.ViewModels.Account
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            allTimeZones = new List<SelectListItem>();
        }
        

        public Guid UserId { get; set; } = Guid.Empty;
        public Guid SiteId { get; set; } = Guid.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool IsLockedOut { get; set; }
        public bool AccountApproved { get; set; }
        public bool Trusted { get; set; }

        //  //[StringLength(AppSettings.PasswordMaxLength, MinimumLength = AppSettings.PasswordMinLength, ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //  //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[RequiredWhen("UserId", -1, ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLengthWithConfig(
        //    MinimumLength = 7,
        //    MaximumLength = 100,
        //    MinLengthKey = "PasswordMinLength",
        //    MaxLengthKey = "PasswordMaxLength",
        //    ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[RequiredWhen("UserId", -1,AllowEmptyStrings =false, ErrorMessage = "Password is required.")]
        //[Required(AllowEmptyStrings = false)]
        //[DataType(DataType.Password)]
        // public string Password { get; set; }

        //DataType(DataType.Password)]
        //[CompareWhen(WhenProperty = "UserId", WhenValue = -1, CompareProperty = "Password",ErrorMessage = "Confirm Password must match Password.")]
        //[Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMatchErrorMessage", ErrorMessageResourceType = typeof(CommonResources))]
        //public string ConfirmPassword { get; set; }



        // //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Remote("LoginNameAvailable", "Account", AdditionalFields = "UserId",
        //    ErrorMessageResourceName = "LoginNameNotAvailable", ErrorMessageResourceType = typeof(CommonResources),
        //    HttpMethod = "Post")]
        [Required(ErrorMessage = "Login name is required")]
        public string LoginName { get; set; } = string.Empty;
        
        private string displayName = string.Empty;

        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        
        [Required(ErrorMessage = "Display name is required")]
        public string DisplayName { get; set; } = string.Empty;
        
        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
     
        public string FirstName { get; set; } = string.Empty;
        
        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        public string LastName { get; set; } = string.Empty;
        
        public DateTime? DateOfBirth { get; set; }
        
        public DateTime? LastLoginDate { get; set; }

        public string TimeZoneId { get; set; }

        private IEnumerable<SelectListItem> allTimeZones = null;
        public IEnumerable<SelectListItem> AllTimeZones
        {
            get { return allTimeZones; }
            set { allTimeZones = value; }
        }

        public string Comment { get; set; }


    }
}
