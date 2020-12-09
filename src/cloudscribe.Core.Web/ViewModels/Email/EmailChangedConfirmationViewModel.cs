using cloudscribe.Core.Models;

namespace cloudscribe.Core.Web.ViewModels.Email
{
    public class EmailChangedConfirmationViewModel
    {
        public ISiteContext Tenant            { get; set; }
        public string       ConfirmationUrl   { get; set; }
        public string       ConfirmationToken { get; set; }
        public string       OldEmail          { get; set; }
        public string       NewEmail          { get; set; }
        public string       SiteUrl           { get; set; }
    }
}
