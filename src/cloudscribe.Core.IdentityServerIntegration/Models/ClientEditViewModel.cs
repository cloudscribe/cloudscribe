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
            //CurrentClient = new ClientItemViewModel();
            NewClient = new NewClientViewModel();
        }

        public string SiteId { get; set; } = string.Empty;

        public ClientItemViewModel CurrentClient { get; set; } = null;

        public NewClientViewModel NewClient { get; set; }



    }
}
