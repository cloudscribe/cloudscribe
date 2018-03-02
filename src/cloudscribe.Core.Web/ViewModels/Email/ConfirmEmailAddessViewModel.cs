using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class ConfirmEmailAddessViewModel
    {
        public ISiteContext Tenant { get; set; }
        public string ConfirmationUrl { get; set; }
    }
}
