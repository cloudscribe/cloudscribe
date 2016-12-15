using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewApiScopeViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ApiName { get; set; }

        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
        
        public bool Emphasize { get; set; }
        
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
    }
}
