using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class EmailRequiredViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The email address does not appear as valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
