using cloudscribe.Web.Common.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class DeletePersonalDataViewModel
    {
        [RequiredWhen("HasPassword", false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool HasPassword { get; set; }
    }
}
