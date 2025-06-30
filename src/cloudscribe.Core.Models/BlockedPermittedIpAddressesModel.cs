using System;

namespace cloudscribe.Core.Models
{
    public class BlockedPermittedIpAddressesModel
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public Guid SiteId { get; set; }
        public bool IsPermitted { get; set; }
    }
}