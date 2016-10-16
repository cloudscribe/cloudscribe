using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewScopeSecretViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ScopeName { get; set; }

        [Required]
        public string Value { get; set; }

        public DateTime? Expiration { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }
    }
}
