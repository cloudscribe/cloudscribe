using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Resources;
using cloudscribe.Configuration;
using cloudscribe.Configuration.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(CommonResources))]
        public string Email { get; set; }

        //[StringLength(AppSettings.PasswordMaxLength, MinimumLength = AppSettings.PasswordMinLength, ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Required]
        [StringLengthAppSettingsAttribute(MinimumLength = 6, MaximumLength=10, MinLengthKey="PasswordMinLength", MaxLengthKey="PasswordMaxLength", ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(CommonResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(CommonResources))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMatchErrorMessage", ErrorMessageResourceType = typeof(CommonResources))]
        public string ConfirmPassword { get; set; }
    }
}
