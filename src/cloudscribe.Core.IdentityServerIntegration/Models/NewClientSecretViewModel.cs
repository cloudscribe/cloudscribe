using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewClientSecretViewModel
    {
        public NewClientSecretViewModel()
        {

        }

        public NewClientSecretViewModel(string siteId, string clientId)
        {
            SiteId = siteId;
            ClientId = clientId;
        }

        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string Value { get; set; }

        public DateTime? Expiration { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }
    }
}
