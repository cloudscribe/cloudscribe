using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class PasswordResetEmailViewModel
    {
        public ISiteContext Tenant { get; set; }
        public string ResetUrl { get; set; }
    }
}
