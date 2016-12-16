using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewIdentityClaimViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ResourceName { get; set; }

        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string ClaimName { get; set; }
    }
}
