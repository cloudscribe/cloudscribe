using System;
using System.Collections.Generic;
// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-20
//	Last Modified:		    2017-09-23
// 


namespace cloudscribe.Core.Web.ViewModels.SystemInfo
{
    public class SystemInfoViewModel
    {
        public SystemInfoViewModel()
        {
            OtherVersions = new List<KeyValuePair<string, string>>();
        }

        public string Name { get; set; }
        public string LocalAddress { get; set; }
        public string OperatingSystem { get; set; }
        public string Runtime { get; set; }
        public string EnvironmentName { get; set; }
        public string DatabasePlatform { get; set; }
        public string CloudscribeCoreVersion { get; set; }
        public DateTime ServerTimeUtc { get; } = DateTime.UtcNow;

        public List<KeyValuePair<string, string>> OtherVersions { get; set; }
    }
}
