// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class AddPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Phone number is required to proceed")]
        [Phone]
        [StringLength(50, ErrorMessage = "Phone number has a maximum length of 50 characters")]
        public string Number { get; set; }
    }
}
