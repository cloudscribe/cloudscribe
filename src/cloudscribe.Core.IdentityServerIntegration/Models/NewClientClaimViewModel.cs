using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewClientClaimViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }

        //public string ValueType { get; set; }

        //public string Issuer { get; set; }
        
        //public string OriginalIssuer { get; set; }
    }
}
