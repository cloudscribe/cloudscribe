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

        public bool SendPasswordActivationEmail { get; set; } = false;

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Display Name is required.")]
        public string DisplayName { get; set; } = string.Empty;
        
        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

    }
}
