// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2016-10-17
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Controllers
{
    public class ClientsController : Controller
    {
        public ClientsController(
            SiteManager siteManager,
            ClientsManager clientsManager,
            IStringLocalizer<CloudscribeIntegration> localizer
            )
        {
            this.siteManager = siteManager;
            this.clientsManager = clientsManager;
            sr = localizer;

        }

        private SiteManager siteManager;
        private ClientsManager clientsManager;
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
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Client Management"], selectedSite.SiteName);
            }
            else
            {
                ViewData["Title"] = sr["Client Management"];
            }

            int itemsPerPage = 10;
            if (pageSize > 0)
            {
                itemsPerPage = pageSize;
            }
            var model = new ClientListViewModel();
            model.SiteId = selectedSite.Id.ToString();
            var result = await clientsManager.GetClients(selectedSite.Id.ToString(), pageNumber, itemsPerPage);
            model.Clients = result.Data;

            model.Paging.CurrentPage = pageNumber;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = result.TotalItems;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditClient(
            Guid? siteId,
            string clientId = null)
        {
            
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            if (!string.IsNullOrEmpty(clientId))
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - Edit Client"], selectedSite.SiteName);
            }

            var model = new ClientEditViewModel();
            model.SiteId = selectedSite.Id.ToString();
            //model.NewScope.SiteId = model.SiteId;
            if (!string.IsNullOrEmpty(clientId))
            {
                var client = await clientsManager.FetchClient(model.SiteId, clientId);
                model.CurrentClient = new ClientItemViewModel(model.SiteId, client);
            }

            if (model.CurrentClient == null)
            {
                ViewData["Title"] = string.Format(CultureInfo.CurrentUICulture, sr["{0} - New Client"], selectedSite.SiteName);
                var currentCrumbAdjuster = new NavigationNodeAdjuster(Request.HttpContext);
                currentCrumbAdjuster.KeyToAdjust = "EditClient";
                currentCrumbAdjuster.AdjustedText = sr["New Client"];
                currentCrumbAdjuster.AddToContext();
            }
            else
            {
                //model.NewScopeClaim.SiteId = model.SiteId;
                //model.NewScopeClaim.ScopeName = model.CurrentScope.Name;
                //model.NewScopeSecret.SiteId = model.SiteId;
                //model.NewScopeSecret.ScopeName = model.CurrentScope.Name;
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditClient(ClientItemViewModel clientModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = clientModel.SiteId, clientId = clientModel.ClientId });
            }

            Guid siteId = siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(clientModel.SiteId) && clientModel.SiteId.Length == 36)
            {
                siteId = new Guid(clientModel.SiteId);
            }
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientModel.ClientId);

            if (client == null)
            {
                this.AlertDanger(sr["Client not found"], true);
                return RedirectToAction("Index", new { siteId = selectedSite.Id.ToString() });
            }

            client.AbsoluteRefreshTokenLifetime = clientModel.AbsoluteRefreshTokenLifetime;
            client.AccessTokenLifetime = clientModel.AccessTokenLifetime;
            client.AccessTokenType = clientModel.AccessTokenType;
            client.AllowAccessToAllScopes = clientModel.AllowAccessToAllScopes;
            client.AllowAccessTokensViaBrowser = clientModel.AllowAccessTokensViaBrowser;
            client.AllowRememberConsent = clientModel.AllowRememberConsent;
            client.AlwaysSendClientClaims = clientModel.AlwaysSendClientClaims;
            client.AuthorizationCodeLifetime = clientModel.AuthorizationCodeLifetime;
            client.ClientName = clientModel.ClientName;
            client.ClientUri = clientModel.ClientUri;
            client.Enabled = clientModel.Enabled;
            client.EnableLocalLogin = clientModel.EnableLocalLogin;
            client.IdentityTokenLifetime = clientModel.IdentityTokenLifetime;
            client.IncludeJwtId = clientModel.IncludeJwtId;
            client.LogoUri = clientModel.LogoUri;
            client.LogoutSessionRequired = clientModel.LogoutSessionRequired;
            client.LogoutUri = clientModel.LogoutUri;
            client.PrefixClientClaims = clientModel.PrefixClientClaims;
            client.RefreshTokenExpiration = clientModel.RefreshTokenExpiration;
            client.RefreshTokenUsage = clientModel.RefreshTokenUsage;
            client.RequireClientSecret = clientModel.RequireClientSecret;
            client.RequireConsent = clientModel.RequireConsent;
            client.RequirePkce = clientModel.RequirePkce;
            client.SlidingRefreshTokenLifetime = clientModel.SlidingRefreshTokenLifetime;
            client.UpdateAccessTokenClaimsOnRefresh = clientModel.UpdateAccessTokenClaimsOnRefresh;

            // TODO: separate actions and views for each collection
            //client.AllowedCorsOrigins = clientModel.
            //client.AllowedGrantTypes
            //client.AllowedScopes
            //client.Claims
            //client.ClientSecrets
            //client.IdentityProviderRestrictions
            //client.PostLogoutRedirectUris
            //client.RedirectUris
            
            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            var successFormat = sr["The Client <b>{0}</b> was successfully updated."];

            this.AlertSuccess(string.Format(successFormat, client.ClientId), true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = client.ClientId });
        }


    }
}
