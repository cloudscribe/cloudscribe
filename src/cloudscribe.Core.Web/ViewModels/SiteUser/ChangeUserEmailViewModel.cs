using cloudscribe.Web.Common.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class ChangeUserEmailViewModel
    {
        [RequiredWhen("HasPassword", false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool HasPassword { get; set; }

        [Display(Name = "Current Email")]
        public string CurrentEmail { get; set; }

        [Display(Name = "New Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string NewEmail { get; set; }

        public bool AccountApproved { get; set; }

        public bool AllowUserToChangeEmail { get; set; }

        public bool EmailIsConfigured { get; set; }

        public bool RequireConfirmedEmail { get; set; }

        public string SuccessNotification {  get; set; }
    }
}
