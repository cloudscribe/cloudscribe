using IdentityServer4.Models;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewClientViewModel
    {
        //TODO: localize error messages
        [Required]
        public string SiteId { get; set; }

        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string ClientId { get; set; }

        [Required]
        public string ClientName { get; set; }

        public AccessTokenType AccessTokenType { get; set; }

        public TokenExpiration RefreshTokenExpiration { get; set; }
        public TokenUsage RefreshTokenUsage { get; set; }
    }
}
