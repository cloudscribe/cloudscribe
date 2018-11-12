// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-03
//	Last Modified:              2018-11-11
// 


using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.FileManager.Web.Services
{
    public class FileManagerVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.FileManager.Web"; } }

        public Guid ApplicationId { get { return new Guid("088dbabd-e7c6-4d54-b2a6-aad489b808a1"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(FileManagerService).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
