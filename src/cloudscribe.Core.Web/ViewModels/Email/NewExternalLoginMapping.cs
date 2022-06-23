using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class NewExternalLoginMappingEmailViewModel
    {
        public ISiteContext Tenant { get; set; }
        public string ToAddress { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ManageExternalLoginsUrl {  get; set; }
    }
}