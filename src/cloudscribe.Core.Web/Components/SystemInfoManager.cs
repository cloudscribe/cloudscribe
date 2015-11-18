// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-21
//	Last Modified:		    2015-11-18
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Models.Logging;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Components
{
    public class SystemInfoManager
    {
        public SystemInfoManager(
            IRuntimeEnvironment runtimeEnvironment,
            IHostingEnvironment hostingEnvironment,
            IApplicationEnvironment appEnvironment,
            IVersionProviderFactory versionProviderFactory,
            IDb database,
            ILogRepository logRepository)
        {
            runtimeInfo = runtimeEnvironment;
            appInfo = appEnvironment;
            hostingInfo = hostingEnvironment;
            db = database;
            logRepo = logRepository;
            versionProviders = versionProviderFactory;
            cloudscribeVersionProvider = versionProviders.Get("cloudscribe-core");


        }

        private IRuntimeEnvironment runtimeInfo;
        private IApplicationEnvironment appInfo;
        private IHostingEnvironment hostingInfo;
        private IVersionProviderFactory versionProviders;
        IVersionProvider cloudscribeVersionProvider = null;
        private IDb db;
        private ILogRepository logRepo;

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
            get { return db.DBPlatform; }
        }

        public string CloudscribeCoreVersion
        {
            get
            {
                if(cloudscribeVersionProvider != null)
                {
                    return cloudscribeVersionProvider.GetCodeVersion().ToString();
                }
                return "not found";
            }
        }


        public async Task<int> GetLogItemCount()
        {
            return await logRepo.GetCount();
        }

        public async Task<List<ILogItem>> GetLogsDescending(int pageNumber, int pageSize)
        {
            return await logRepo.GetPageDescending(pageNumber, pageSize);
        }

        public async Task<List<ILogItem>> GetLogsAscending(int pageNumber, int pageSize)
        {
            return await logRepo.GetPageAscending(pageNumber, pageSize);
        }

        public async Task<bool> DeleteLogItem(int id)
        {
            return await logRepo.Delete(id);
        }

        public async Task<bool> DeleteAllLogItems()
        {
            return await logRepo.DeleteAll();
        }

    }
}
