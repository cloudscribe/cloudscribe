// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2016-06-10
// 

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.Core.Web.ViewModels.UserAdmin
{
    public class NewUserViewModel
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid SiteId { get; set; } = Guid.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "invalid email format")]
        public string Email { get; set; }

        //  //[StringLength(AppSettings.PasswordMaxLength, MinimumLength = AppSettings.PasswordMinLength, ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //  //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        //[RequiredWhen("UserId", -1, ErrorMessage = "PasswordRequired")]
        //[StringLengthWithConfig(
        //    MinimumLength = 7,
        //    MaximumLength = 100,
        //    MinLengthKey = "PasswordMinLength",
        //    MaxLengthKey = "PasswordMaxLength",
        //    ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]

        //[Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        //[Display(Name = "Password", ResourceType = typeof(CommonResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match password")]
        public string ConfirmPassword { get; set; }

        // http://stackoverflow.com/questions/36033022/using-remote-validation-wit-asp-net-core
        // //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Remote("LoginNameAvailable", "Account", AdditionalFields = "UserId",
           ErrorMessage = "Login Name not available, please try another value",
           HttpMethod = "Post")]

        [Required(ErrorMessage = "Login Name is required.")]
        public string LoginName { get; set; } = string.Empty;

        // TODO do we want unique display names?
        // people can have the same real names like John Smith and why should we prevent anyone from using their real name?
        // not that we can be sure they are telling the truth about anything

        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Required(ErrorMessage = "Display Name is required.")]
        public string DisplayName { get; set; } = string.Empty;

        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        public string FirstName { get; set; } = string.Empty;

        //  //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

    }
}
