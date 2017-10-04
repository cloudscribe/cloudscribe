// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-04
//	Last Modified:              2017-10-04
// 

using cloudscribe.Core.Web.Controllers.Mvc;
using cloudscribe.Web.Common.Setup;
using System;
using System.Reflection;

namespace cloudscribe.Core.Web.Mvc
{
    public class ControllerVersionInfo : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Core.Web.Mvc"; } }

        public Guid ApplicationId { get { return new Guid("59405659-0a77-4094-971e-b82c990bf75c"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(SiteAdminController).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }

    }
}
