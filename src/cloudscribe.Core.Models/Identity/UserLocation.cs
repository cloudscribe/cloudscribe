// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-01
// Last Modified:			2015-12-01
// 

using System;

namespace cloudscribe.Core.Models
{
    public class UserLocation : IUserLocation
    {
        public Guid RowId { get; set; } = Guid.Empty;
        public Guid UserGuid { get; set; } = Guid.Empty;
        public Guid SiteGuid { get; set; } = Guid.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public long IpAddressLong { get; set; } = 0;
        public string HostName { get; set; } = string.Empty;
        public float Longitude { get; set; } = 0;
        public float Latitude { get; set; } = 0;
        public string Isp { get; set; } = string.Empty;
        public string Continent { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public int CaptureCount { get; set; } = 0;
        public DateTime FirstCaptureUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastCaptureUtc { get; set; } = DateTime.UtcNow;


    }
}
