//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:					2016-10-13
//// Last Modified:			2016-10-21
//// 

//using cloudscribe.Core.IdentityServerIntegration.Models;
//using cloudscribe.Core.IdentityServerIntegration.Services;
//using cloudscribe.Core.Web.Components;
//using cloudscribe.Web.Common.Extensions;
//using cloudscribe.Web.Navigation;
//using IdentityServer4.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Localization;
//using System;
//using System.Globalization;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.IdentityServerIntegration.Controllers.Mvc
//{
//    [Authorize(Policy = "IdentityServerAdminPolicy")]
//    public class ScopesController : Controller
//    {
//        public ScopesController(
//            SiteManager siteManager,
//            ScopesManager scopesManager,
//            IStringLocalizer<CloudscribeIntegration> localizer
//            )
//        {
//            this.siteManager = siteManager;
//            this.scopesManager = scopesManager;
//            sr = localizer;
//        }

//        private SiteManager siteManager;
//        private ScopesManager scopesManager;
//        private IStringLocalizer<CloudscribeIntegration> sr;

//        [HttpGet]
//        public async Task<IActionResult> Index(
//            Guid? siteId,
//            int pageNumber = 1,
//            int pageSize = -1
//            )
//        {
//            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);
//            // only server admin site can edit other sites settings
//            if (selectedSite.Id != siteManager.CurrentSite.Id)
//            {
//                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Scope Management"], selectedSite.SiteName);
//            }
//            else
//            {
//                ViewData["Title"] = sr["Scope Management"];
//            }

//            int itemsPerPage = 10;
//            if (pageSize > 0)
//            {
//                itemsPerPage = pageSize;
//            }
//            var model = new ScopeListViewModel();
//            model.SiteId = selectedSite.Id.ToString();
//            var result = await scopesManager.GetScopes(selectedSite.Id.ToString(), pageNumber, itemsPerPage);
//            model.Scopes = result.Data;

//            model.Paging.CurrentPage = pageNumber;
//            model.Paging.ItemsPerPage = itemsPerPage;
//            model.Paging.TotalItems = result.TotalItems;

//            return View(model);
//        }

//        [HttpGet]
//        public async Task<IActionResult> EditScope(
//            Guid? siteId,
//            string scopeName = null)
//        {

//            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

//            if (!string.IsNullOrEmpty(scopeName))
//            {
//                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Edit Scope"], selectedSite.SiteName);
//            }

//            var model = new ScopeEditViewModel();
//            model.SiteId = selectedSite.Id.ToString();
//            model.NewScope.SiteId = model.SiteId;
//            if (!string.IsNullOrEmpty(scopeName))
//            {
//                var scope = await scopesManager.FetchScope(model.SiteId, scopeName);
//                model.CurrentScope = scope;
//            }

//            if (model.CurrentScope == null)
//            {
//                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New Scope"], selectedSite.SiteName);
//                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
//                currentCrumbAdjuster.KeyToAdjust = "EditScope";
//                currentCrumbAdjuster.AdjustedText = sr["New Scope"];
//                currentCrumbAdjuster.AddToContext();
//            }
//            else
//            {
//                model.NewScopeClaim.SiteId = model.SiteId;
//                model.NewScopeClaim.ScopeName = model.CurrentScope.Name;
//                model.NewScopeSecret.SiteId = model.SiteId;
//                model.NewScopeSecret.ScopeName = model.CurrentScope.Name;
//            }

//            return View(model);

//        }

//        [HttpPost]
//        public async Task<IActionResult> EditScope(ScopeItemViewModel scopeModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return RedirectToAction("EditScope", new { siteId = scopeModel.SiteId, scopeName = scopeModel.Name });
//            }

//            Guid siteId = siteManager.CurrentSite.Id;
//            if (!string.IsNullOrEmpty(scopeModel.SiteId) && scopeModel.SiteId.Length == 36)
//            {
//                siteId = new Guid(scopeModel.SiteId);
//            }
//            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

//            var scope = await scopesManager.FetchScope(selectedSite.Id.ToString(), scopeModel.Name);

//            if (scope == null)
//            {
//                this.AlertDanger(sr["Scope not found"], true);
//                return RedirectToAction("Index", new { siteId = selectedSite.Id.ToString() });
//            }

