// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2017-09-15
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

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match password")]
        public string ConfirmPassword { get; set; }

        public bool MustChangePwd { get; set; }

        //[Remote("UsernameAvailable", "Account", AdditionalFields = "UserId",ErrorMessage = "Username not available, please try another value",HttpMethod = "Post")]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        // TODO do we want unique display names?
        // people can have the same real names like John Smith and why should we prevent anyone from using their real name?
        // not that we can be sure they are telling the truth about anything
        
        [Required(ErrorMessage = "Display Name is required.")]
        public string DisplayName { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

    }
}
