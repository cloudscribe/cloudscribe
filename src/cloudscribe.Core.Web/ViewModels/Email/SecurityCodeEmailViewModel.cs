using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class SecurityCodeEmailViewModel
    {
        public ISiteContext Tenant { get; set; }
        public string SecurityCode { get; set; }
    }
}
