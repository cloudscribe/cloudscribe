using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class ClientEditViewModel
    {
        public ClientEditViewModel()
        {
            CurrentClient = new ClientItemViewModel();
        }

        public string SiteId { get; set; } = string.Empty;

        public ClientItemViewModel CurrentClient { get; set; }



    }
}
