using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.IpAddresses
{
    public class BulkUploadIpAddressesModel
    {
        public Guid Id { get; set; }
        public IFormFile BulkIpAddresses { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }

        [Required(ErrorMessage = "Site ID is required")]
        public Guid SiteId { get; set; }
        public bool IsPermitted { get; set; }
        public bool IsRange { get; set; }
    }
}
