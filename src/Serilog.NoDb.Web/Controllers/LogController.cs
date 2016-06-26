// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-06-26
// Last Modified:			2016-06-26
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog.NoDb.Web.Services;
using Serilog.NoDb.Web.ViewModels;
using System;
using System.Threading.Tasks;

namespace Serilog.NoDb.Web.Controllers
{
    public class LogController : Controller
    {
        public LogController(
            LogManager logManager,
            ITimeZoneIdResolver timeZoneIdResolver)
        {
            this.logManager = logManager;
            this.timeZoneIdResolver = timeZoneIdResolver;
        }

        private LogManager logManager;
        private ITimeZoneIdResolver timeZoneIdResolver;

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
                //model.Items = await logManager.GetLogsDescending(pageNumber, itemsPerPage);
            }
            else
            {
                //model.Items = await logManager.GetLogsAscending(pageNumber, itemsPerPage);
            }

            model.TimeZoneId = await timeZoneIdResolver.GetUserTimeZoneId();

            //var count = await logManager.GetLogItemCount();

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            //model.Paging.TotalItems = count;

            return View(model);

        }


    }
}
