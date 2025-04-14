﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using cloudscribe.Web.Common.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            ExternalAuthenticationList = new List<Microsoft.AspNetCore.Authentication.AuthenticationScheme>();
        }

        public bool UseEmailForLogin { get; set; } = true;
        public bool DisableDbAuth { get; set; } = false;

        [Required(ErrorMessage = "Email or username is required")]
        [StringLength(100, ErrorMessage = "Email or username has a maximum length of 100 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        public string RecaptchaSiteKey { get; set; } = string.Empty;

        public bool UseInvisibleCaptcha { get; set; } = false;

        public string LoginInfoTop { get; set; } = string.Empty;

        public string LoginInfoBottom { get; set; } = string.Empty;


        public IEnumerable<Microsoft.AspNetCore.Authentication.AuthenticationScheme> ExternalAuthenticationList {get;set;}
    }
}
