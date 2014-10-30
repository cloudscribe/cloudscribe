// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2014-10-30
// 
// TODO: support custom profile properties that are required for registration

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Resources;
using cloudscribe.Configuration;
using cloudscribe.Configuration.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class RegisterViewModel
    {
        // strange that when you specify resource parms for EmailAddressAttribute
        // it throws an error unless you explcitely set ErrorMessage to empty string
        // I guess its base class sets a default value that must be cleared

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [EmailAddress(ErrorMessage = "",ErrorMessageResourceName = "EmailNotValid", ErrorMessageResourceType = typeof(CommonResources))]
        [Display(Name = "Email", ResourceType = typeof(CommonResources))]
        public string Email { get; set; }

        //[StringLength(AppSettings.PasswordMaxLength, MinimumLength = AppSettings.PasswordMinLength, ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [StringLengthWithConfig(MinimumLength = 7, MaximumLength=100, MinLengthKey="PasswordMinLength", MaxLengthKey="PasswordMaxLength", ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(CommonResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(CommonResources))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMatchErrorMessage", ErrorMessageResourceType = typeof(CommonResources))]
        public string ConfirmPassword { get; set; }
    }
}
