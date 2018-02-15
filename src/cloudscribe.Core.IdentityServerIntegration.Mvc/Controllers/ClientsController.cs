// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-10-13
// Last Modified:			2018-01-01
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServerIntegration.Controllers.Mvc
{
    [Authorize(Policy = "IdentityServerAdminPolicy")]
    public class ClientsController : Controller
    {
        public ClientsController(
            SiteManager siteManager,
            ClientsManager clientsManager,
            IStringLocalizer<CloudscribeIds4Resources> localizer
            )
        {
            this.siteManager = siteManager;
            this.clientsManager = clientsManager;
            sr = localizer;

        }

        private SiteManager siteManager;
        private ClientsManager clientsManager;
        private IStringLocalizer sr;

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
            model.Clients = result;

            //model.Paging.CurrentPage = pageNumber;
            //model.Paging.ItemsPerPage = itemsPerPage;
            //model.Paging.TotalItems = result.TotalItems;

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
            model.NewClient.SiteId = model.SiteId;
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
            client.AllowOfflineAccess = clientModel.AllowOfflineAccess;
            //client.AllowAccessToAllScopes = clientModel.AllowAccessToAllScopes;
            client.AlwaysIncludeUserClaimsInIdToken = clientModel.AlwaysIncludeUserClaimsInIdToken;
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
            client.FrontChannelLogoutSessionRequired = clientModel.FrontChannelLogoutSessionRequired;
            client.FrontChannelLogoutUri = clientModel.FrontChannelLogoutUri;
            client.BackChannelLogoutSessionRequired = clientModel.BackChannelLogoutSessionRequired;
            client.BackChannelLogoutUri = clientModel.BackChannelLogoutUri;
            //Consider making client claims prefix value configurable
            //https://github.com/IdentityServer/IdentityServer4/issues/1534
            //PrefixClientClaims = client.PrefixClientClaims;
            //client.PrefixClientClaims = clientModel.PrefixClientClaims;
            client.ClientClaimsPrefix = clientModel.ClientClaimsPrefix;

            client.PairWiseSubjectSalt = clientModel.PairWiseSubjectSalt;
            client.RefreshTokenExpiration = clientModel.RefreshTokenExpiration;
            client.RefreshTokenUsage = clientModel.RefreshTokenUsage;
            client.RequireClientSecret = clientModel.RequireClientSecret;
            client.RequireConsent = clientModel.RequireConsent;
            client.RequirePkce = clientModel.RequirePkce;
            client.SlidingRefreshTokenLifetime = clientModel.SlidingRefreshTokenLifetime;
            client.UpdateAccessTokenClaimsOnRefresh = clientModel.UpdateAccessTokenClaimsOnRefresh;
            
            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            var successFormat = sr["The Client <b>{0}</b> was successfully updated."];

            this.AlertSuccess(string.Format(successFormat, client.ClientId), true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = client.ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> NewClient(NewClientViewModel clientModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = clientModel.SiteId });
            }

            Guid siteId = siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(clientModel.SiteId) && clientModel.SiteId.Length == 36)
            {
                siteId = new Guid(clientModel.SiteId);
            }
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var found = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientModel.ClientId);

            if (found != null)
            {
                this.AlertDanger(sr["Client already exists with that client id"], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = found.ClientId });
            }

            var client = new Client();
            client.ClientId = clientModel.ClientId;
            client.ClientName = clientModel.ClientName;
            client.AccessTokenType = clientModel.AccessTokenType;
            client.RefreshTokenExpiration = clientModel.RefreshTokenExpiration;
            client.RefreshTokenUsage = clientModel.RefreshTokenUsage;

            await clientsManager.CreateClient(selectedSite.Id.ToString(), client);

            var successFormat = sr["The Client <b>{0}</b> was successfully Created."];

            this.AlertSuccess(string.Format(successFormat, client.ClientId), true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = client.ClientId });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteClient(Guid siteId, string clientId)
        {
            await clientsManager.DeleteClient(siteId.ToString(), clientId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddClientClaim(NewClientClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = model.SiteId, clientId = model.ClientId });
            }

            Guid siteId = siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), model.ClientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, Client not found."], true);
                return RedirectToAction("Index");
            }

            var claim = new Claim(model.Type, model.Value);
            
            if (client.HasClaim(claim))
            {
                this.AlertDanger(sr["Client already has a claim with that type and value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = model.ClientId });
            }
            client.Claims.Add(claim);

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            var successFormat = sr["The Claim <b>{0}</b> was successfully added."];

            this.AlertSuccess(string.Format(successFormat, claim.Type), true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = model.ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientClaim(Guid siteId, string clientId, string claimType, string claimValue)
        {
            var client = await clientsManager.FetchClient(siteId.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            Claim found = null;
            foreach (var c in client.Claims)
            {
                if (c.Type == claimType && c.Value == claimValue)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                client.Claims.Remove(found);
                await clientsManager.UpdateClient(siteId.ToString(), client);
                var successFormat = sr["The Claim <b>{0}</b> was successfully removed."];
                this.AlertSuccess(string.Format(successFormat, claimType), true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, client claim not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientProperty(NewClientPropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = model.SiteId, clientId = model.ClientId });
            }

            Guid siteId = siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), model.ClientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, Client not found."], true);
                return RedirectToAction("Index");
            }

            if(client.Properties.Keys.Contains(model.Key))
            {
                //client.Properties[model.Key] = model.Value;
                this.AlertDanger(sr["Client already has a property with that key, to change a proprty you must delete it and then add it back with the new value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = model.ClientId });
            }
            else
            {
                client.Properties.Add(model.Key, model.Value);
            }
            
            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            var successFormat = sr["The property <b>{0}</b> was successfully added."];

            this.AlertSuccess(string.Format(successFormat, model.Key), true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = model.ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientProperty(Guid siteId, string clientId, string key, string value)
        {
            var client = await clientsManager.FetchClient(siteId.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            if (!client.Properties.Keys.Contains(key))
            {
                this.AlertDanger(sr["Invalid request, client property not found."], true);
            }

            client.Properties.Remove(key);
            
            await clientsManager.UpdateClient(siteId.ToString(), client);
            var successFormat = sr["The property <b>{0}</b> was successfully removed."];
            this.AlertSuccess(string.Format(successFormat, key), true);
            
            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }


        [HttpPost]
        public async Task<IActionResult> AddClientSecret(NewClientSecretViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = model.SiteId, clientId = model.ClientId });
            }

            Guid siteId = siteManager.CurrentSite.Id;
            if (!string.IsNullOrEmpty(model.SiteId) && model.SiteId.Length == 36)
            {
                siteId = new Guid(model.SiteId);
            }
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), model.ClientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            
            var secretValue = model.Value;
            if(string.IsNullOrEmpty(model.Type) || model.Type == IdentityServerConstants.SecretTypes.SharedSecret)
            {
                switch (model.HashOption)
                {
                    case "sha256":
                        secretValue = model.Value.Sha256();
                        break;
                    case "sha512":
                        secretValue = model.Value.Sha512();
                        break;
                }
            }
            
            var secret = new Secret(secretValue, model.Description, model.Expiration);
            secret.Type = model.Type;

            if (client.ClientSecrets.Contains(secret))
            {
                this.AlertDanger(sr["Client already has a secret with that value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = model.ClientId });
            }
            client.ClientSecrets.Add(secret);

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            this.AlertSuccess(sr["The Secret was successfully added."], true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = model.ClientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientSecret(Guid siteId, string clientId, string secretValue)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            Secret found = null;
            foreach (var c in client.ClientSecrets)
            {
                if (c.Value == secretValue)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                client.ClientSecrets.Remove(found);
                await clientsManager.UpdateClient(siteId.ToString(), client);
                this.AlertSuccess(sr["The Secret was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, client secret not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientRedirect(Guid siteId, string clientId, string redirectUri)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = siteId, clientId = clientId });
            }

            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }
            
            if (client.RedirectUris.Contains(redirectUri))
            {
                this.AlertDanger(sr["Client already has a Redirect Uri with that value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
            }
            client.RedirectUris.Add(redirectUri);

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            this.AlertSuccess(sr["The Redirect Uri was successfully added."], true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientRedirect(Guid siteId, string clientId, string redirectUri)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, Client not found."], true);
                return RedirectToAction("Index");
            }

            string found = null;
            foreach (var c in client.RedirectUris)
            {
                if (c == redirectUri)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                client.RedirectUris.Remove(found);
                await clientsManager.UpdateClient(siteId.ToString(), client);
                this.AlertSuccess(sr["The Redirect Url was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, Redirect Url not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientLogoutRedirect(Guid siteId, string clientId, string logoutRedirectUri)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = siteId, clientId = clientId });
            }

            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            if (client.PostLogoutRedirectUris.Contains(logoutRedirectUri))
            {
                this.AlertDanger(sr["Client already has a Logout Redirect Url with that value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
            }
            client.PostLogoutRedirectUris.Add(logoutRedirectUri);

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            this.AlertSuccess(sr["The Logout Redirect Url was successfully added."], true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientLogoutRedirect(Guid siteId, string clientId, string logoutRedirectUri)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            string found = null;
            foreach (var c in client.PostLogoutRedirectUris)
            {
                if (c == logoutRedirectUri)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                client.PostLogoutRedirectUris.Remove(found);
                await clientsManager.UpdateClient(siteId.ToString(), client);
                this.AlertSuccess(sr["The Logout Redirect Url was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, Logout Redirect Url not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientCorsOrigin(Guid siteId, string clientId, string corsOrigin)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = siteId, clientId = clientId });
            }

            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            if (client.AllowedCorsOrigins.Contains(corsOrigin))
            {
                this.AlertDanger(sr["Client already has a CORS Origin with that value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
            }
            client.AllowedCorsOrigins.Add(corsOrigin);

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            this.AlertSuccess(sr["The CORS Origin was successfully added."], true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientCorsOrigin(Guid siteId, string clientId, string corsOrigin)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            string found = null;
            foreach (var c in client.AllowedCorsOrigins)
            {
                if (c == corsOrigin)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                client.AllowedCorsOrigins.Remove(found);
                await clientsManager.UpdateClient(siteId.ToString(), client);
                this.AlertSuccess(sr["The CORS Origin was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, CORS Origin not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientGrantType(Guid siteId, string clientId, string grantType)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = siteId, clientId = clientId });
            }

            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            var grants = client.AllowedGrantTypes.ToList();
            if (grants.Contains(grantType))
            {
                this.AlertDanger(sr["Client already has a Grant Type with that value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
            }

            grants.Add(grantType);
            // error here can't have both implicit and hybrid
            try
            {
                client.AllowedGrantTypes = grants;
            }
            catch(InvalidOperationException)
            {
                grants.Clear();
                grants.Add(grantType);
                client.AllowedGrantTypes = grants;

                this.AlertWarning(sr["There was a conflicting grant type that had to be removed in order to add the new one."], true);
            }
            

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            this.AlertSuccess(sr["The Grant Type was successfully added."], true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientGrantType(Guid siteId, string clientId, string grantType)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            var grants = client.AllowedGrantTypes.ToList();
            string found = null;
            foreach (var c in grants)
            {
                if (c == grantType)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                var didRemove = true;
                grants.Remove(found);
                // empty is not allowed, default is implicit
                if(grants.Count == 0)
                {
                    grants.Add(GrantType.Implicit);
                    this.AlertWarning(sr["A client must have at least one grant type, implicit is the default grant type."], true);
                    didRemove = false;
                }
                
                client.AllowedGrantTypes = grants;
                await clientsManager.UpdateClient(siteId.ToString(), client);
                if(didRemove)
                {
                    this.AlertSuccess(sr["The Grant Type was successfully removed."], true);
                }
                
            }
            else
            {
                this.AlertDanger(sr["Invalid request, Grant Type not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientScope(Guid siteId, string clientId, string scopeName)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = siteId, clientId = clientId });
            }

            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            if (client.AllowedScopes.Contains(scopeName))
            {
                this.AlertDanger(sr["Client already has a Scope with that value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
            }
            client.AllowedScopes.Add(scopeName);

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            this.AlertSuccess(sr["The Scope was successfully added."], true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientScope(Guid siteId, string clientId, string scopeName)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            string found = null;
            foreach (var c in client.AllowedScopes)
            {
                if (c == scopeName)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                client.AllowedScopes.Remove(scopeName);
                await clientsManager.UpdateClient(siteId.ToString(), client);
                this.AlertSuccess(sr["The Scope was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, Scope not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> AddClientRestriction(Guid siteId, string clientId, string restriction)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditClient", new { siteId = siteId, clientId = clientId });
            }

            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            if (client.IdentityProviderRestrictions.Contains(restriction))
            {
                this.AlertDanger(sr["Client already has a Restriction with that value."], true);
                return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
            }
            client.IdentityProviderRestrictions.Add(restriction);

            await clientsManager.UpdateClient(selectedSite.Id.ToString(), client);

            this.AlertSuccess(sr["The Restriction was successfully added."], true);

            return RedirectToAction("EditClient", new { siteId = selectedSite.Id.ToString(), clientId = clientId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClientRestriction(Guid siteId, string clientId, string restriction)
        {
            var selectedSite = await siteManager.GetSiteForDataOperations(siteId);

            var client = await clientsManager.FetchClient(selectedSite.Id.ToString(), clientId);
            if (client == null)
            {
                this.AlertDanger(sr["Invalid request, client not found."], true);
                return RedirectToAction("Index");
            }

            string found = null;
            foreach (var c in client.IdentityProviderRestrictions)
            {
                if (c == restriction)
                {
                    found = c;
                    break;
                }
            }
            if (found != null)
            {
                client.IdentityProviderRestrictions.Remove(restriction);
                await clientsManager.UpdateClient(siteId.ToString(), client);
                this.AlertSuccess(sr["The Restriction was successfully removed."], true);
            }
            else
            {
                this.AlertDanger(sr["Invalid request, Restriction not found."], true);
            }

            return RedirectToAction("EditClient", new { siteId = siteId.ToString(), clientId = clientId });
        }

    }
}
