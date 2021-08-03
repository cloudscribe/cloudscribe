// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2017-07-25
// 
// TODO: support custom profile properties that are required for registration

using System;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Web.Common.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            ExternalAuthenticationList = new List<Microsoft.AspNetCore.Authentication.AuthenticationScheme>();
        }
        
        public Guid UserId { get; set; } = Guid.Empty;
        public Guid SiteId { get; set; } = Guid.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The email address does not appear as valid")]
        [StringLength(100, ErrorMessage = "Email has a maximum length of 100 characters")]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match password")]
        public string ConfirmPassword { get; set; }

        //public bool UseEmailForLogin { get; set; } = true;

        [Remote("UsernameAvailable", "Account", AdditionalFields = "UserId",
           ErrorMessage = "Username not accepted, please try another value",
           HttpMethod = "Post")]
        [Required(ErrorMessage = "Username is required")]
        //[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username can only contain letters or digits, with no spaces")]
        //[RequiredWhen("UseEmailForLogin", false, ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        // TODO do we want unique display names?
        // people can have the same real names like John Smith and why should we prevent anyone from using their real name?
        // not that we can be sure they are telling the truth about anything
        
        //[Required(ErrorMessage = "Display name  is required")]
        public string DisplayName { get; set; } = string.Empty;
        
        [StringLength(100, ErrorMessage = "First name has a maximum length of 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Last name has a maximum length of 100 characters")]
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string RecaptchaSiteKey { get; set; } = string.Empty;
        public bool UseInvisibleCaptcha { get; set; } = false;

        public string RegistrationPreamble { get; set; } = string.Empty;

        public string RegistrationAgreement { get; set; } = string.Empty;
        
        public bool AgreementRequired { get; set; } = false; // if true then AgreeToTerms is required to be checked

        [EnforceTrue("AgreementRequired", ErrorMessage = "You must agree to the terms of use.")]
        public bool AgreeToTerms { get; set; } = false;

        public IEnumerable<Microsoft.AspNetCore.Authentication.AuthenticationScheme> ExternalAuthenticationList { get; set; }
    }
}
