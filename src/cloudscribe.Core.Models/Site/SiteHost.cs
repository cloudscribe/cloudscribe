// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2014-08-17
// Last Modified:			2016-11-12
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

        private string hostName = string.Empty;
        public string HostName
        {
            get { return hostName ?? string.Empty; }
            set { hostName = value; }
        }

        public Guid SiteId { get; set; }
        
    }
}
