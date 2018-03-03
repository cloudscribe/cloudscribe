using cloudscribe.Core.Models;

namespace sourceDev.WebApp.ViewModels
{
    public class TestEmailMessageViewModel
    {
        public ISiteContext Tenant { get; set; }

        public string Greeting { get; set; }

        public string Message { get; set; }
    }
}
