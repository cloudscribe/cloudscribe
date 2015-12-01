using System;

namespace cloudscribe.Core.Models
{
    public interface IUserLocation
    {
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
        float Latitude { get; set; }
        float Longitude { get; set; }
        string Region { get; set; }
        Guid RowId { get; set; }
        Guid SiteGuid { get; set; }
        string TimeZone { get; set; }
        Guid UserGuid { get; set; }
    }
}