//            scope.AllowUnrestrictedIntrospection = scopeModel.AllowUnrestrictedIntrospection;
//            scope.ClaimsRule = scopeModel.ClaimsRule;
//            scope.Description = scopeModel.Description;
//            scope.DisplayName = scopeModel.DisplayName;
//            scope.Emphasize = scopeModel.Emphasize;
//            scope.Enabled = scopeModel.Enabled;
//            scope.IncludeAllClaimsForUser = scopeModel.IncludeAllClaimsForUser;
//            scope.Required = scopeModel.Required;
//            scope.ShowInDiscoveryDocument = scopeModel.ShowInDiscoveryDocument;
//            await scopesManager.UpdateScope(selectedSite.Id.ToString(), scope);

//            var successFormat = sr["The Scope <b>{0}</b> was successfully updated."];

//            this.AlertSuccess(string.Format(successFormat, scope.Name), true);

//            return RedirectToAction("EditScope", new { siteId = selectedSite.Id.ToString(), scopeName = scope.Name });

//        }

//        [HttpPost]
//        public async Task<IActionResult> NewScope(ScopeItemViewModel scopeModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return RedirectToAction("EditScope", new { siteId = scopeModel.SiteId, scopeName = scopeModel.Name });
//            }

//            Guid siteId = siteManager.CurrentSite.Id;
//            if (!string.IsNullOrEmpty(scopeModel.SiteId) && scopeModel.SiteId.Length == 36)
//            {
//                siteId = new Guid(scopeModel.SiteId);
//            }
//            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

//            ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New Scope"], selectedSite.SiteName);

//            var exists = await scopesManager.ScopeExists(selectedSite.Id.ToString(), scopeModel.Name);
//            var scopeType = scopesManager.ResolveScopeType(scopeModel.ScopeType);
//            if (exists || !scopeType.HasValue)
//            {
//                var model = new ScopeEditViewModel();
//                model.SiteId = selectedSite.Id.ToString();
//                model.NewScope = scopeModel;
//                model.NewScope.SiteId = model.SiteId;

//                if (exists) ModelState.AddModelError("scopenameinuseerror", sr["Scope name is already in use"]);
//                if (!scopeType.HasValue) ModelState.AddModelError("invalidscopetypeerror", sr["Invalid Scope Type"]);

//                return View("EditScope", model);
//            }

//            var scope = new Scope
//            {
//                Type = scopeType.Value,
//                Name = scopeModel.Name,
//                DisplayName = scopeModel.DisplayName,
//                Description = scopeModel.Description
//            };

//            await scopesManager.CreateScope(selectedSite.Id.ToString(), scope);

//            var successFormat = sr["The Scope <b>{0}</b> was successfully Created."];

//            this.AlertSuccess(string.Format(successFormat, scope.Name), true);

//            return RedirectToAction("EditScope", new { siteId = selectedSite.Id.ToString(), scopeName = scope.Name });
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteScope(Guid siteId, string scopeName)
//        {
//            await scopesManager.DeleteScope(siteId.ToString(), scopeName);
//            return RedirectToAction("Index");
//        }

//        // Scope Claims
//        // List of user claims that should be included in the identity (identity scope) or access token (resource scope).

//        [HttpPost]
//        public async Task<IActionResult> AddScopeClaim(NewScopeClaimViewModel scopeModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return RedirectToAction("EditScope", new { siteId = scopeModel.SiteId, scopeName = scopeModel.ScopeName });
//            }

//            Guid siteId = siteManager.CurrentSite.Id;
//            if (!string.IsNullOrEmpty(scopeModel.SiteId) && scopeModel.SiteId.Length == 36)
//            {
//                siteId = new Guid(scopeModel.SiteId);
//            }
//            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

//            var scope = await scopesManager.FetchScope(selectedSite.Id.ToString(), scopeModel.ScopeName);
//            if (scope == null)
//            {
//                this.AlertDanger(sr["Invalid request, scope not found."], true);
//                return RedirectToAction("Index");
//            }

//            var claim = new ScopeClaim(scopeModel.Name, scopeModel.AlwaysIncludeInIdToken);
//            claim.Description = scopeModel.Description;

//            if (scope.Claims.Contains(claim))
//            {
//                this.AlertDanger(sr["Scope already has a claim with that name."], true);
//                return RedirectToAction("EditScope", new { siteId = selectedSite.Id.ToString(), scopeName = scopeModel.ScopeName });
//            }
//            scope.Claims.Add(claim);

//            await scopesManager.UpdateScope(selectedSite.Id.ToString(), scope);

//            var successFormat = sr["The Claim <b>{0}</b> was successfully added."];

//            this.AlertSuccess(string.Format(successFormat, claim.Name), true);

