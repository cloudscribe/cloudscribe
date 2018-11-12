// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-09-23
//	Last Modified:              2018-11-11
// 

using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Core.Web.Components
{
    public class CloudscribeCoreVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Core.Web"; } }

        public Guid ApplicationId { get { return new Guid("b7dcd727-91c3-477f-bc42-d4e5c8721daa"); } }

        public Version CurrentVersion
        {
           
            get {
                
                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(SiteManager).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if(!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }
                
                return version;
            }
        }
    }

    
}
