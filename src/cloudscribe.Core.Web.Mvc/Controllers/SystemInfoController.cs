// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2016-06-06
// 

using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.SystemInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Web.Controllers.Mvc
{
    public class SystemInfoController : Controller
    {
        public SystemInfoController(
            SystemInfoManager systemInfoManager,
            IOptions<UIOptions> uiOptionsAccessor,
            IStringLocalizer<CloudscribeCore> localizer
            )
        {
            systemInfo = systemInfoManager;
            uiOptions = uiOptionsAccessor.Value;
            sr = localizer;

        }

        private SystemInfoManager systemInfo;
        private UIOptions uiOptions;
        private IStringLocalizer sr;

        [Authorize(Policy = "AdminPolicy")]
        public IActionResult Index()
        {
            ViewData["Title"] = sr["System Information"];
            
            var serverInfo = new SystemInfoViewModel();
            serverInfo.Name = this.HttpContext.Request.Host.Value;
            if(HttpContext.Connection.LocalIpAddress != null)
            {
                serverInfo.LocalAddress = HttpContext.Connection.LocalIpAddress.ToString();
                
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
