// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2015-12-19
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.SystemInfo;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    public class SystemInfoController : CloudscribeBaseController
    {
        public SystemInfoController(
            SystemInfoManager systemInfoManager,
            IOptions<UIOptions> uiOptionsAccessor,
            ITimeZoneResolver timeZoneResolver
            )
        {
            systemInfo = systemInfoManager;

            this.timeZoneResolver = timeZoneResolver;
            uiOptions = uiOptionsAccessor.Value;

        }

        private SystemInfoManager systemInfo;
        private ITimeZoneResolver timeZoneResolver;
        private UIOptions uiOptions;

        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Index()
        {
            ViewData["Title"] = "System Information";
            ViewData["Heading"] = "System Information";

            var serverInfo = new SystemInfoViewModel();
            serverInfo.Name = this.HttpContext.Request.Host.Value;
            if(this.HttpContext.Connection.LocalIpAddress != null)
            {
                serverInfo.LocalAddress = this.HttpContext.Connection.LocalIpAddress.ToString();
                
            }
            
            serverInfo.OperatingSystem = systemInfo.OperatingSystem;
            serverInfo.Runtime = systemInfo.Runtime;
            serverInfo.EnvironmentName = systemInfo.EnvironmentName;
            serverInfo.DatabasePlatform = systemInfo.DatabasePlatform;
            serverInfo.CloudscribeCoreVersion = systemInfo.CloudscribeCoreVersion;
            

            return View(serverInfo);
        }

        



    }
}
