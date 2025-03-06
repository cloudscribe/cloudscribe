using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ApiItemViewModel
    {
        public ApiItemViewModel()
        {

        }
        public ApiItemViewModel(string siteId, ApiResource apiResource)
        {
            SiteId = siteId;
            Name = apiResource.Name;
            DisplayName = apiResource.DisplayName;
            Description = apiResource.Description;
            Enabled = apiResource.Enabled;
        }

        [Required]
        public string SiteId { get; set; }

        
        /// <summary>
        /// This is the value a client will use to request the scope.
        /// </summary>
        [Required(ErrorMessage="Name is required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed in name")]
        public string Name { get; set; }

        /// <summary>
        /// Display name for consent screen.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Description for the consent screen.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates if scope is enabled and can be requested. Defaults to true
        /// </summary>
        public bool Enabled { get; set; }
    }
}
