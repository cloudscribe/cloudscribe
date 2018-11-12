// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-21
//	Last Modified:		    2018-11-11
// 

using cloudscribe.Core.Models;
using cloudscribe.Versioning;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace cloudscribe.Core.Web.Components
{
    public class SystemInfoManager
    {
        public SystemInfoManager(
            IHostingEnvironment hostingEnvironment,
            IVersionProviderFactory versionProviderFactory,
            IDataPlatformInfo databaseInfo)
        {
            _hostingInfo = hostingEnvironment;
            _dbInfo = databaseInfo;
            _versionProviders = versionProviderFactory;
            _cloudscribeVersionProvider = _versionProviders.Get("cloudscribe.Core.Web");

        }

        private IHostingEnvironment _hostingInfo;
        private IVersionProviderFactory _versionProviders;
        private IVersionProvider _cloudscribeVersionProvider = null;
        private IDataPlatformInfo _dbInfo;
        

        public string OperatingSystem
        {
            get { return RuntimeInformation.OSDescription + " " + RuntimeInformation.OSArchitecture.ToString(); }
        }

        public string Runtime
        {
            get { return RuntimeInformation.FrameworkDescription; }
        }

        public string EnvironmentName
        {
            get {  return _hostingInfo.EnvironmentName; }
        }

        public string DatabasePlatform
        {
            get { return _dbInfo.DBPlatform; }
        }

        public string CloudscribeCoreVersion
        {
            get
            {
                if(_cloudscribeVersionProvider != null)
                {
                    return _cloudscribeVersionProvider.CurrentVersion.ToString();
                }
                return "not found";
            }
        }

        public List<KeyValuePair<string,string>> GetOtherVersions()
        {
            var list = new List<KeyValuePair<string,string>>();

            foreach(var v in _versionProviders.VersionProviders.OrderBy(v => v.Name))
            {
                if(v.Name != "cloudscribe.Core.Web")
                {
                    list.Add(new KeyValuePair<string, string>(v.Name, v.CurrentVersion.ToString()));
                }
            }

            return list;

        }


        

    }
}
