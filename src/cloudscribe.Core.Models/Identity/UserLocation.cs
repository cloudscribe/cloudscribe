// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-01
// Last Modified:			2018-04-23
// 

using System;

namespace cloudscribe.Core.Models
{
    public class UserLocation : IUserLocation
    {
        public UserLocation()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public Guid SiteId { get; set; }

        public string IpAddress { get; set; }
        public long IpAddressLong { get; set; } = 0;
        
        public string HostName { get; set; }

        public double Longitude { get; set; } = 0;
        public double Latitude { get; set; } = 0;

        public string Isp { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string TimeZone { get; set; }
        
        public int CaptureCount { get; set; } = 0;
        public DateTime FirstCaptureUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastCaptureUtc { get; set; } = DateTime.UtcNow;

        public static UserLocation FromIUserLocation(IUserLocation i)
        {
            UserLocation l = new UserLocation
            {
                CaptureCount = i.CaptureCount,
                City = i.City,
                Continent = i.Continent,
                Country = i.Country,
                FirstCaptureUtc = i.FirstCaptureUtc,
                HostName = i.HostName,
                IpAddress = i.IpAddress,
                IpAddressLong = i.IpAddressLong,
                LastCaptureUtc = i.LastCaptureUtc,
                Latitude = i.Latitude,
                Longitude = i.Longitude,
                Region = i.Region,
                Id = i.Id,
                SiteId = i.SiteId,
                TimeZone = i.TimeZone,
                UserId = i.UserId
            };

            return l;
        }


    }
}
