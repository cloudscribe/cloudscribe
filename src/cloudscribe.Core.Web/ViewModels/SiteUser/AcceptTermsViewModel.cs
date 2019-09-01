using cloudscribe.Web.Common.DataAnnotations;
using System;

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
