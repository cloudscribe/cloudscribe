// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-05-05
// Last Modified:			2015-06-10
// 

using cloudscribe.Core.Models;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.SiteSettings
{
    public class SiteHostMappingsViewModel
    {
        public SiteHostMappingsViewModel()
        {
            HostMappings = new List<ISiteHost>();
        }

        public string Heading { get; set; }
        public IList<ISiteHost> HostMappings { get; set; }

        private int siteListReturnPageNumber = 1;

        public int SiteListReturnPageNumber
        {
            get { return siteListReturnPageNumber; }
            set { siteListReturnPageNumber = value; }
        }

        private Guid siteGuid = Guid.Empty;

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        private int siteId = -1;

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

    }
}