//            return RedirectToAction("EditScope", new { siteId = selectedSite.Id.ToString(), scopeName = scopeModel.ScopeName });
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteScopeClaim(Guid siteId, string scopeName, string claimName)
//        {
//            var scope = await scopesManager.FetchScope(siteId.ToString(), scopeName);
//            if (scope == null)
//            {
//                this.AlertDanger(sr["Invalid request, scope not found."], true);
//                return RedirectToAction("Index");
//            }

//            ScopeClaim found = null;
//            foreach (var c in scope.Claims)
//            {
//                if (c.Name == claimName)
//                {
//                    found = c;
//                    break;
//                }
//            }
//            if (found != null)
//            {
//                scope.Claims.Remove(found);
//                await scopesManager.UpdateScope(siteId.ToString(), scope);
//                var successFormat = sr["The Claim <b>{0}</b> was successfully removed."];
//                this.AlertSuccess(string.Format(successFormat, claimName), true);
//            }
//            else
//            {
//                this.AlertDanger(sr["Invalid request, scope claim not found."], true);
//            }

//            return RedirectToAction("EditScope", new { siteId = siteId.ToString(), scopeName = scopeName });
//        }

//        //ScopeSecrets
//        // https://identityserver.github.io/Documentation/docsv2/configuration/secrets.html
//        // Secrets define how machines (e.g. a client or a scope) can authenticate with IdentityServer.
//        // Value The value of the secret. This is being interpreted by the secret validator 
//        // (e.g. a “password”-like share secret or something else that identifies a credential)
//        // Description The description of the secret - useful for attaching some extra information to the secret
//        // Expiration A point in time, where this secret will expire
//        // Type Some string that gives the secret validator a hint what type of secret to expect (e.g. “SharedSecret” or “X509CertificateThumbprint”)

//        [HttpPost]
//        public async Task<IActionResult> AddScopeSecret(NewScopeSecretViewModel scopeModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                return RedirectToAction("EditScope", new { siteId = scopeModel.SiteId, scopeName = scopeModel.ScopeName });
//            }

//            Guid siteId = siteManager.CurrentSite.Id;
//            if (!string.IsNullOrEmpty(scopeModel.SiteId) && scopeModel.SiteId.Length == 36)
//            {
//                siteId = new Guid(scopeModel.SiteId);
//            }
//            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

//            var scope = await scopesManager.FetchScope(selectedSite.Id.ToString(), scopeModel.ScopeName);
//            if (scope == null)
//            {
//                this.AlertDanger(sr["Invalid request, scope not found."], true);
//                return RedirectToAction("Index");
//            }

//            var secret = new Secret(scopeModel.Value, scopeModel.Description, scopeModel.Expiration);
//            secret.Type = scopeModel.Type;

//            if (scope.ScopeSecrets.Contains(secret))
//            {
//                this.AlertDanger(sr["Scope already has a secret with that value."], true);
//                return RedirectToAction("EditScope", new { siteId = selectedSite.Id.ToString(), scopeName = scopeModel.ScopeName });
//            }
//            scope.ScopeSecrets.Add(secret);

//            await scopesManager.UpdateScope(selectedSite.Id.ToString(), scope);

//            this.AlertSuccess(sr["The Secret was successfully added."], true);

//            return RedirectToAction("EditScope", new { siteId = selectedSite.Id.ToString(), scopeName = scopeModel.ScopeName });
//        }

//        [HttpPost]
//        public async Task<IActionResult> DeleteScopeSecret(Guid siteId, string scopeName, string secretValue)
//        {
//            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

//            var scope = await scopesManager.FetchScope(selectedSite.Id.ToString(), scopeName);
//            if (scope == null)
//            {
//                this.AlertDanger(sr["Invalid request, scope not found."], true);
//                return RedirectToAction("Index");
//            }

//            Secret found = null;
//            foreach (var c in scope.ScopeSecrets)
//            {
//                if (c.Value == secretValue)
//                {
//                    found = c;
//                    break;
//                }
//            }
//            if (found != null)
//            {
//                scope.ScopeSecrets.Remove(found);
//                await scopesManager.UpdateScope(siteId.ToString(), scope);
//                this.AlertSuccess(sr["The Secret was successfully removed."], true);
//            }
//            else
//            {
//                this.AlertDanger(sr["Invalid request, scope secret not found."], true);
//            }

//            return RedirectToAction("EditScope", new { siteId = siteId.ToString(), scopeName = scopeName });
//        }

//    }
//}
