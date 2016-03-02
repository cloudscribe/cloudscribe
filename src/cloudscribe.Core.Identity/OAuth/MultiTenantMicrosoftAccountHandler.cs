//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:                  Joe Audette
//// Created:                 2014-08-29
//// Last Modified:           2016-02-05
//// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.MicrosoftAccount/MicrosoftAccountHandler.cs


//using Microsoft.AspNet.Authentication;
//using Microsoft.AspNet.Authentication.MicrosoftAccount;
//using Microsoft.AspNet.Authentication.OAuth;
//using Microsoft.AspNet.Http;
//using Microsoft.AspNet.Http.Authentication;
//using Microsoft.AspNet.Http.Extensions;
//using Microsoft.Extensions.Logging;
//using cloudscribe.Core.Models;
//using Newtonsoft.Json.Linq;
//using SaasKit.Multitenancy;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Identity.OAuth
//{

//    internal class MultiTenantMicrosoftAccountHandler : MultiTenantOAuthHandler<MicrosoftAccountOptions>
//    {
//        public MultiTenantMicrosoftAccountHandler(
//            HttpClient httpClient,
//            //ISiteResolver siteResolver,
//            IHttpContextAccessor contextAccessor,
//            ITenantResolver<SiteSettings> siteResolver,
//            ISiteRepository siteRepository,
//            MultiTenantOptions multiTenantOptions,
//            ILoggerFactory loggerFactory)
//            : base(
//                  httpClient, 
//                  loggerFactory,
//                  new MultiTenantOAuthOptionsResolver(contextAccessor, siteResolver, multiTenantOptions)
//                  )
//        {
//            log = loggerFactory.CreateLogger<MultiTenantMicrosoftAccountHandler>();
//            this.contextAccessor = contextAccessor;
//            this.siteResolver = siteResolver;
//            this.multiTenantOptions = multiTenantOptions;
//            siteRepo = siteRepository;
//        }

//        private ILogger log;
//        private IHttpContextAccessor contextAccessor;
//        private ITenantResolver<SiteSettings> siteResolver;
//        private ISiteRepository siteRepo;
//        private MultiTenantOptions multiTenantOptions;

//        private ISiteSettings site = null;

//        private async Task<ISiteSettings> GetSite()
//        {
//            if (multiTenantOptions.UseRelatedSitesMode)
//            {
//                if (multiTenantOptions.Mode == MultiTenantMode.FolderName)
//                {
//                    CancellationToken cancellationToken
//                        = contextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

//                    site = await siteRepo.Fetch(multiTenantOptions.RelatedSiteId, cancellationToken);
//                    return site;
//                }
//            }

//            TenantContext<SiteSettings> tenantContext
//                = await siteResolver.ResolveAsync(contextAccessor.HttpContext);

//            if (tenantContext != null && tenantContext.Tenant != null)
//            {
//                site = tenantContext.Tenant;
//            }

//            return site;
//        }

//        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
//        {
//            log.LogDebug("ExchangeCodeAsync called with code " + code + " redirectUri " + redirectUri);

//            //var tokenRequestParameters = new Dictionary<string, string>()
//            //{
//            //    { "client_id", Options.ClientId },
//            //    { "redirect_uri", redirectUri },
//            //    { "client_secret", Options.ClientSecret },
//            //    { "code", code },
//            //    { "grant_type", "authorization_code" },
//            //};

//            var currentSite = await GetSite();

//            var tenantFbOptions = new MultiTenantMicrosoftOptionsResolver(
//                Options,
//                currentSite, 
//                multiTenantOptions);

//            var tokenRequestParameters = new Dictionary<string, string>()
//            {
//                { "client_id", tenantFbOptions.ClientId },
//                { "redirect_uri", tenantFbOptions.ResolveRedirectUrl(redirectUri) },
//                { "client_secret", tenantFbOptions.ClientSecret },
//                { "code", code },
//                { "grant_type", "authorization_code" },
//            };

//            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

//            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
//            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            requestMessage.Content = requestContent;
//            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
//            response.EnsureSuccessStatusCode();
//            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

//            return new OAuthTokenResponse(payload);
//        }

//        protected override async Task<string> BuildChallengeUrl(
//            AuthenticationProperties properties, 
//            string redirectUri)
//        {
//            log.LogDebug("BuildChallengeUrl called with redirectUri = " + redirectUri);

//            var scope = FormatScope();

//            var state = Options.StateDataFormat.Protect(properties);

//            //var queryBuilder = new QueryBuilder()
//            //{
//            //    { "client_id", Options.ClientId },
//            //    { "scope", scope },
//            //    { "response_type", "code" },
//            //    { "redirect_uri", redirectUri },
//            //    { "state", state },
//            //};

//            var currentSite = await GetSite();

//            var tenantFbOptions = new MultiTenantMicrosoftOptionsResolver(
//                Options,
//                currentSite, 
//                multiTenantOptions);


//            string resolvedRedirectUri = tenantFbOptions.ResolveRedirectUrl(redirectUri);
//            log.LogDebug("resolvedRedirectUri was " + resolvedRedirectUri);

//            var queryBuilder = new QueryBuilder()
//            {
//                { "client_id", tenantFbOptions.ClientId },
//                { "scope", scope },
//                { "response_type", "code" },
//                { "redirect_uri", resolvedRedirectUri  },
//                { "state", state },
//            };

//            return Options.AuthorizationEndpoint + queryBuilder.ToString();
//        }

//        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
//        {
//            log.LogDebug("CreateTicketAsync called");

//            var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
//            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

//            var response = await Backchannel.SendAsync(request, Context.RequestAborted);
//            response.EnsureSuccessStatusCode();

//            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

//            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload)
//            {
//                Properties = properties,
//                Principal = new ClaimsPrincipal(identity)
//            };

//            var identifier = MicrosoftAccountHelper.GetId(payload);
//            if (!string.IsNullOrEmpty(identifier))
//            {
//                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
//                identity.AddClaim(new Claim("urn:microsoftaccount:id", identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
//            }

//            var name = MicrosoftAccountHelper.GetName(payload);
//            if (!string.IsNullOrEmpty(name))
//            {
//                identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, Options.ClaimsIssuer));
//                identity.AddClaim(new Claim("urn:microsoftaccount:name", name, ClaimValueTypes.String, Options.ClaimsIssuer));
//            }

//            var email = MicrosoftAccountHelper.GetEmail(payload);
//            if (!string.IsNullOrEmpty(email))
//            {
//                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
//            }

//            await Options.Events.CreatingTicket(context);

//            //ISiteSettings site = siteResolver.Resolve();
//            var currentSite = await GetSite();

//            if (currentSite != null)
//            {
//                Claim siteGuidClaim = new Claim("SiteGuid", currentSite.SiteGuid.ToString());
//                if (!identity.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
//                {
//                    identity.AddClaim(siteGuidClaim);
//                }

//            }

//            //return new AuthenticationTicket(notification.Principal, notification.Properties, notification.Options.AuthenticationScheme);
//            return new AuthenticationTicket(context.Principal, context.Properties, AuthenticationScheme.External);
//        }

//    }
//}
