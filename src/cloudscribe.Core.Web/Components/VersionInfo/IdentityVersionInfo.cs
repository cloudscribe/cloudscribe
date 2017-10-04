// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-04
//	Last Modified:              2017-10-04
// 

using cloudscribe.Core.Identity;
using cloudscribe.Web.Common.Setup;
using System;
using System.Reflection;

namespace cloudscribe.Core.Web.Components
{
    public class IdentityVersionInfo : IVersionProvider
    {
        
        private Assembly assembly = typeof(ClaimsPrincipalExtensions).Assembly;
       
        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("192018f1-512e-456a-bcb9-078fe261e539"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }

    }
}
