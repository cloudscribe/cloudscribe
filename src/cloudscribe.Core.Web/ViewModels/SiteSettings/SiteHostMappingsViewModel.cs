// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-05-05
// Last Modified:			2016-06-14
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

        public IList<ISiteHost> HostMappings { get; set; }    
        public int SiteListReturnPageNumber { get; set; } = 1;
        public Guid SiteId { get; set; } = Guid.Empty;
        
    }
}
