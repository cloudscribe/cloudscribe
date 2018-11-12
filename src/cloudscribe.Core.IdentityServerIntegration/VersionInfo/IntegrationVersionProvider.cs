// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-04
//	Last Modified:              2017-10-04
// 

using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class IntegrationVersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Core.IdentityServerIntegration"; } }

        public Guid ApplicationId { get { return new Guid("685ba5c6-568a-4ef2-9d5c-d1c09481eb87"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(ClientExtensions).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
