// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2016-12-02
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
            state.Id = igeo.Id;
            state.CountryId = igeo.CountryId;
            state.Code = igeo.Code;
            state.Name = igeo.Name;

            return state;
        }

    }
}
