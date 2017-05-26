using System;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Web.Common.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.Account
{
    public class AcceptTermsViewModel
    {
        public DateTime TermsUpdatedDate { get; set; }
        public string RegistrationPreamble { get; set; } = string.Empty;

        public string RegistrationAgreement { get; set; } = string.Empty;

        public bool AgreementRequired { get; set; } = false; // if true then AgreeToTerms is required to be checked

        [EnforceTrue("AgreementRequired", ErrorMessage = "You must agree to the terms of use.")]
        public bool AgreeToTerms { get; set; } = false;
    }
}
