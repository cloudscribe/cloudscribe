using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class AccountExistsEmailViewModel
    {
        public ISiteContext Tenant { get; set; }
        public string LoginUrl { get; set; }
        public string ResetUrl { get; set; }
        public string ConfirmUrl { get; set; }
        public bool StillNeedsApproval { get; set; }
    }
}