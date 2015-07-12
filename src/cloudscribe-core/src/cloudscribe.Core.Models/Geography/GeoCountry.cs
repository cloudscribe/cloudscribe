// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-11-02
// Last Modified:			2014-11-02
// 


using System;

namespace cloudscribe.Core.Models.Geography
{
    public class GeoCountry : IGeoCountry
    {

        public GeoCountry()
        { }

        private Guid guid = Guid.Empty;

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        private string name = string.Empty;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string iSOCode2 = string.Empty;

        public string ISOCode2
        {
            get { return iSOCode2; }
            set { iSOCode2 = value; }
        }
        private string iSOCode3 = string.Empty;

        public string ISOCode3
        {
            get { return iSOCode3; }
            set { iSOCode3 = value; }
        }

    }
}
