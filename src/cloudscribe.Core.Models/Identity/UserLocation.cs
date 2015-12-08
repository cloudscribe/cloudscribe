// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-01
// Last Modified:			2015-12-08
// 

using System;

namespace cloudscribe.Core.Models
{
    public class UserLocation : IUserLocation
    {
        public Guid RowId { get; set; } = Guid.Empty;
        public Guid UserGuid { get; set; } = Guid.Empty;
        public Guid SiteGuid { get; set; } = Guid.Empty;

        private string ipAddress = string.Empty;
        public string IpAddress
        {
            get { return ipAddress ?? string.Empty; }
            set { ipAddress = value; }
        }

        public long IpAddressLong { get; set; } = 0;

        private string hostName = string.Empty;
        public string HostName
        {
            get { return hostName ?? string.Empty; }
            set { hostName = value; }
        }

        public float Longitude { get; set; } = 0;
        public float Latitude { get; set; } = 0;

        private string isp = string.Empty;
        public string Isp
        {
            get { return isp ?? string.Empty; }
            set { isp = value; }
        }

        private string continent = string.Empty;
        public string Continent
        {
            get { return continent ?? string.Empty; }
            set { continent = value; }
        }

        private string country = string.Empty;
        public string Country
        {
            get { return country ?? string.Empty; }
            set { country = value; }
        }

        private string region = string.Empty;
        public string Region
        {
            get { return region ?? string.Empty; }
            set { region = value; }
        }

        private string city = string.Empty;
        public string City
        {
            get { return city ?? string.Empty; }
            set { city = value; }
        }

        private string timeZone = string.Empty;
        public string TimeZone
        {
            get { return timeZone ?? string.Empty; }
            set { timeZone = value; }
        }
        
        public int CaptureCount { get; set; } = 0;
        public DateTime FirstCaptureUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastCaptureUtc { get; set; } = DateTime.UtcNow;


    }
}
