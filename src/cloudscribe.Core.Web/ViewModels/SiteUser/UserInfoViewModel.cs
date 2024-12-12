﻿using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class UserInfoViewModel
    {
        [StringLength(100, ErrorMessage = "First Name has a maximum length of 100 characters")]
        public string FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Last Name has a maximum length of 100 characters")]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [Phone]
        [StringLength(50, ErrorMessage = "Phone number has a maximum length of 50 characters")]
        public string PhoneNumber { get; set; }

        [Url(ErrorMessage ="You must provide a valid URL")]
        [StringLength(300, ErrorMessage = "Website Url has a maximum length of 300 characters")]
        public string WebSiteUrl { get; set; }

        public string AvatarUrl { get; set; }
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "Display Name has a maximum length of 100 characters")]
        public string DisplayName { get; set; }
    }
}
