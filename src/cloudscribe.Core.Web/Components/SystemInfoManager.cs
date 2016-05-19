// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-21
//	Last Modified:		    2016-05-19
// 

using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Setup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SystemInfoManager
    {
        public SystemInfoManager(
            IHostingEnvironment hostingEnvironment,
            IVersionProviderFactory versionProviderFactory,
            IDataPlatformInfo databaseInfo)
        {
            hostingInfo = hostingEnvironment;
            dbInfo = databaseInfo;
            versionProviders = versionProviderFactory;
            cloudscribeVersionProvider = versionProviders.Get("cloudscribe-core");

            runtimeInfo = PlatformServices.Default.Runtime;
        }

        private RuntimeEnvironment runtimeInfo;
        private IHostingEnvironment hostingInfo;
        private IVersionProviderFactory versionProviders;
        IVersionProvider cloudscribeVersionProvider = null;
        private IDataPlatformInfo dbInfo;
        

        public string OperatingSystem
        {
            get { return runtimeInfo.OperatingSystem + " " + runtimeInfo.OperatingSystemVersion; }
        }

        public string Runtime
        {
            get { return runtimeInfo.RuntimeType + " " + runtimeInfo.RuntimeVersion + " " + runtimeInfo.RuntimeArchitecture; }
        }

        public string EnvironmentName
        {
            get {  return hostingInfo.EnvironmentName; }
        }

        public string DatabasePlatform
        {
            get { return dbInfo.DBPlatform; }
        }

        public string CloudscribeCoreVersion
        {
            get
            {
                if(cloudscribeVersionProvider != null)
                {
                    return cloudscribeVersionProvider.CurrentVersion.ToString();
                }
                return "not found";
            }
        }


        

    }
}
