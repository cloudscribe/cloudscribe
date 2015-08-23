// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-08-19
//	Last Modified:		    2015-08-23
// 


using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using cloudscribe.Core.Web.ViewModels.SystemInfo;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.Controllers
{
    public class SystemInfoController : CloudscribeBaseController
    {
        public SystemInfoController(
            SystemInfoManager systemInfoManager,
            ConfigHelper configuration,
            ITimeZoneResolver timeZoneResolver
            )
        {
            systemInfo = systemInfoManager;

            config = configuration;
            this.timeZoneResolver = timeZoneResolver;

        }

        private SystemInfoManager systemInfo;
        private ConfigHelper config;
        private ITimeZoneResolver timeZoneResolver;


        [Authorize(Roles = "Admins")]
        public IActionResult Index()
        {
            ViewData["Title"] = "System Information";
            ViewData["Heading"] = "System Information";

            var serverInfo = new SystemInfoViewModel();
            serverInfo.Name = this.Context.Request.Host.Value;
            serverInfo.LocalAddress = this.Context.Connection.LocalIpAddress.ToString();
            serverInfo.OperatingSystem = systemInfo.OperatingSystem;
            serverInfo.Runtime = systemInfo.Runtime;
            serverInfo.EnvironmentName = systemInfo.EnvironmentName;
            serverInfo.DatabasePlatform = systemInfo.DatabasePlatform;
            serverInfo.CloudscribeCoreVersion = systemInfo.CloudscribeCoreVersion;
            

            return View(serverInfo);
        }

        [Authorize(Roles = "ServerAdmins")]
        public async Task<IActionResult> ViewLog(
            int pageNumber = 1,
            int pageSize = -1,
            string sort = "desc")
        {
            ViewData["Title"] = "System Log";
            ViewData["Heading"] = "System Log";

            int itemsPerPage = config.DefaultPageSize_LogView();
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new LogListViewModel();
            if (sort == "desc")
            {
                model.LogPage = await systemInfo.GetLogsDescending(pageNumber, itemsPerPage);
            }
            else
            {
                model.LogPage = await systemInfo.GetLogsAscending(pageNumber, itemsPerPage);
            }

            model.TimeZone = await timeZoneResolver.GetUserTimeZone();

            var count = await systemInfo.GetLogItemCount();

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;

            return View(model);

        }

        [Authorize(Roles = "ServerAdmins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogItemDelete(int id)
        {
            bool result = await systemInfo.DeleteLogItem(id);
               
            return RedirectToAction("ViewLog");
        }

        [Authorize(Roles = "ServerAdmins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogDeleteAll()
        {
            bool result = await systemInfo.DeleteAllLogItems();

            return RedirectToAction("ViewLog");
        }



    }
}
