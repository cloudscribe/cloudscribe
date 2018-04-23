// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2018-04-23
// 


using System;

namespace cloudscribe.Core.Models.Geography
{
    public class GeoCountry : IGeoCountry
    {

        public GeoCountry()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string ISOCode2 { get; set; }

        public string ISOCode3 { get; set; }

        public static GeoCountry FromIGeoCountry(IGeoCountry igeo)
        {
            GeoCountry country = new GeoCountry
            {
                Id = igeo.Id,
                ISOCode2 = igeo.ISOCode2,
                ISOCode3 = igeo.ISOCode3,
                Name = igeo.Name
            };

            return country;
        }

    }
}
