using System.ComponentModel.DataAnnotations;

namespace cloudscribe.Core.Web.Components
{
    public class ValidateIpAddress
    {
        public static ValidationResult IpAddressValidation(string ipAddress)
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