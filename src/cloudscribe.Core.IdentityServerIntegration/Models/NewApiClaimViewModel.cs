using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewApiClaimViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ApiName { get; set; }

        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
