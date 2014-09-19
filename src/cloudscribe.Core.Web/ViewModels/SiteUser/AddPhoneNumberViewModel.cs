using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}
