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
            //Emphasize = scope.Emphasize;
            //Required = scope.Required;
            //ClaimsRule = scope.ClaimsRule;
            //AllowUnrestrictedIntrospection = scope.AllowUnrestrictedIntrospection;
            Enabled = apiResource.Enabled;
            //IncludeAllClaimsForUser = scope.IncludeAllClaimsForUser;
            //ShowInDiscoveryDocument = scope.ShowInDiscoveryDocument;

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
        /// Specifies whether the consent screen will emphasize this scope. Use this setting for sensitive or important scopes. Defaults to false
        /// </summary>
       // public bool Emphasize { get; set; }

        /// <summary>
        /// Specifies whether the user can de-select the scope on the consent screen. Defaults to false
        /// </summary>
        //public bool Required { get; set; }

        /// <summary>
        /// Rule for determining which claims should be included in the token (this is implementation specific)
        /// </summary>
        //public string ClaimsRule { get; set; }

        /// <summary>
        /// Allows this scope to see all other scopes in the access token when using the introspection endpoint
        /// </summary>
        //public bool AllowUnrestrictedIntrospection { get; set; }

        /// <summary>
        /// Indicates if scope is enabled and can be requested. Defaults to true
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// If enabled, all claims for the user will be included in the token. Defaults to false
        /// </summary>
        //public bool IncludeAllClaimsForUser { get; set; }

        /// <summary>
        /// Specifies whether this scope is shown in the discovery document. Defaults to true
        /// </summary>
        //public bool ShowInDiscoveryDocument { get; set; }
    }
}
