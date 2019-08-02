// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-12-15
// Last Modified:			2019-08-02
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

namespace cloudscribe.Core.IdentityServerIntegration.Controllers.Mvc
{
    [Authorize(Policy = "IdentityServerAdminPolicy")]
    public class IdentityResourceController : Controller
    {
        public IdentityResourceController(
            SiteManager siteManager,
            ApiResourceManager apiManager,
            IdentityResourceManager idManager,
            IStringLocalizer<CloudscribeIds4Resources> localizer)
        {
            _apiManager = apiManager;
            _siteManager = siteManager;
            _idManager = idManager;
            sr = localizer;
        }

        private SiteManager _siteManager;
        private IdentityResourceManager _idManager;
        private ApiResourceManager _apiManager;
        private IStringLocalizer sr;

        [HttpGet]
        public async Task<IActionResult> Index(
            Guid? siteId,
            int pageNumber = 1,
            int pageSize = -1
            )
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            // only server admin site can edit other sites settings
            if (selectedSite.Id != _siteManager.CurrentSite.Id)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Identity Resource Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Identity Resource Management"];
            }

            int itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = new IdentityListViewModel();
            model.SiteId = selectedSite.Id.ToString();
            var result = await _idManager.GetIdentityResources(selectedSite.Id.ToString(), pageNumber, itemsPerPage);
            model.IdentityResources = result;

