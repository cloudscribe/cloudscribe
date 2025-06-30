using System;
using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.ViewModels.IpAddresses
{
    public class IpAddressesViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "IP Address is required")]
        [StringLength(39, ErrorMessage = "IP Address has a maximum length of 39 characters")]
        [CustomValidation(typeof(IpAddressesViewModel), nameof(ValidateIpAddress), ErrorMessage = "Invalid IP Address format")]
        public string IpAddress { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }

        [Required(ErrorMessage = "Site ID is required")]
        public Guid SiteId { get; set; }
        public bool IsPermitted { get; set; }

        public static ValidationResult ValidateIpAddress(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return new ValidationResult("IP Address cannot be empty");
            }
            // Simple validation for IPv4 and IPv6 formats
            if (!System.Net.IPAddress.TryParse(ipAddress, out _))
            {
                return new ValidationResult("Invalid IP Address format");
            }
            return ValidationResult.Success;
        }
    }
}