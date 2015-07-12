// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2014-11-02
// 

using System;

namespace cloudscribe.Core.Models.Geography
{
    public class GeoZone : IGeoZone
    {

        public GeoZone()
        { }

        private Guid guid = Guid.Empty;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        private Guid countryGuid = Guid.Empty;

        public Guid CountryGuid
        {
            get { return countryGuid; }
            set { countryGuid = value; }
        }

        private string name = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string code = string.Empty;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

    }
}
