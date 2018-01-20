using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewApiSecretViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ApiName { get; set; }

        [Required]
        public string Value { get; set; }

        public DateTime? Expiration { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }

        public string HashOption { get; set; }
    }
}
