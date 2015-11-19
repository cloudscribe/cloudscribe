// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-29
// Last Modified:		    2015-11-19
// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Twitter/TwitterHandler.cs

using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Features.Authentication;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
// this is not available in beta6/7 guess this will have to wait till beta 8 or later
//using Microsoft.Framework.Primitives;


namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantTwitterHandler : RemoteAuthenticationHandler<TwitterOptions>
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private const string StateCookie = "__TwitterState";
        private const string RequestTokenEndpoint = "https://api.twitter.com/oauth/request_token";
        private const string AuthenticationEndpoint = "https://twitter.com/oauth/authenticate?oauth_token=";
        private const string AccessTokenEndpoint = "https://api.twitter.com/oauth/access_token";

        private readonly HttpClient _httpClient;

        public MultiTenantTwitterHandler(
            HttpClient httpClient,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            MultiTenantOptions multiTenantOptions,
            ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;

            log = loggerFactory.CreateLogger<MultiTenantTwitterHandler>();
            this.siteResolver = siteResolver;
            this.multiTenantOptions = multiTenantOptions;
            siteRepo = siteRepository;
        }

        private ILogger log;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;

        public override async Task<bool> HandleRequestAsync()
        {
            //if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
            // we need to respond not just to /signin-twitter but also folder sites /foldername/signin-twitter
            // for example so changed to check contains instead of exact match
            if (Options.CallbackPath.HasValue && Request.Path.Value.Contains(Options.CallbackPath.Value))
            {
                //return await InvokeReturnPathAsync();
                return await HandleRemoteCallbackAsync();
            }
            return false;
        }

        //public async Task<bool> InvokeReturnPathAsync()
        //{
        //    var model = await HandleAuthenticateOnceAsync();
        //    if (model == null)
        //    {
        //        Logger.LogWarning("Invalid return state, unable to redirect.");
        //        Response.StatusCode = 500;
        //        return true;
        //    }

        //    var context = new SigningInContext(Context, model)
        //    {
        //        SignInScheme = Options.SignInScheme,
        //        RedirectUri = model.Properties.RedirectUri
        //    };
        //    model.Properties.RedirectUri = null;

        //    await Options.Events.SigningIn(context);

        //    if (context.SignInScheme != null && context.Principal != null)
        //    {
        //        await Context.Authentication.SignInAsync(context.SignInScheme, context.Principal, context.Properties);
        //    }

        //    if (!context.IsRequestCompleted && context.RedirectUri != null)
        //    {
        //        if (context.Principal == null)
        //        {
        //            // add a redirect hint that sign-in failed in some way
        //            context.RedirectUri = QueryHelpers.AddQueryString(context.RedirectUri, "error", "access_denied");
        //        }
        //        Response.Redirect(context.RedirectUri);
        //        context.RequestCompleted();
        //    }

        //    return context.IsRequestCompleted;
        //}

        protected override async Task<AuthenticateResult> HandleRemoteAuthenticateAsync()
        {
            AuthenticationProperties properties = null;
            try
            {
                var tenantOptions = new MultiTenantTwitterOptionsResolver(Options, siteResolver, siteRepo, multiTenantOptions);

                var query = Request.Query;
                var protectedRequestToken = Request.Cookies[tenantOptions.ResolveStateCookieName(StateCookie)];

                var requestToken = Options.StateDataFormat.Unprotect(protectedRequestToken);

                if (requestToken == null)
                {
                    Logger.LogWarning("Invalid state");
                    return AuthenticateResult.Failed("Invalid state cookie.");
                }

                properties = requestToken.Properties;

                var returnedToken = query["oauth_token"];
                //if (StringValues.IsNullOrEmpty(returnedToken))
                if (string.IsNullOrEmpty(returnedToken))
                {
                    Logger.LogWarning("Missing oauth_token");
                    //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                    return AuthenticateResult.Failed("Missing oauth_token");
                }

                if (!string.Equals(returnedToken, requestToken.Token, StringComparison.Ordinal))
                {
                    Logger.LogWarning("Unmatched token");
                    //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                    return AuthenticateResult.Failed("Unmatched token");
                }

                var oauthVerifier = query["oauth_verifier"];
                //if (StringValues.IsNullOrEmpty(oauthVerifier))
                if (string.IsNullOrEmpty(oauthVerifier))
                {
                    Logger.LogWarning("Missing or blank oauth_verifier");
                    //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                    return AuthenticateResult.Failed("Missing or blank oauth_verifier");
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps
                };

                Response.Cookies.Delete(tenantOptions.ResolveStateCookieName(StateCookie), cookieOptions);

                

                //var accessToken = await ObtainAccessTokenAsync(
                //    Options.ConsumerKey, 
                //    Options.ConsumerSecret, 
                //    requestToken, 
                //    oauthVerifier);

                var accessToken = await ObtainAccessTokenAsync(
                    tenantOptions.ConsumerKey,
                    tenantOptions.ConsumerSecret,
                    requestToken,
                    oauthVerifier);

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, accessToken.UserId, ClaimValueTypes.String, Options.ClaimsIssuer),
                    new Claim(ClaimTypes.Name, accessToken.ScreenName, ClaimValueTypes.String, Options.ClaimsIssuer),
                    new Claim("urn:twitter:userid", accessToken.UserId, ClaimValueTypes.String, Options.ClaimsIssuer),
                    new Claim("urn:twitter:screenname", accessToken.ScreenName, ClaimValueTypes.String, Options.ClaimsIssuer)
                },
                Options.ClaimsIssuer);

                if (Options.SaveTokensAsClaims)
                {
                    identity.AddClaim(new Claim("access_token", accessToken.Token, ClaimValueTypes.String, Options.ClaimsIssuer));
                }

                //return await CreateTicketAsync(identity, properties, accessToken);
                return AuthenticateResult.Success(await CreateTicketAsync(identity, properties, accessToken));
            }
            catch (Exception ex)
            {
                Logger.LogError("Authentication failed", ex);
                //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                return AuthenticateResult.Failed("Authentication failed, exception logged");
            }
        }

        protected virtual async Task<AuthenticationTicket> CreateTicketAsync(
            ClaimsIdentity identity, 
            AuthenticationProperties properties, 
            AccessToken token)
        {
            log.LogDebug("CreateTicketAsync called tokens.ScreenName was " + token.ScreenName);

            //var context = new TwitterCreatingTicketContext(Context, token.UserId, token.ScreenName, token.Token, token.TokenSecret)
            //{
            //    Principal = new ClaimsPrincipal(identity),
            //    Properties = properties
            //};
            var context = new TwitterCreatingTicketContext(Context, Options, token.UserId, token.ScreenName, token.Token, token.TokenSecret)
            {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.CreatingTicket(context);

            if (context.Principal?.Identity == null)
            {
                return null;
            }

            ISiteSettings site = siteResolver.Resolve();

            if (site != null)
            {
                Claim siteGuidClaim = new Claim("SiteGuid", site.SiteGuid.ToString());
                if (!identity.HasClaim(siteGuidClaim.Type, siteGuidClaim.Value))
                {
                    identity.AddClaim(siteGuidClaim);
                }

            }

            //return new AuthenticationTicket(notification.Principal, notification.Properties, Options.AuthenticationScheme);
            return new AuthenticationTicket(context.Principal, context.Properties, AuthenticationScheme.External);
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var properties = new AuthenticationProperties(context.Properties);
            if (string.IsNullOrEmpty(properties.RedirectUri))
            {
                properties.RedirectUri = CurrentUri;
            }

            var tenantOptions = new MultiTenantTwitterOptionsResolver(Options, siteResolver, siteRepo, multiTenantOptions);

            //var requestToken = await ObtainRequestTokenAsync(
            //    Options.ConsumerKey, 
            //    Options.ConsumerSecret, 
            //    BuildRedirectUri(Options.CallbackPath), 
            //    properties);

            var requestToken = await ObtainRequestTokenAsync(
                tenantOptions.ConsumerKey,
                tenantOptions.ConsumerSecret,
                BuildRedirectUri(tenantOptions.ResolveRedirectUrl(Options.CallbackPath)),
                properties);

            if (requestToken.CallbackConfirmed)
            {
                var twitterAuthenticationEndpoint = AuthenticationEndpoint + requestToken.Token;

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps
                };

                Response.Cookies.Append(
                    tenantOptions.ResolveStateCookieName(StateCookie), 
                    Options.StateDataFormat.Protect(requestToken), 
                    cookieOptions);

                var redirectContext = new TwitterRedirectToAuthorizationEndpointContext(
                    Context, Options,
                    properties, twitterAuthenticationEndpoint);

                await Options.Events.RedirectToAuthorizationEndpoint(redirectContext);

                return true;
            }
            else
            {
                Logger.LogError("requestToken CallbackConfirmed!=true");
            }
            return false; // REVIEW: Make sure this should not stop other handlers
        }

        

        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            throw new NotSupportedException();
        }

        protected override Task HandleSignInAsync(SignInContext context)
        {
            throw new NotSupportedException();
        }

        protected override Task<bool> HandleForbiddenAsync(ChallengeContext context)
        {
            throw new NotSupportedException();
        }

        private async Task<RequestToken> ObtainRequestTokenAsync(
            string consumerKey, 
            string consumerSecret, 
            string callBackUri, 
            AuthenticationProperties properties)
        {
            Logger.LogVerbose("ObtainRequestToken");

            log.LogDebug("ObtainRequestTokenAsync called with consumerKey " + consumerKey + " and callBackUri " + callBackUri);

            var nonce = Guid.NewGuid().ToString("N");

            var authorizationParts = new SortedDictionary<string, string>
            {
                { "oauth_callback", callBackUri },
                { "oauth_consumer_key", consumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", GenerateTimeStamp() },
                { "oauth_version", "1.0" }
            };

            var parameterBuilder = new StringBuilder();
            foreach (var authorizationKey in authorizationParts)
            {
                parameterBuilder.AppendFormat("{0}={1}&", UrlEncoder.UrlEncode(authorizationKey.Key), UrlEncoder.UrlEncode(authorizationKey.Value));
            }
            parameterBuilder.Length--;
            var parameterString = parameterBuilder.ToString();

            var canonicalizedRequestBuilder = new StringBuilder();
            canonicalizedRequestBuilder.Append(HttpMethod.Post.Method);
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.UrlEncode(RequestTokenEndpoint));
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.UrlEncode(parameterString));

            var signature = ComputeSignature(consumerSecret, null, canonicalizedRequestBuilder.ToString());
            authorizationParts.Add("oauth_signature", signature);

            var authorizationHeaderBuilder = new StringBuilder();
            authorizationHeaderBuilder.Append("OAuth ");
            foreach (var authorizationPart in authorizationParts)
            {
                authorizationHeaderBuilder.AppendFormat(
                    "{0}=\"{1}\", ", authorizationPart.Key, UrlEncoder.UrlEncode(authorizationPart.Value));
            }
            authorizationHeaderBuilder.Length = authorizationHeaderBuilder.Length - 2;

            var request = new HttpRequestMessage(HttpMethod.Post, RequestTokenEndpoint);
            request.Headers.Add("Authorization", authorizationHeaderBuilder.ToString());

            var response = await _httpClient.SendAsync(request, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            string responseText = await response.Content.ReadAsStringAsync();

            var responseParameters = new FormCollection(FormReader.ReadForm(responseText));
            if (string.Equals(responseParameters["oauth_callback_confirmed"], "true", StringComparison.Ordinal))
            {
                return new RequestToken { Token = Uri.UnescapeDataString(responseParameters["oauth_token"]), TokenSecret = Uri.UnescapeDataString(responseParameters["oauth_token_secret"]), CallbackConfirmed = true, Properties = properties };
            }

            return new RequestToken();
        }

        private async Task<AccessToken> ObtainAccessTokenAsync(string consumerKey, string consumerSecret, RequestToken token, string verifier)
        {
            // https://dev.twitter.com/docs/api/1/post/oauth/access_token

            Logger.LogVerbose("ObtainAccessToken");

            log.LogDebug("ObtainRequestTokenAsync called with consumerKey " + consumerKey + " and token " + token.Token + " and verifier " + verifier);

            var nonce = Guid.NewGuid().ToString("N");

            var authorizationParts = new SortedDictionary<string, string>
            {
                { "oauth_consumer_key", consumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_token", token.Token },
                { "oauth_timestamp", GenerateTimeStamp() },
                { "oauth_verifier", verifier },
                { "oauth_version", "1.0" },
            };

            var parameterBuilder = new StringBuilder();
            foreach (var authorizationKey in authorizationParts)
            {
                parameterBuilder.AppendFormat("{0}={1}&", UrlEncoder.UrlEncode(authorizationKey.Key), UrlEncoder.UrlEncode(authorizationKey.Value));
            }
            parameterBuilder.Length--;
            var parameterString = parameterBuilder.ToString();

            var canonicalizedRequestBuilder = new StringBuilder();
            canonicalizedRequestBuilder.Append(HttpMethod.Post.Method);
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.UrlEncode(AccessTokenEndpoint));
            canonicalizedRequestBuilder.Append("&");
            canonicalizedRequestBuilder.Append(UrlEncoder.UrlEncode(parameterString));

            var signature = ComputeSignature(consumerSecret, token.TokenSecret, canonicalizedRequestBuilder.ToString());
            authorizationParts.Add("oauth_signature", signature);
            authorizationParts.Remove("oauth_verifier");

            var authorizationHeaderBuilder = new StringBuilder();
            authorizationHeaderBuilder.Append("OAuth ");
            foreach (var authorizationPart in authorizationParts)
            {
                authorizationHeaderBuilder.AppendFormat(
                    "{0}=\"{1}\", ", authorizationPart.Key, UrlEncoder.UrlEncode(authorizationPart.Value));
            }
            authorizationHeaderBuilder.Length = authorizationHeaderBuilder.Length - 2;

            var request = new HttpRequestMessage(HttpMethod.Post, AccessTokenEndpoint);
            request.Headers.Add("Authorization", authorizationHeaderBuilder.ToString());

            var formPairs = new Dictionary<string, string>()
            {
                { "oauth_verifier", verifier },
            };

            request.Content = new FormUrlEncodedContent(formPairs);

            var response = await _httpClient.SendAsync(request, Context.RequestAborted);

            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("AccessToken request failed with a status code of " + response.StatusCode);
                response.EnsureSuccessStatusCode(); // throw
            }

            var responseText = await response.Content.ReadAsStringAsync();
            var responseParameters = new FormCollection(FormReader.ReadForm(responseText));

            return new AccessToken
            {
                Token = Uri.UnescapeDataString(responseParameters["oauth_token"]),
                TokenSecret = Uri.UnescapeDataString(responseParameters["oauth_token_secret"]),
                UserId = Uri.UnescapeDataString(responseParameters["user_id"]),
                ScreenName = Uri.UnescapeDataString(responseParameters["screen_name"])
            };
        }

        private static string GenerateTimeStamp()
        {
            var secondsSinceUnixEpocStart = DateTime.UtcNow - Epoch;
            return Convert.ToInt64(secondsSinceUnixEpocStart.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }

        private string ComputeSignature(string consumerSecret, string tokenSecret, string signatureData)
        {
            //log.LogDebug("ComputeSignature called with consumerSecret " + consumerSecret);

            using (var algorithm = new HMACSHA1())
            {
                algorithm.Key = Encoding.ASCII.GetBytes(
                    string.Format(CultureInfo.InvariantCulture,
                        "{0}&{1}",
                        UrlEncoder.UrlEncode(consumerSecret),
                        string.IsNullOrEmpty(tokenSecret) ? string.Empty : UrlEncoder.UrlEncode(tokenSecret)));
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(signatureData));
                return Convert.ToBase64String(hash);
            }
        }

    }
}
