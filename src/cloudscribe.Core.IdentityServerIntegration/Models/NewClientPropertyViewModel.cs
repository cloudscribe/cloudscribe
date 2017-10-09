using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace cloudscribe.Core.IdentityServerIntegration.Models
{
    public class NewClientPropertyViewModel
    {
        [Required]
        public string SiteId { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
