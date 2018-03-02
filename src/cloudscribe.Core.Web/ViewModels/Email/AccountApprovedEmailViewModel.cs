using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class AccountApprovedEmailViewModel
    {
        public ISiteContext Tenant { get; set; }
        public string LoginUrl { get; set; }
    }
}
