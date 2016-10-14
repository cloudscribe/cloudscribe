// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-10-14
// 

using cloudscribe.Core.IdentityServerIntegration.Models;
using cloudscribe.Core.IdentityServerIntegration.Services;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Controllers
{
    [Authorize(Policy = "IdentityServerAdminPolicy")]
    public class ScopesController : Controller
    {
        public ScopesController(
            //SiteContext currentSite,
            SiteManager siteManager,
            ScopesManager scopesManager,
            IStringLocalizer<CloudscribeIntegration> localizer
            )
        {
            this.siteManager = siteManager;
            this.scopesManager = scopesManager;
            sr = localizer;
        }

        private SiteManager siteManager;
        private ScopesManager scopesManager;
        private IStringLocalizer<CloudscribeIntegration> sr;

        [HttpGet]
        public async Task<IActionResult> Index(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1
            )
        {
            ISiteContext selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value) as ISiteContext;
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Resource Scope Management"], selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = sr["Resource Scope Management"];
            }

            int itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = new ScopeListViewModel();
            var result = await scopesManager.GetScopes(selectedSite.Id.ToString(), pageNumber, itemsPerPage);
            model.Scopes = result.Data;

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = result.TotalItems;

            return View(model);
        }

    }
}
