//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:				    2014-08-25
//// Last Modified:		    2016-02-05
//// 

//using SaasKit.Multitenancy;
//using System;
//using System.Globalization;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using cloudscribe.Core.Models;
//using Microsoft.AspNet.Authentication;
//using Microsoft.AspNet.Authentication.OAuth;
//using Microsoft.AspNet.Authentication.Facebook;
//using Microsoft.AspNet.Http;
//using Microsoft.AspNet.Http.Authentication;
//using Microsoft.AspNet.Http.Extensions;
//using Microsoft.AspNet.Http.Internal;
//using Microsoft.AspNet.WebUtilities;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json.Linq;

//namespace cloudscribe.Core.Identity.OAuth
//{
//    /// <summary>
//    /// based on  https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Facebook/FacebookHandler.cs
//    /// https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.OAuth/OAuthHandler.cs
//    /// https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationHandler.cs
//    /// </summary>
//    internal class MultiTenantFacebookHandler : MultiTenantOAuthHandler<FacebookOptions>
//    {
//        public MultiTenantFacebookHandler(
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
//            log = loggerFactory.CreateLogger<MultiTenantFacebookHandler>();
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
//            //var queryBuilder = new QueryBuilder()
//            //{
//            //    { "grant_type", "authorization_code" },
//            //    { "code", code },
//            //    { "redirect_uri", redirectUri },
//            //    { "client_id", Options.AppId },
//            //    { "client_secret", Options.AppSecret },
//            //};

//            var currentSite = await GetSite();

//            var tenantFbOptions 
//                = new MultiTenantFacebookOptionsResolver(
//                    Options,
//                    currentSite, 
//                    multiTenantOptions);

            
//            var queryBuilder = new QueryBuilder()
//            {
//                { "grant_type", "authorization_code" },
//                { "code", code },
//                { "redirect_uri", tenantFbOptions.ResolveRedirectUrl(redirectUri) },
//                { "client_id", tenantFbOptions.AppId },
//                { "client_secret", tenantFbOptions.AppSecret },
//            };

//            var response = await Backchannel.GetAsync(Options.TokenEndpoint + queryBuilder.ToString(), Context.RequestAborted);
//            response.EnsureSuccessStatusCode();

//            var form = new FormCollection(FormReader.ReadForm(await response.Content.ReadAsStringAsync()));
//            var payload = new JObject();
//            foreach (string key in form.Keys)
//            {
//                //payload.Add(string.Equals(key, "expires", StringComparison.OrdinalIgnoreCase) ? "expires_in" : key, form[key]);
//                payload.Add(string.Equals(key, "expires", StringComparison.OrdinalIgnoreCase) ? "expires_in" : key, (string)form[key]);
//            }

//            // The refresh token is not available.
//            return new OAuthTokenResponse(payload);
//        }

//        protected override async Task<string> BuildChallengeUrl(
//            AuthenticationProperties properties, 
//            string redirectUri)
//        {
//            log.LogDebug("BuildChallengeUrl called with redirectUri = " + redirectUri);

//            var scope = FormatOAuthScope();

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

//            var tenantFbOptions = new MultiTenantFacebookOptionsResolver(
//                Options, 
//                currentSite, 
//                multiTenantOptions);

            
//            var queryBuilder = new QueryBuilder()
//            {
//                { "client_id", tenantFbOptions.AppId },
//                { "scope", scope },
//                { "response_type", "code" },
//                { "redirect_uri", tenantFbOptions.ResolveRedirectUrl(redirectUri) }, // we are hijacking this property here
//                { "state", state },
//            };


//            return Options.AuthorizationEndpoint + queryBuilder.ToString();
//        }




//        protected override async Task<AuthenticationTicket> CreateTicketAsync(
//            ClaimsIdentity identity, 
//            AuthenticationProperties properties, 
//            OAuthTokenResponse tokens)
//        {
//            log.LogDebug("CreateTicketAsync called");
//            //Options.AuthenticationScheme = AuthenticationScheme.External;

//            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
//            if (Options.SendAppSecretProof)
//            {
//                var proof = await GenerateAppSecretProof(tokens.AccessToken);
//                endpoint = QueryHelpers.AddQueryString(
//                    endpoint, 
//                    "appsecret_proof", 
//                    proof);
//            }

