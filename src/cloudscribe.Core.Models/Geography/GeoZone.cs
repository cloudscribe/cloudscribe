// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2015-12-08
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

        private string name = string.Empty;
        public string Name
        {
            get { return name ?? string.Empty; }
            set { name = value; }
        }

        private string code = string.Empty;
        public string Code
        {
            get { return code ?? string.Empty; }
            set { code = value; }
        }

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
