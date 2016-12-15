using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewApiScopeClaimViewModel
    {
        public NewApiScopeClaimViewModel()
        { }

        public NewApiScopeClaimViewModel(
            string siteId,
            string apiName,
            string scopeName)
        {
            SiteId = siteId;
            ApiName = apiName;
            ScopeName = scopeName;
        }

        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ApiName { get; set; }

       [Required]
        public string ScopeName { get; set; }

        [Required]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string ClaimName { get; set; }

        
    }
}
