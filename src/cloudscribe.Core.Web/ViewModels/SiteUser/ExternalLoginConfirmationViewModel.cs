// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using cloudscribe.Web.Common.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "The email address does not appear as valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string RegistrationPreamble { get; set; } = string.Empty;

        public string RegistrationAgreement { get; set; } = string.Empty;

        public bool AgreementRequired { get; set; } = false; // if true then AgreeToTerms is required to be checked

        [EnforceTrue("AgreementRequired", ErrorMessage = "You must agree to the terms of use.")]
        public bool AgreeToTerms { get; set; } = false;
    }
}
