// Copyright(c) Source Tree Solutions, LLC.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//  Author:                     Joe Audette
//  Created:                    2017-10-04
//	Last Modified:              2018-11-11
// 

using cloudscribe.Core.Models;
using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.Core.Web.Components
{
    public class DataStorageVersionInfo : IVersionProvider
    {
        public DataStorageVersionInfo(
            IDataPlatformInfo dbPlatform)
        {
            _dbPlatform = dbPlatform;
            name = _dbPlatform.GetType().Assembly.GetName().Name;
    
        }

        private IDataPlatformInfo _dbPlatform;
        private string name = "DataStorageVersionInfo";

        public string Name
        {
            get { return name; }
 
        }

        public Guid ApplicationId { get { return new Guid("93ce8ef4-20d8-480a-b919-e5dd6a90512d"); } }

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