//            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
//            response.EnsureSuccessStatusCode();

//            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

//            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens, payload)
//            {
//                Properties = properties,
//                Principal = new ClaimsPrincipal(identity)
//            };

//            var identifier = FacebookHelper.GetId(payload);
//            if (!string.IsNullOrEmpty(identifier))
//            {
//                log.LogDebug("CreateTicketAsync FacebookAuthenticationHelper.GetId(payload) " + identifier);

//                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
//            }

//            var userName = FacebookHelper.GetUserName(payload);
//            if (!string.IsNullOrEmpty(userName))
//            {
//                log.LogDebug("CreateTicketAsync FacebookAuthenticationHelper.GetUserName(payload) " + userName);

//                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userName, ClaimValueTypes.String, Options.ClaimsIssuer));
//            }

//            var email = FacebookHelper.GetEmail(payload);
//            if (!string.IsNullOrEmpty(email))
//            {
//                log.LogDebug("CreateTicketAsync FacebookAuthenticationHelper.GetEmail(payload) " + email);

//                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
//            }

//            var name = FacebookHelper.GetName(payload);
//            if (!string.IsNullOrEmpty(name))
//            {
//                log.LogDebug("CreateTicketAsync FacebookAuthenticationHelper.GetName(payload) " + name);

//                identity.AddClaim(new Claim("urn:facebook:name", name, ClaimValueTypes.String, Options.ClaimsIssuer));

//                // Many Facebook accounts do not set the UserName field.  Fall back to the Name field instead.
//                if (string.IsNullOrEmpty(userName))
//                {
//                    identity.AddClaim(new Claim(identity.NameClaimType, name, ClaimValueTypes.String, Options.ClaimsIssuer));
//                }
//            }

//            var link = FacebookHelper.GetLink(payload);
//            if (!string.IsNullOrEmpty(link))
//            {
//                log.LogDebug("CreateTicketAsync FacebookAuthenticationHelper.GetLink(payload) " + link);

//                identity.AddClaim(new Claim("urn:facebook:link", link, ClaimValueTypes.String, Options.ClaimsIssuer));
//            }

//            log.LogDebug("CreateTicketAsync notification.Options.AuthenticationScheme " + context.Options.AuthenticationScheme);

//            await Options.Events.CreatingTicket(context);

//            var currentSite = await GetSite();
//            //ISiteSettings site = siteResolver.Resolve();

//            if (currentSite != null)
//            {
//                Claim siteGuidClaim = new Claim("SiteGuid", currentSite.SiteGuid.ToString());
//                if (!identity.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
//                {
//                    identity.AddClaim(siteGuidClaim);
//                }

//            }


//            log.LogDebug("CreateTicketAsync notification.Principal " + context.Principal.Identity.Name.ToString());

//            //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationTicket.cs
//            //return new AuthenticationTicket(notification.Principal, notification.Properties, notification.Options.AuthenticationScheme);
//            return new AuthenticationTicket(context.Principal, context.Properties, AuthenticationScheme.External);
//        }

//        private async Task<string> GenerateAppSecretProof(string accessToken)
//        {
//            var currentSite = await GetSite();

//            var tenantFbOptions = new MultiTenantFacebookOptionsResolver(
//                Options, 
//                currentSite, 
//                multiTenantOptions);

//            //using (var algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(Options.AppSecret)))
//            using (var algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(tenantFbOptions.AppSecret)))
//            {
//                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
//                var builder = new StringBuilder();
//                for (int i = 0; i < hash.Length; i++)
//                {
//                    builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
//                }
//                return builder.ToString();
//            }
//        }

//        protected virtual string FormatOAuthScope()
//        {
//            // OAuth2 3.3 space separated
//            return string.Join(" ", Options.Scope);
//        }

//        protected override string FormatScope()
//        {
//            // Facebook deviates from the OAuth spec here. They require comma separated instead of space separated.
//            // https://developers.facebook.com/docs/reference/dialogs/oauth
//            // http://tools.ietf.org/html/rfc6749#section-3.3
//            return string.Join(",", Options.Scope);
//        }

        


//    }
//}
