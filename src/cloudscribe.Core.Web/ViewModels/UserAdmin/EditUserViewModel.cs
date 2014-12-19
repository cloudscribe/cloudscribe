// Author:					Joe Audette
// Created:				    2014-08-31
// Last Modified:		    2014-10-30
// 
// TODO: support custom profile properties that are required for registration

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Resources;
using cloudscribe.Configuration;
using cloudscribe.Configuration.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.UserAdmin
{
    public class EditUserViewModel
    {
        private int userId = -1;

        [Display(Name = "UserId")]
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private Guid userGuid = Guid.Empty;

        [Display(Name = "UserGuid")]
        public Guid SiteGuid
        {
            get { return userGuid; }
            set { userGuid = value; }

        }

        // strange that when you specify resource parms for EmailAddressAttribute
        // it throws an error unless you explcitely set ErrorMessage to empty string
        // I guess its base class sets a default value that must be cleared

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [EmailAddress(ErrorMessage = "", ErrorMessageResourceName = "EmailNotValid", ErrorMessageResourceType = typeof(CommonResources))]
        [Display(Name = "Email", ResourceType = typeof(CommonResources))]
        public string Email { get; set; }

        //[StringLength(AppSettings.PasswordMaxLength, MinimumLength = AppSettings.PasswordMinLength, ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [RequiredIf("UserId", -1, ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [StringLengthWithConfig(MinimumLength = 7, MaximumLength = 100, MinLengthKey = "PasswordMinLength", MaxLengthKey = "PasswordMaxLength", ErrorMessageResourceName = "PasswordLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(CommonResources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(CommonResources))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordMatchErrorMessage", ErrorMessageResourceType = typeof(CommonResources))]
        public string ConfirmPassword { get; set; }


        // TODO: this should be conditionally hidden or visible and required
        // if we implement login with username vs email
        private string loginName = string.Empty;

        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Display(Name = "LoginName", ResourceType = typeof(CommonResources))]
        public string LoginName
        {
            get { return loginName; }
            set { loginName = value; }
        }

        private string displayName = string.Empty;

        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Required(ErrorMessageResourceName = "DisplayNameRequired", ErrorMessageResourceType = typeof(CommonResources))]
        [Display(Name = "DisplayName", ResourceType = typeof(CommonResources))]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        private string firstName = string.Empty;

        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Display(Name = "FirstName", ResourceType = typeof(CommonResources))]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName = string.Empty;

        //[StringLengthWithConfig(MinimumLength = 3, MaximumLength = 255, MinLengthKey = "SiteNameMinLength", MaxLengthKey = "SiteNameMaxLength", ErrorMessageResourceName = "SiteNameLengthErrorFormat", ErrorMessageResourceType = typeof(CommonResources))]
        [Display(Name = "LastName", ResourceType = typeof(CommonResources))]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
    }
}
