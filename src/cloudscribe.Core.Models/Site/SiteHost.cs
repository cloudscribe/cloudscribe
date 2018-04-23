// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2018-04-23
// 

using System;

namespace cloudscribe.Core.Models
{
    public class SiteHost : ISiteHost
    {
        public SiteHost()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public string HostName { get; set; }
        
        public Guid SiteId { get; set; }
        
    }
}
