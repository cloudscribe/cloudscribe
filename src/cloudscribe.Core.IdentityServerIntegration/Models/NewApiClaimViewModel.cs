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

        /// <summary>
        /// Specifies whether this claim should always be present in the identity token 
        /// (even if an access token has been requested as well). 
        /// Applies to identity scopes only. Defaults to false
        /// </summary>
        //public bool AlwaysIncludeInIdToken { get; set; }
        public string Description { get; set; }
    }
}
