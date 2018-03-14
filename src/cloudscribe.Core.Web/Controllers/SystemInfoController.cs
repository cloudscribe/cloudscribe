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
            _systemInfo = systemInfoManager;
            _uiOptions = uiOptionsAccessor.Value;
            _sr = localizer;

        }

        private SystemInfoManager _systemInfo;
        private UIOptions _uiOptions;
        private IStringLocalizer _sr;

        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public IActionResult Index()
        {
            ViewData["Title"] = _sr["System Information"];

            var serverInfo = new SystemInfoViewModel
            {
                Name = this.HttpContext.Request.Host.Value
            };
            if (HttpContext.Connection.LocalIpAddress != null)
            {
                serverInfo.LocalAddress = HttpContext.Connection.LocalIpAddress.ToString();
                
            }
            
            serverInfo.OperatingSystem = _systemInfo.OperatingSystem;
            serverInfo.Runtime = _systemInfo.Runtime;
            serverInfo.EnvironmentName = _systemInfo.EnvironmentName;
            serverInfo.DatabasePlatform = _systemInfo.DatabasePlatform;
            serverInfo.CloudscribeCoreVersion = _systemInfo.CloudscribeCoreVersion;
            serverInfo.OtherVersions = _systemInfo.GetOtherVersions();
            
            return View(serverInfo);
        }

    }
}
