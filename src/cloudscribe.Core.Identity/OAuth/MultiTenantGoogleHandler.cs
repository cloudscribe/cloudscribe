// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-29
// Last Modified:		    2015-11-18
// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Google/GoogleHandler.cs



using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity.OAuth
{
    internal class MultiTenantGoogleHandler : MultiTenantOAuthHandler<GoogleOptions>
    {
        public MultiTenantGoogleHandler(
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
            log = loggerFactory.CreateLogger<MultiTenantGoogleHandler>();
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            siteRepo = siteRepository;
        }

        private ILogger log;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity, 
            AuthenticationProperties properties, 
            OAuthTokenResponse tokens)
        {
            log.LogDebug("CreateTicketAsync called tokens.AccessToken was " + tokens.AccessToken);

            // Get the Google user
            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
            //string r = await response.Content.ReadAsStringAsync();
            //log.LogInformation(r);
            response.EnsureSuccessStatusCode();


            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload)
            {
                Properties = properties,
                Principal = new ClaimsPrincipal(identity)
            };

            var identifier = GoogleHelper.GetId(payload);
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var givenName = GoogleHelper.GetGivenName(payload);
            if (!string.IsNullOrEmpty(givenName))
            {
                identity.AddClaim(new Claim(ClaimTypes.GivenName, givenName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var familyName = GoogleHelper.GetFamilyName(payload);
            if (!string.IsNullOrEmpty(familyName))
            {
                identity.AddClaim(new Claim(ClaimTypes.Surname, familyName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var name = GoogleHelper.GetName(payload);
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var email = GoogleHelper.GetEmail(payload);
            if (!string.IsNullOrEmpty(email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var profile = GoogleHelper.GetProfile(payload);
            if (!string.IsNullOrEmpty(profile))
            {
                identity.AddClaim(new Claim("urn:google:profile", profile, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            await Options.Events.CreatingTicket(context);

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
            return new AuthenticationTicket(context.Principal, context.Properties, AuthenticationScheme.External);
        }

        // TODO: Abstract this properties override pattern into the base class?
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var scope = FormatScope();

            var tenantOptions = new MultiTenantGoogleOptionsResolver(Options, siteResolver, siteRepo, multiTenantOptions);
            string resolvedRedirectUri = tenantOptions.ResolveRedirectUrl(redirectUri);
            log.LogDebug("resolvedRedirectUri was " + resolvedRedirectUri);

            var queryStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            queryStrings.Add("response_type", "code");
            //queryStrings.Add("client_id", Options.ClientId);
            //queryStrings.Add("redirect_uri", redirectUri);
            queryStrings.Add("client_id", tenantOptions.ClientId);
            queryStrings.Add("redirect_uri", resolvedRedirectUri);

            AddQueryString(queryStrings, properties, "scope", scope);

            AddQueryString(queryStrings, properties, "access_type", Options.AccessType);
            AddQueryString(queryStrings, properties, "approval_prompt");
            AddQueryString(queryStrings, properties, "login_hint");

            var state = Options.StateDataFormat.Protect(properties);
            queryStrings.Add("state", state);

            var authorizationEndpoint = QueryHelpers.AddQueryString(Options.AuthorizationEndpoint, queryStrings);
            return authorizationEndpoint;
        }

        private static void AddQueryString(IDictionary<string, string> queryStrings, AuthenticationProperties properties,
            string name, string defaultValue = null)
        {
            string value;
            if (!properties.Items.TryGetValue(name, out value))
            {
                value = defaultValue;
            }
            else
            {
                // Remove the parameter from AuthenticationProperties so it won't be serialized to state parameter
                properties.Items.Remove(name);
            }

            if (value == null)
            {
                return;
            }

            queryStrings[name] = value;
        }

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

            var tenantOptions = new MultiTenantGoogleOptionsResolver(Options, siteResolver, siteRepo, multiTenantOptions);

            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", tenantOptions.ClientId },
                { "redirect_uri", tenantOptions.ResolveRedirectUrl(redirectUri) },
                { "client_secret", tenantOptions.ClientSecret },
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

    }
}
