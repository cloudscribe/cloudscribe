// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2015-08-04
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class GeoZone : IGeoZone
    {
        public GeoZone()
        { }
        
        public Guid Guid { get; set; } = Guid.Empty;
        public Guid CountryGuid { get; set; } = Guid.Empty; 
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public static GeoZone FromIGeoZone(IGeoZone igeo)
        {
            GeoZone state = new GeoZone();
            state.Guid = igeo.Guid;
            state.CountryGuid = igeo.CountryGuid;
            state.Code = igeo.Code;
            state.Name = igeo.Name;

            return state;
        }

    }
}
