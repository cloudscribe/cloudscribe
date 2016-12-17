using IdentityServer4.Models;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class IdentityItemViewModel
    {
        public IdentityItemViewModel()
        { }

        public IdentityItemViewModel(
            string siteId,
            IdentityResource resource)
        {
            SiteId = siteId;
            Name = resource.Name;
            Description = resource.Description;
            DisplayName = resource.DisplayName;
            Emphasize = resource.Emphasize;
            Enabled = resource.Enabled;
            Required = resource.Required;
            ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument;

        }

        [Required]
        public string SiteId { get; set; }


        /// <summary>
        /// This is the value a client will use to request the scope.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public bool Emphasize { get; set; }
        public bool Enabled { get; set; } = true;
        
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
    }
}
