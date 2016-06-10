// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2016-06-10
// 
// TODO: support custom profile properties that are required for registration

using System;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Web.Common.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        
        [Display(Name = "UserId")]
        public Guid UserId { get; set; } = Guid.Empty;

        [Display(Name = "SiteId")]
        public Guid SiteId { get; set; } = Guid.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "invalid email format")]
        //[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[EmailAddress(ErrorMessage = "", ErrorMessageResourceName = "EmailNotValid", ErrorMessageResourceType = typeof(CommonResources))]
        //[Display(Name = "Email", ResourceType = typeof(CommonResources))]
        public string Email { get; set; }

        //  //[StringLength(AppSettings.PasswordMaxLength, MinimumLength = AppSettings.PasswordMinLength, ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //  //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[RequiredWhen("UserId", -1, ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[StringLengthWithConfig(
        //    MinimumLength = 7,
        //    MaximumLength = 100,
        //    MinLengthKey = "PasswordMinLength",
        //    MaxLengthKey = "PasswordMaxLength",
        //    ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        
        //[Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is required")]
        //[Display(Name = "Password", ResourceType = typeof(CommonResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match password")]
        public string ConfirmPassword { get; set; }

        // http://stackoverflow.com/questions/36033022/using-remote-validation-wit-asp-net-core
        // //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Remote("LoginNameAvailable", "Account", AdditionalFields = "UserId",
        //    ErrorMessageResourceName = "LoginNameNotAvailable", ErrorMessageResourceType = typeof(CommonResources),
        //    HttpMethod = "Post")]
        //[Display(Name = "LoginName", ResourceType = typeof(CommonResources))]
        //[Required]
        public string LoginName { get; set; } = string.Empty;

        
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

        public string RecaptchaSiteKey { get; set; } = string.Empty;


        public string RegistrationPreamble { get; set; } = string.Empty;

        public string RegistrationAgreement { get; set; } = string.Empty;

        [Display(Name = "AgreeToTerms")]

        public bool AgreementRequired { get; set; } = false;

        [EnforceTrue("AgreementRequired", ErrorMessage = "You must agree to the terms of use.")]
        public bool AgreeToTerms { get; set; } = false;
    }
}
