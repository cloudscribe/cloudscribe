// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Http.Authentication;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            ExternalAuthenticationList = new List<AuthenticationDescription>();
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        public string RecaptchaSiteKey { get; set; } = string.Empty;

        public string LoginInfoTop { get; set; } = string.Empty;

        public string LoginInfoBottom { get; set; } = string.Empty;


        public IEnumerable<AuthenticationDescription> ExternalAuthenticationList {get;set;}
    }
}
