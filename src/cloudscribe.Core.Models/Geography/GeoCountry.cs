// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2015-12-08
// 


using System;

namespace cloudscribe.Core.Models.Geography
{
    public class GeoCountry : IGeoCountry
    {

        public GeoCountry()
        { }

        public Guid Guid { get; set; } = Guid.Empty;

        private string name = string.Empty;
        public string Name
        {
            get { return name ?? string.Empty; }
            set { name = value; }
        }

        private string iSOCode2 = string.Empty;
        public string ISOCode2
        {
            get { return iSOCode2 ?? string.Empty; }
            set { iSOCode2 = value; }
        }

        private string iSOCode3 = string.Empty;
        public string ISOCode3
        {
            get { return iSOCode3 ?? string.Empty; }
            set { iSOCode3 = value; }
        }

        
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
