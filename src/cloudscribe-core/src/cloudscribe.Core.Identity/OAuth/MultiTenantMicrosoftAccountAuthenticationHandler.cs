// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-29
// Last Modified:		    2015-09-09
// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.MicrosoftAccount/MicrosoftAccountAuthenticationHandler.cs


using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.Framework.Logging;
using cloudscribe.Core.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity.OAuth
{

    internal class MultiTenantMicrosoftAccountAuthenticationHandler : MultiTenantOAuthAuthenticationHandler<MicrosoftAccountAuthenticationOptions>
    {
        public MultiTenantMicrosoftAccountAuthenticationHandler(
            HttpClient httpClient,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            MultiTenantOptions multiTenantOptions,
            ILoggerFactory loggerFactory)
            : base(
                  httpClient, 
                  loggerFactory,
                  new MultiTenantOAuthOptionsResolver(siteResolver, multiTenantOptions)
                  )
        {
            log = loggerFactory.CreateLogger<MultiTenantMicrosoftAccountAuthenticationHandler>();
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            siteRepo = siteRepository;
        }

        private ILogger log;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            log.LogDebug("ExchangeCodeAsync called with code " + code + " redirectUri " + redirectUri);

            //var tokenRequestParameters = new Dictionary<string, string>()
            //{
            //    { "client_id", Options.ClientId },
            //    { "redirect_uri", redirectUri },
            //    { "client_secret", Options.ClientSecret },
            //    { "code", code },
            //    { "grant_type", "authorization_code" },
            //};

            var tenantFbOptions = new MultiTenantMicrosoftOptionsResolver(Options, siteResolver, siteRepo, multiTenantOptions);

            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", tenantFbOptions.ClientId },
                { "redirect_uri", tenantFbOptions.ResolveRedirectUrl(redirectUri) },
                { "client_secret", tenantFbOptions.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new OAuthTokenResponse(payload);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            log.LogDebug("BuildChallengeUrl called with redirectUri = " + redirectUri);

            var scope = FormatScope();

            var state = Options.StateDataFormat.Protect(properties);

            //var queryBuilder = new QueryBuilder()
            //{
            //    { "client_id", Options.ClientId },
            //    { "scope", scope },
            //    { "response_type", "code" },
            //    { "redirect_uri", redirectUri },
            //    { "state", state },
            //};

            var tenantFbOptions = new MultiTenantMicrosoftOptionsResolver(Options, siteResolver, siteRepo, multiTenantOptions);
            string resolvedRedirectUri = tenantFbOptions.ResolveRedirectUrl(redirectUri);
            log.LogDebug("resolvedRedirectUri was " + resolvedRedirectUri);

            var queryBuilder = new QueryBuilder()
            {
                { "client_id", tenantFbOptions.ClientId },
                { "scope", scope },
                { "response_type", "code" },
                { "redirect_uri", resolvedRedirectUri  },
                { "state", state },
            };

            return Options.AuthorizationEndpoint + queryBuilder.ToString();
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            log.LogDebug("CreateTicketAsync called");

            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var notification = new OAuthAuthenticatedContext(Context, Options, Backchannel, tokens, payload)
            {
                Properties = properties,
                Principal = new ClaimsPrincipal(identity)
            };

            var identifier = MicrosoftAccountAuthenticationHelper.GetId(payload);
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
                identity.AddClaim(new Claim("urn:microsoftaccount:id", identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var name = MicrosoftAccountAuthenticationHelper.GetName(payload);
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, Options.ClaimsIssuer));
                identity.AddClaim(new Claim("urn:microsoftaccount:name", name, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var email = MicrosoftAccountAuthenticationHelper.GetEmail(payload);
            if (!string.IsNullOrEmpty(email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            await Options.Notifications.Authenticated(notification);

            ISiteSettings site = siteResolver.Resolve();

            if (site != null)
            {
                Claim siteGuidClaim = new Claim("SiteGuid", site.SiteGuid.ToString());
                if (!identity.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
                {
                    identity.AddClaim(siteGuidClaim);
                }

            }

            //return new AuthenticationTicket(notification.Principal, notification.Properties, notification.Options.AuthenticationScheme);
            return new AuthenticationTicket(notification.Principal, notification.Properties, AuthenticationScheme.External);
        }

    }
}
