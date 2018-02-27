// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-04
//	Last Modified:              2017-10-04
// 


using cloudscribe.Web.Common.Setup;
using System;
using System.Reflection;

namespace cloudscribe.Core.IdentityServerIntegration.Mvc
{
    public class VersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Core.IdentityServerIntegration.Mvc"; } }

        public Guid ApplicationId { get { return new Guid("dfc8e8e1-f67b-42c5-8b46-dcd202c54651"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(ConsentController).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }

    }
}
