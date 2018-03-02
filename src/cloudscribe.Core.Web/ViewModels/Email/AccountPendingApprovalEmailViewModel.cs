using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class AccountPendingApprovalEmailViewModel
    {
        public ISiteContext Tenant { get; set; }
        public IUserContext User { get; set; }
    }
}
