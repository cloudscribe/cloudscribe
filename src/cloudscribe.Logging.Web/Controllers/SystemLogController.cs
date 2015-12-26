// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//	Author:                 Joe Audette
//  Created:			    2011-12-23
//	Last Modified:		    2015-12-26
// 


using cloudscribe.Core.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.Logging.Web.Controllers
{
    public class SystemLogController : Controller
    {
        public SystemLogController(
            LogManager logManager,
            ITimeZoneResolver timeZoneResolver)
        {
            this.logManager = logManager;
            this.timeZoneResolver = timeZoneResolver;
        }

        private LogManager logManager;
        private ITimeZoneResolver timeZoneResolver;

        [Authorize(Policy = "SystemLogPolicy")]
        public async Task<IActionResult> Index(
            int pageNumber = 1,
            int pageSize = -1,
            string sort = "desc")
        {
            ViewData["Title"] = "System Log";
            ViewData["Heading"] = "System Log";

            int itemsPerPage = logManager.LogPageSize;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }

            var model = new LogListViewModel();
            if (sort == "desc")
            {
                model.LogPage = await logManager.GetLogsDescending(pageNumber, itemsPerPage);
            }
            else
            {
                model.LogPage = await logManager.GetLogsAscending(pageNumber, itemsPerPage);
            }

            model.TimeZone = await timeZoneResolver.GetUserTimeZone();

            var count = await logManager.GetLogItemCount();

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = count;

            return View(model);

        }

        [Authorize(Policy = "SystemLogPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogItemDelete(int id)
        {
            bool result = await logManager.DeleteLogItem(id);

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "SystemLogPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogDeleteAll()
        {
            bool result = await logManager.DeleteAllLogItems();

            return RedirectToAction("Index");
        }

    }
}
