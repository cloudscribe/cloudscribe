using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class IdentityEditViewModel
    {
        public IdentityEditViewModel()
        {
            NewResource = new IdentityItemViewModel();
            NewClaim = new NewIdentityClaimViewModel();
        }

        public string SiteId { get; set; } = string.Empty;

        public IdentityResource CurrentResource { get; set; } = null;

        public IdentityItemViewModel NewResource { get; set; }

        public NewIdentityClaimViewModel NewClaim { get; set; }
    }
}
