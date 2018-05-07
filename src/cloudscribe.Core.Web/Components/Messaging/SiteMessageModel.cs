using cloudscribe.Core.Models;
using cloudscribe.Email.Senders;

namespace cloudscribe.Core.Web.Components.Messaging
{
    public class SiteMessageModel : MessageModel
    {
        public string Title { get; set; }
        public ISiteContext Tenant { get; set; }
    }
}
