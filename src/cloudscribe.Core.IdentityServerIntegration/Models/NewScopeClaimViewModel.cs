using System.ComponentModel.DataAnnotations;

// https://identityserver.github.io/Documentation/docsv2/configuration/scopesAndClaims.html

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewScopeClaimViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ScopeName { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Specifies whether this claim should always be present in the identity token 
        /// (even if an access token has been requested as well). 
        /// Applies to identity scopes only. Defaults to false
        /// </summary>
        public bool AlwaysIncludeInIdToken { get; set; }
        public string Description { get; set; }
    }
}
