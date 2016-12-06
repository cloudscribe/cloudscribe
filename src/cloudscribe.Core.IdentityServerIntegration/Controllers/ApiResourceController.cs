// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-10-21
// 

using cloudscribe.Core.IdentityServerIntegration.Models;
using cloudscribe.Core.IdentityServerIntegration.Services;
using cloudscribe.Core.Web.Components;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Controllers
{
    [Authorize(Policy = "IdentityServerAdminPolicy")]
    public class ApiResourceController : Controller
    {
        public ApiResourceController(
            SiteManager siteManager,
            ApiResourceManager apiManager,
            IStringLocalizer<CloudscribeIntegration> localizer)
        {
            this.siteManager = siteManager;
            this.apiManager = apiManager;
            sr = localizer;
        }

        private SiteManager siteManager;
        private ApiResourceManager apiManager;
        private IStringLocalizer<CloudscribeIntegration> sr;

        [HttpGet]
        public async Task<IActionResult> Index(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1
            )
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Scope Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Scope Management"];
            }

            int itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = new ApiListViewModel();
            model.SiteId = selectedSite.Id.ToString();
            var result = await apiManager.GetApiResources(selectedSite.Id.ToString(), pageNumber, itemsPerPage);
            model.Apis = result.Data;

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = result.TotalItems;

            return View(model);
        }


    }
}
