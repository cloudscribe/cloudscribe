// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2019-08-02
// 

using cloudscribe.Core.IdentityServerIntegration.Models;
using cloudscribe.Core.IdentityServerIntegration.Services;
using cloudscribe.Core.Web.Components;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Navigation;
using IdentityServer4;
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
    public class ApiResourceController : Controller
    {
        public ApiResourceController(
            SiteManager siteManager,
            ApiResourceManager apiManager,
            IdentityResourceManager idManager,
            IStringLocalizer<CloudscribeIds4Resources> localizer)
        {
            _siteManager = siteManager;
            _apiManager = apiManager;
            _idManager = idManager;
            sr = localizer;
        }

        private SiteManager _siteManager;
        private ApiResourceManager _apiManager;
        private IdentityResourceManager _idManager;
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
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - API Resource Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["API Resource Management"];
            }

            int itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = new ApiListViewModel();
            model.SiteId = selectedSite.Id.ToString();
            var result = await _apiManager.GetApiResources(selectedSite.Id.ToString(), pageNumber, itemsPerPage);
            model.Apis = result;

            //model.Paging.CurrentPage = pageNumber;
            //model.Paging.ItemsPerPage = itemsPerPage;
            //model.Paging.TotalItems = result.TotalItems;

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> EditApiResource(
            Guid? siteId,
            string apiName = null)
        {

            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            if (!string.IsNullOrEmpty(apiName))
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Edit API Resource"], selectedSite.SiteName);
            }

            var model = new ApiEditViewModel();
            model.SiteId = selectedSite.Id.ToString();
            model.NewApi.SiteId = model.SiteId;
            if (!string.IsNullOrEmpty(apiName))
            {
                var apiResource = await _apiManager.FetchApiResource(model.SiteId, apiName);
                model.CurrentApi = apiResource;
            }

            if (model.CurrentApi == null)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New API Resource"], selectedSite.SiteName);
                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
                currentCrumbAdjuster.KeyToAdjust = "EditApiResource";
                currentCrumbAdjuster.AdjustedText = sr["New API Resource"];
                currentCrumbAdjuster.AddToContext();
            }
            else
            {
                model.NewApiClaim.SiteId = model.SiteId;
                model.NewApiClaim.ApiName = model.CurrentApi.Name;
                model.NewApiSecret.SiteId = model.SiteId;
                model.NewApiSecret.ApiName = model.CurrentApi.Name;
                model.NewScope.SiteId = model.SiteId;
                model.NewScope.ApiName = model.CurrentApi.Name;
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditApiResource(ApiItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditApiResource", new { siteId = model.SiteId, name = model.Name });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), model.Name);

            if (apiResource == null)
            {
                this.AlertDanger(sr["API Resource not found"], true);
                return RedirectToAction("Index", new { siteId = selectedSite.Id.ToString() });
            }

           // apiResource.AllowUnrestrictedIntrospection = model.AllowUnrestrictedIntrospection;
            //apiResource.ClaimsRule = model.ClaimsRule;
            apiResource.Description = model.Description;
            apiResource.DisplayName = model.DisplayName;
           // apiResource.Emphasize = model.Emphasize;
            apiResource.Enabled = model.Enabled;
            //apiResource.IncludeAllClaimsForUser = model.IncludeAllClaimsForUser;
            //apiResource.Required = model.Required;
            //apiResource.ShowInDiscoveryDocument = model.ShowInDiscoveryDocument;
            await _apiManager.UpdateApiResource(selectedSite.Id.ToString(), apiResource);

            var successFormat = sr["The API Resource <b>{0}</b> was successfully updated."];

            this.AlertSuccess(string.Format(successFormat, apiResource.Name), true);

            return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = apiResource.Name });

        }

        [HttpPost]
        public async Task<IActionResult> NewApiResource(ApiItemViewModel apiModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditApiResource", new { siteId = apiModel.SiteId, ame = apiModel.Name });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(apiModel.SiteId) && apiModel.SiteId.Length == 36)
            {
                siteId = new Guid(apiModel.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New API Resource"], selectedSite.SiteName);

            var exists = await _apiManager.ApiResourceExists(selectedSite.Id.ToString(), apiModel.Name);
            
            if (exists)
            {
                var model = new ApiEditViewModel();
                model.SiteId = selectedSite.Id.ToString();
                model.NewApi = apiModel;
                model.NewApi.SiteId = model.SiteId;

                if (exists) ModelState.AddModelError("apinameinuseerror", sr["API Resource name is already in use"]);
                

                return View("EditApiResource", model);
            }

            exists = await _idManager.IdentityResourceExists(selectedSite.Id.ToString(), apiModel.Name);

            if (exists)
            {
                var model = new ApiEditViewModel();
                model.SiteId = selectedSite.Id.ToString();
                model.NewApi = apiModel;
                model.NewApi.SiteId = model.SiteId;

                if (exists) ModelState.AddModelError("apinameinuseerror", sr["Sorry, there already exists an Identity resource with this name, it is not allowed to have API resources with the same names as Identity resources"]);


                return View("EditApiResource", model);
            }

            var api = new ApiResource
            {
                Name = apiModel.Name,
                DisplayName = apiModel.DisplayName,
                Description = apiModel.Description
            };

            await _apiManager.CreateApiResource(selectedSite.Id.ToString(), api);

            var successFormat = sr["The API Resource <b>{0}</b> was successfully created."];

            this.AlertSuccess(string.Format(successFormat, api.Name), true);

            return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = api.Name });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiResource(Guid siteId, string apiName)
        {
            await _apiManager.DeleteApiResource(siteId.ToString(), apiName);
            return RedirectToAction("Index");
        }

        // Scope Claims
        // List of user claims that should be included in the identity (identity scope) or access token (resource scope).

        [HttpPost]
        public async Task<IActionResult> AddApiClaim(NewApiClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditApiResource", new { siteId = model.SiteId, apiName = model.ApiName });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), model.ApiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            //var claim = new ScopeClaim(model.Name, model.AlwaysIncludeInIdToken);
            //claim.Description = model.Description;

            if (apiResource.UserClaims.Contains(model.Name))
            {
                this.AlertDanger(sr["API Resource already has a claim with that name."], true);
                return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = model.ApiName });
            }
            apiResource.UserClaims.Add(model.Name);

            await _apiManager.UpdateApiResource(selectedSite.Id.ToString(), apiResource);

            var successFormat = sr["The Claim <b>{0}</b> was successfully added."];

            this.AlertSuccess(string.Format(successFormat, model.Name), true);

            return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = model.ApiName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiClaim(Guid siteId, string apiName, string claimName)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);
            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), apiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            string found = null;
            foreach (var c in apiResource.UserClaims)
            {
                if (c == claimName)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                apiResource.UserClaims.Remove(found);
                await _apiManager.UpdateApiResource(siteId.ToString(), apiResource);
                var successFormat = sr["The Claim <b>{0}</b> was successfully removed."];
                this.AlertSuccess(string.Format(successFormat, claimName), true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, API Resource claim not found."], true);
            }

            return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = apiName });
        }
        // http://docs.identityserver.io/en/release/reference/api_resource.html
        //        //API Secrets
        //        // https://identityserver.github.io/Documentation/docsv2/configuration/secrets.html
        //        // Secrets define how machines (e.g. a client or a scope) can authenticate with IdentityServer.
        //        // Value The value of the secret. This is being interpreted by the secret validator 
        //        // (e.g. a “password”-like share secret or something else that identifies a credential)
        //        // Description The description of the secret - useful for attaching some extra information to the secret
        //        // Expiration A point in time, where this secret will expire
        //        // Type Some string that gives the secret validator a hint what type of secret to expect (e.g. “SharedSecret” or “X509CertificateThumbprint”)

        [HttpPost]
        public async Task<IActionResult> AddApiSecret(NewApiSecretViewModel apiModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditApiResource", new { siteId = apiModel.SiteId, apiName = apiModel.ApiName });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(apiModel.SiteId) && apiModel.SiteId.Length == 36)
            {
                siteId = new Guid(apiModel.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), apiModel.ApiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            var secretValue = apiModel.Value;
            if (string.IsNullOrEmpty(apiModel.Type) || apiModel.Type == IdentityServerConstants.SecretTypes.SharedSecret)
            {
                switch (apiModel.HashOption)
                {
                    case "sha256":
                        secretValue = apiModel.Value.Sha256();
                        break;
                    case "sha512":
                        secretValue = apiModel.Value.Sha512();
                        break;
                }
            }

            var secret = new Secret(secretValue, apiModel.Description, apiModel.Expiration);
            secret.Type = apiModel.Type;

            if (apiResource.ApiSecrets.Contains(secret))
            {
                this.AlertDanger(sr["API Resource already has a secret with that value."], true);
                return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = apiModel.ApiName });
            }
            apiResource.ApiSecrets.Add(secret);

            await _apiManager.UpdateApiResource(selectedSite.Id.ToString(), apiResource);

            this.AlertSuccess(sr["The Secret was successfully added."], true);

            return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = apiModel.ApiName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiSecret(Guid siteId, string apiName, string secretValue)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), apiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            Secret found = null;
            foreach (var c in apiResource.ApiSecrets)
            {
                if (c.Value == secretValue)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                apiResource.ApiSecrets.Remove(found);
                await _apiManager.UpdateApiResource(siteId.ToString(), apiResource);
                this.AlertSuccess(sr["The Secret was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, API secret not found."], true);
            }

            return RedirectToAction("EditApiResource", new { siteId = siteId.ToString(), apiName = apiName });
        }


        [HttpPost]
        public async Task<IActionResult> AddApiScope(NewApiScopeViewModel apiModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditApiResource", new { siteId = apiModel.SiteId, apiName = apiModel.ApiName });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(apiModel.SiteId) && apiModel.SiteId.Length == 36)
            {
                siteId = new Guid(apiModel.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), apiModel.ApiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            var scope = new Scope(apiModel.Name, apiModel.DisplayName);
            scope.Description = apiModel.Description;
            scope.Required = apiModel.Required;
            scope.Emphasize = apiModel.Emphasize;
            scope.ShowInDiscoveryDocument = apiModel.ShowInDiscoveryDocument;

            var found = false;
            foreach (var c in apiResource.Scopes)
            {
                if (c.Name == scope.Name)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                this.AlertDanger(sr["API Resource already has a scope with that name."], true);
                return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = apiModel.ApiName });
            }
            apiResource.Scopes.Add(scope);

            await _apiManager.UpdateApiResource(selectedSite.Id.ToString(), apiResource);

            this.AlertSuccess(sr["The Scope was successfully added."], true);

            return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = apiModel.ApiName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiScope(Guid siteId, string apiName, string scopeName)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), apiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            Scope found = null;
            foreach (var c in apiResource.Scopes)
            {
                if (c.Name == scopeName)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                apiResource.Scopes.Remove(found);
                await _apiManager.UpdateApiResource(siteId.ToString(), apiResource);
                this.AlertSuccess(sr["The Scope was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, API scope not found."], true);
            }

            return RedirectToAction("EditApiResource", new { siteId = siteId.ToString(), apiName = apiName });
        }

        [HttpPost]
        public async Task<IActionResult> AddApiScopeClaim(NewApiScopeClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditApiResource", new { siteId = model.SiteId, apiName = model.ApiName });
            }

            Guid siteId = _siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), model.ApiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            Scope found = null;
            foreach (var c in apiResource.Scopes)
            {
                if (c.Name == model.ScopeName)
                {
                    found = c;
                    break;
                }
            }
            if (found == null)
            {
                this.AlertDanger(sr["Invalid request could not find scope with that name."], true);
                return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = model.ApiName });
            }

            if (found.UserClaims.Contains(model.ClaimName))
            {
                this.AlertDanger(sr["API Resource scope already has a claim with that name."], true);
                return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = model.ApiName });
            }
            found.UserClaims.Add(model.ClaimName);

            await _apiManager.UpdateApiResource(selectedSite.Id.ToString(), apiResource);

            var successFormat = sr["The Scope Claim <b>{0}</b> was successfully added."];

            this.AlertSuccess(string.Format(successFormat, model.ClaimName), true);

            return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = model.ApiName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApiScopeClaim(Guid siteId, string apiName, string scopeName, string claimName)
        {
            var selectedSite = await _siteManager.GetSiteForDataOperations(siteId);

            var apiResource = await _apiManager.FetchApiResource(selectedSite.Id.ToString(), apiName);
            if (apiResource == null)
            {
                this.AlertDanger(sr["Invalid request, API Resource not found."], true);
                return RedirectToAction("Index");
            }

            Scope found = null;
            foreach (var c in apiResource.Scopes)
            {
                if (c.Name == scopeName)
                {
                    found = c;
                    break;
                }
            }
            if (found == null)
            {
                this.AlertDanger(sr["Invalid request could not find scope claim with that name."], true);
                return RedirectToAction("EditApiResource", new { siteId = selectedSite.Id.ToString(), apiName = apiName });
            }

            
            if (found.UserClaims.Contains(claimName))
            {
                found.UserClaims.Remove(claimName);
                await _apiManager.UpdateApiResource(siteId.ToString(), apiResource);
                var successFormat = sr["The Scope Claim <b>{0}</b> was successfully removed."];
                this.AlertSuccess(string.Format(successFormat, claimName), true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, API Resource scope claim not found."], true);
            }

            return RedirectToAction("EditApiResource", new { siteId = siteId.ToString(), apiName = apiName });
        }

    }
}
