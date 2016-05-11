using System;

namespace cloudscribe.Core.Models
{
    public interface IUserLocation
    {
        Guid Id { get; set; }
        Guid SiteId { get; set; }
        Guid UserId { get; set; }

        int CaptureCount { get; set; }
        string City { get; set; }
        string Continent { get; set; }
        string Country { get; set; }
        DateTime FirstCaptureUtc { get; set; }
        string HostName { get; set; }
        string IpAddress { get; set; }
        long IpAddressLong { get; set; }
        string Isp { get; set; }
        DateTime LastCaptureUtc { get; set; }

        //http://stackoverflow.com/questions/28068123/double-or-decimal-for-latitude-longitude-values-in-c-sharp
        //http://stackoverflow.com/questions/1440620/which-sql-server-data-type-best-represents-a-double-in-c

        double Latitude { get; set; }
        double Longitude { get; set; }

        string Region { get; set; }
        string TimeZone { get; set; }
        
    }
}