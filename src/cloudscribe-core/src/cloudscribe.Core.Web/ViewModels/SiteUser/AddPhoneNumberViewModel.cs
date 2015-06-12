using System.ComponentModel.DataAnnotations;
//using cloudscribe.Resources;


namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        //[Display(Name = "PhoneNumber", ResourceType = typeof(CommonResources))]
        public string Number { get; set; }
    }
}
