// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2015-11-22
// 


using System;

namespace cloudscribe.Core.Models.Geography
{
    public class GeoCountry : IGeoCountry
    {

        public GeoCountry()
        { }

        public Guid Guid { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string ISOCode2 { get; set; } = string.Empty;
        public string ISOCode3 { get; set; } = string.Empty;

        public static GeoCountry FromIGeoCountry(IGeoCountry igeo)
        {
            GeoCountry country = new GeoCountry();
            country.Guid = igeo.Guid;
            country.ISOCode2 = igeo.ISOCode2;
            country.ISOCode3 = igeo.ISOCode3;
            country.Name = igeo.Name;

            return country;
        }

    }
}
