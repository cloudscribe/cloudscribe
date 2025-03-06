namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ClientEditViewModel
    {
        public ClientEditViewModel()
        {
            NewClient = new NewClientViewModel();
        }

        public string SiteId { get; set; } = string.Empty;

        public ClientItemViewModel CurrentClient { get; set; } = null;

        public NewClientViewModel NewClient { get; set; }
    }
}
