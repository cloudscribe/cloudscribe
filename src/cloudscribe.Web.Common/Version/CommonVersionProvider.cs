// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-03
//	Last Modified:              2017-10-03
// 

using System;
using System.Reflection;


namespace cloudscribe.Web.Common.Setup
{
    public class CommonVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Web.Common"; } }

        public Guid ApplicationId { get { return new Guid("e0858342-43fa-4c88-a37d-258d2c899592"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(VersionProviderFactory).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
