// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2015-08-20
// 



using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.Helpers;
using cloudscribe.Core.Web.ViewModels.SystemInfo;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.Runtime;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    public class SystemInfoController : CloudscribeBaseController
    {
        public SystemInfoController(
            IRuntimeEnvironment runtimeEnvironment,
            IHostingEnvironment hostingEnvironment,
            IApplicationEnvironment appEnvironment,
            IVersionProviderFactory versionProviderFactory,
            IDb database)
        {
            runtimeInfo = runtimeEnvironment;
            appInfo = appEnvironment;
            hostingInfo = hostingEnvironment;
            db = database;
            versionProviders = versionProviderFactory;

        }

        private IRuntimeEnvironment runtimeInfo;
        private IApplicationEnvironment appInfo;
        private IHostingEnvironment hostingInfo;
        private IVersionProviderFactory versionProviders;
        private IDb db;

        public IActionResult Index()
        {
            ViewData["Title"] = "System Information";
            ViewData["Heading"] = "System Information";

            var serverInfo = new SystemInfoViewModel();
            serverInfo.Name = this.Context.Request.Host.Value;
            serverInfo.LocalAddress = this.Context.Connection.LocalIpAddress.ToString();
            serverInfo.OperatingSystem = runtimeInfo.OperatingSystem + " " + runtimeInfo.OperatingSystemVersion;
            serverInfo.Runtime = runtimeInfo.RuntimeType + " " + runtimeInfo.RuntimeVersion + " " + runtimeInfo.RuntimeArchitecture;
            serverInfo.EnvironmentName = hostingInfo.EnvironmentName;
            serverInfo.DatabasePlatform = db.DBPlatform;
            IVersionProvider cloudscribeVersionProvider = versionProviders.Get("cloudscribe-core");
            if(cloudscribeVersionProvider != null)
            {
                serverInfo.CloudscribeCoreVersion = cloudscribeVersionProvider.GetCodeVersion().ToString();
            }


            return View(serverInfo);
        }

    }
}
