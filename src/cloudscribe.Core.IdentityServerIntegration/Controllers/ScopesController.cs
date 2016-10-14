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
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Scope Management"], selectedSite.SiteName);
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
                ViewData["Title"] = sr["Scope Management"];
            }

            int itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = new ScopeListViewModel();
            model.SiteId = selectedSite.Id.ToString();
            var result = await scopesManager.GetScopes(selectedSite.Id.ToString(), pageNumber, itemsPerPage);
            model.Scopes = result.Data;

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = result.TotalItems;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditScope(
            Guid? siteId,
            string scopeName = null)
        {
            //TODO: validate modelstate

            ISiteContext selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId.HasValue) && (siteId.Value != Guid.Empty) && (siteId.Value != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId.Value) as ISiteContext;      
            }
            else
            {
                selectedSite = siteManager.CurrentSite;     
            }

            if (string.IsNullOrEmpty(scopeName))
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New Scope"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Edit Scope"], selectedSite.SiteName);
            }

            var model = new ScopeEditViewModel();
            model.SiteId = selectedSite.Id.ToString();
            if (!string.IsNullOrEmpty(scopeName))
            {
                var scope = await scopesManager.FetchScope(model.SiteId, scopeName);
                model.CurrentScope = scope;
            }
            
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditScope(ScopeItemViewModel scopeModel)
        {
            Guid siteId = siteManager.CurrentSite.Id;
            if(!string.IsNullOrEmpty(scopeModel.SiteId) && scopeModel.SiteId.Length == 36)
            {
                siteId = new Guid(scopeModel.SiteId);
            }
            ISiteContext selectedSite;
            // only server admin site can edit other sites settings
            if ((siteId != Guid.Empty) && (siteId != siteManager.CurrentSite.Id) && (siteManager.CurrentSite.IsServerAdminSite))
            {
                selectedSite = await siteManager.Fetch(siteId) as ISiteContext;
            }
            else
            {
                selectedSite = siteManager.CurrentSite;
            }

            //if (string.IsNullOrEmpty(scopeModel.Name))
            //{
            //    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New Scope"], selectedSite.SiteName);
            //}
            //else
            //{
            //    ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Edit Scope"], selectedSite.SiteName);
            //}
            var scope = await scopesManager.FetchScope(selectedSite.Id.ToString(), scopeModel.Name);
            scope.AllowUnrestrictedIntrospection = scopeModel.AllowUnrestrictedIntrospection;
            scope.ClaimsRule = scopeModel.ClaimsRule;
            scope.Description = scopeModel.Description;
            scope.DisplayName = scopeModel.DisplayName;
            scope.Emphasize = scopeModel.Emphasize;
            scope.Enabled = scopeModel.Enabled;
            scope.IncludeAllClaimsForUser = scopeModel.IncludeAllClaimsForUser;
            scope.Required = scopeModel.Required;
            scope.ShowInDiscoveryDocument = scopeModel.ShowInDiscoveryDocument;
            await scopesManager.UpdateScope(selectedSite.Id.ToString(), scope);


            return RedirectToAction("Index");

        }


    }
}
