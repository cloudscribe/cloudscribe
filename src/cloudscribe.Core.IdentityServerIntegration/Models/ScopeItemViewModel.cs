using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ScopeItemViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public string ClaimsRule { get; set; }
        public bool AllowUnrestrictedIntrospection { get; set; }
        public bool Enabled { get; set; }
        public bool IncludeAllClaimsForUser { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }

    }
}