            //model.Paging.CurrentPage = pageNumber;
            //model.Paging.ItemsPerPage = itemsPerPage;
            //model.Paging.TotalItems = result.TotalItems;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditResource(
            Guid? siteId,
            string resourceName = null)
        {

            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            if (!string.IsNullOrEmpty(resourceName))
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Edit Identity Resource"], selectedSite.SiteName);
            }

            var model = new IdentityEditViewModel();
            model.SiteId = selectedSite.Id.ToString();
            model.NewResource.SiteId = model.SiteId;
            model.NewClaim.SiteId = model.SiteId;
            if (!string.IsNullOrEmpty(resourceName))
            {
                var resource = await _idManager.FetchIdentityResource(model.SiteId, resourceName);
                model.CurrentResource = resource;
            }

            if (model.CurrentResource == null)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New Identity Resource"], selectedSite.SiteName);
                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
                currentCrumbAdjuster.KeyToAdjust = "EditIdentityResource";
                currentCrumbAdjuster.AdjustedText = sr["New Identity Resource"];
                currentCrumbAdjuster.AddToContext();
            }
            else
            {
                model.NewClaim.SiteId = model.SiteId;
                model.NewClaim.ResourceName = model.CurrentResource.Name;
                
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> NewResource(IdentityItemViewModel resourceModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditResource", new { siteId = resourceModel.SiteId, resourceName = resourceModel.Name });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(resourceModel.SiteId) && resourceModel.SiteId.Length == 36)
            {
                siteId = new Guid(resourceModel.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New Identity Resource"], selectedSite.SiteName);

            var exists = await _idManager.IdentityResourceExists(selectedSite.Id.ToString(), resourceModel.Name);
            
            if (exists)
            {
                var model = new IdentityEditViewModel();
                model.SiteId = selectedSite.Id.ToString();
                model.NewResource = resourceModel;
                model.NewResource.SiteId = model.SiteId;

                if (exists) ModelState.AddModelError("resourcenameinuseerror", sr["Identity Resource name is already in use"]);


                return View("EditResource", model);
            }

            exists = await _apiManager.ApiResourceExists(selectedSite.Id.ToString(), resourceModel.Name);

            if (exists)
            {
                var model = new IdentityEditViewModel();
                model.SiteId = selectedSite.Id.ToString();
                model.NewResource = resourceModel;
                model.NewResource.SiteId = model.SiteId;

                if (exists) ModelState.AddModelError("resourcenameinuseerror", sr["Sorry, there already exists an API resource with this name, it is not allowed to have Identity resources with the same names as API resources"]);


                return View("EditResource", model);
            }


            var identityResource = new IdentityResource
            {
                Name = resourceModel.Name,
                DisplayName = resourceModel.DisplayName,
                Description = resourceModel.Description,
                Enabled = resourceModel.Enabled,
                Required = resourceModel.Required,
                ShowInDiscoveryDocument = resourceModel.ShowInDiscoveryDocument
            };

            await _idManager.CreateIdentityResource(selectedSite.Id.ToString(), identityResource);

            var successFormat = sr["The Identity Resource <b>{0}</b> was successfully Created."];

            this.AlertSuccess(string.Format(successFormat, identityResource.Name), true);

            return RedirectToAction("EditResource", new { siteId = selectedSite.Id.ToString(), resourceName = identityResource.Name });
        }

        [HttpPost]
        public async Task<IActionResult> EditResource(IdentityItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditResource", new { siteId = model.SiteId, resourceName = model.Name });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var resource = await _idManager.FetchIdentityResource(selectedSite.Id.ToString(), model.Name);

            if (resource == null)
            {
                this.AlertDanger(sr["API Resource not found"], true);
                return RedirectToAction("Index", new { siteId = selectedSite.Id.ToString() });
            }
            
            resource.Description = model.Description;
            resource.DisplayName = model.DisplayName;
            resource.Emphasize = model.Emphasize;
            resource.Enabled = model.Enabled;
            resource.Required = model.Required;
            resource.ShowInDiscoveryDocument = model.ShowInDiscoveryDocument;
            await _idManager.UpdateIdentityResource(selectedSite.Id.ToString(), resource);

            var successFormat = sr["The Identity Resource <b>{0}</b> was successfully updated."];

            this.AlertSuccess(string.Format(successFormat, resource.Name), true);

            return RedirectToAction("EditResource", new { siteId = selectedSite.Id.ToString(), resourceName = resource.Name });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteResource(Guid siteId, string resourceName)
        {
            await _idManager.DeleteIdentityResource(siteId.ToString(), resourceName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddClaim(NewIdentityClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditResource", new { siteId = model.SiteId, resourceName = model.ResourceName });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var resource = await _idManager.FetchIdentityResource(selectedSite.Id.ToString(), model.ResourceName);
            if (resource == null)
            {
                this.AlertDanger(sr["Invalid request, Identity Resource not found."], true);
                return RedirectToAction("Index");
            }

            //var claim = new ScopeClaim(model.Name, model.AlwaysIncludeInIdToken);
            //claim.Description = model.Description;

            if (resource.UserClaims.Contains(model.ClaimName))
            {
                this.AlertDanger(sr["Identity Resource already has a claim with that name."], true);
                return RedirectToAction("EditResource", new { siteId = selectedSite.Id.ToString(), resourceName = model.ResourceName });
            }
            resource.UserClaims.Add(model.ClaimName);

            await _idManager.UpdateIdentityResource(selectedSite.Id.ToString(), resource);

            var successFormat = sr["The Claim <b>{0}</b> was successfully added."];

            this.AlertSuccess(string.Format(successFormat, model.ClaimName), true);

            return RedirectToAction("EditResource", new { siteId = selectedSite.Id.ToString(), resourceName = model.ResourceName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClaim(Guid siteId, string resourceName, string claimName)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            var resource = await _idManager.FetchIdentityResource(selectedSite.Id.ToString(), resourceName);
            if (resource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            string found = null;
            foreach (var c in resource.UserClaims)
            {
                if (c == claimName)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                resource.UserClaims.Remove(found);
                await _idManager.UpdateIdentityResource(siteId.ToString(), resource);
                var successFormat = sr["The Claim <b>{0}</b> was successfully removed."];
                this.AlertSuccess(string.Format(successFormat, claimName), true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, Identity Resource claim not found."], true);
            }

            return RedirectToAction("EditResource", new { siteId = selectedSite.Id.ToString(), resourceName = resourceName });
        }

    }
}
