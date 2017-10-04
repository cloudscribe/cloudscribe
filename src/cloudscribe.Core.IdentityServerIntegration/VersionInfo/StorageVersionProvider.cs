// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-04
//	Last Modified:              2017-10-04
// 


using cloudscribe.Web.Common.Setup;
using System;
using System.Reflection;

namespace cloudscribe.Core.IdentityServerIntegration
{
    public class StorageVersionProvider : IVersionProvider
    {
        public StorageVersionProvider(IStorageInfo storageInfo)
        {
            _dbPlatform = storageInfo;
            name = _dbPlatform.GetType().Assembly.GetName().Name;
        }

        private IStorageInfo _dbPlatform;
        private string name = "DataStorageVersionInfo";

        public string Name
        {
            get { return name; }

        }

        public Guid ApplicationId { get { return new Guid("b0e19988-ce6e-4141-b93e-3dd735346b79"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = _dbPlatform.GetType().Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }


    }
}
