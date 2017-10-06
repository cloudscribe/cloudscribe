using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class LoginWithRecoveryCodeViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }
    }
}
