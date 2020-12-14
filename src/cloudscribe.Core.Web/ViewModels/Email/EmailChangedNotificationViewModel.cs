using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class EmailChangedNotificationViewModel
    {
        public ISiteContext Tenant   { get; set; }
        public string       OldEmail { get; set; }
        public string       NewEmail { get; set; }
        public string       SiteUrl  { get; set; }
    }
}
