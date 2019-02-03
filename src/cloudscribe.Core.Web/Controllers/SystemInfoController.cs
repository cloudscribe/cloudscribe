// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2019-02-03
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
            SystemInfo = systemInfoManager;
            UIOptions = uiOptionsAccessor.Value;
            StringLocalizer = localizer;

        }

        protected SystemInfoManager SystemInfo { get; private set; }
        protected UIOptions UIOptions { get; private set; }
        protected IStringLocalizer StringLocalizer { get; private set; }

        [Authorize(Policy = PolicyConstants.AdminPolicy)]
        public virtual IActionResult Index()
        {
            ViewData["Title"] = StringLocalizer["System Information"];

            var serverInfo = new SystemInfoViewModel
            {
                Name = this.HttpContext.Request.Host.Value
            };
            if (HttpContext.Connection.LocalIpAddress != null)
            {
                serverInfo.LocalAddress = HttpContext.Connection.LocalIpAddress.ToString();
                
            }
            
            serverInfo.OperatingSystem = SystemInfo.OperatingSystem;
            serverInfo.Runtime = SystemInfo.Runtime;
            serverInfo.EnvironmentName = SystemInfo.EnvironmentName;
            serverInfo.DatabasePlatform = SystemInfo.DatabasePlatform;
            serverInfo.CloudscribeCoreVersion = SystemInfo.CloudscribeCoreVersion;
            serverInfo.OtherVersions = SystemInfo.GetOtherVersions();
            
            return View(serverInfo);
        }

    }
}
