// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2016-01-28
// 
// TODO: support custom profile properties that are required for registration

using System;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Web.Common.DataAnnotations;
//using ExpressiveAnnotations.Attributes;

namespace cloudscribe.Core.Web.ViewModels.Account
{
    public class EditUserViewModel
    {
       
        [Display(Name = "UserId")]
        public int UserId { get; set; } = -1;
        
        [Display(Name = "UserGuid")]
        public Guid UserGuid { get; set; } = Guid.Empty;
        
        [Display(Name = "SiteGuid")]
        public Guid SiteGuid { get; set; } = Guid.Empty;
        
        [Required]
        [EmailAddress(ErrorMessage = "invalid email format")]
        //[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[EmailAddress(ErrorMessage = "", ErrorMessageResourceName = "EmailNotValid", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "Email", ResourceType = typeof(CommonResources))]
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
        //[Display(Name = "Password", ResourceType = typeof(CommonResources))]
       // public string Password { get; set; }

        //DataType(DataType.Password)]
        //[CompareWhen(WhenProperty = "UserId", WhenValue = -1, CompareProperty = "Password",ErrorMessage = "Confirm Password must match Password.")]
        //[Display(Name = "ConfirmPassword", ResourceType = typeof(CommonResources))]
        //[Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMatchErrorMessage", ErrorMessageResourceType = typeof(CommonResources))]
        //public string ConfirmPassword { get; set; }


        
        // //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Remote("LoginNameAvailable", "Account", AdditionalFields = "UserId",
        //    ErrorMessageResourceName = "LoginNameNotAvailable", ErrorMessageResourceType = typeof(CommonResources),
        //    HttpMethod = "Post")]
        //[Display(Name = "LoginName", ResourceType = typeof(CommonResources))]
        public string LoginName { get; set; } = string.Empty;
        
        private string displayName = string.Empty;

        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Required(ErrorMessageResourceName = "DisplayNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "DisplayName", ResourceType = typeof(CommonResources))]
        public string DisplayName { get; set; } = string.Empty;
        
        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "FirstName", ResourceType = typeof(CommonResources))]
        public string FirstName { get; set; } = string.Empty;
        
        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "LastName", ResourceType = typeof(CommonResources))]
        public string LastName { get; set; } = string.Empty;
        
        //[Display(Name = "DateOfBirth", ResourceType = typeof(CommonResources))]
        public DateTime? DateOfBirth { get; set; }

        
        public DateTime? LastLoginDate { get; set; }

        public string TimeZoneId { get; set; }

        [Display(Name = "Administrator Notes")]
        public string Comment { get; set; }


    }
}
