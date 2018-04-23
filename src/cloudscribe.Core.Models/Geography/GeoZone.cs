// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2018-04-23
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class GeoZone : IGeoZone
    {
        public GeoZone()
        {
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; set; }
        public Guid CountryId { get; set; } 

        public string Name { get; set; }

        public string Code { get; set; }

        public static GeoZone FromIGeoZone(IGeoZone igeo)
        {
            GeoZone state = new GeoZone
            {
                Id = igeo.Id,
                CountryId = igeo.CountryId,
                Code = igeo.Code,
                Name = igeo.Name
            };

            return state;
        }

    }
}
