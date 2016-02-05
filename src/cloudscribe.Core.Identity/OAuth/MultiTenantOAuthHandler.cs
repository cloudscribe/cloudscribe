// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-27
// Last Modified:		    2016-02-05
// 



using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Http.Features.Authentication;
using Microsoft.AspNet.WebUtilities;
//using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;




namespace cloudscribe.Core.Identity.OAuth
{
    internal static class Constants
    {
        internal const string SecurityAuthenticate = "security.Authenticate";
        internal const string CorrelationPrefix = ".AspNet.Correlation.";
    }

    public class MultiTenantOAuthHandler<TOptions> : RemoteAuthenticationHandler<TOptions> where TOptions : OAuthOptions
    {
        private static readonly RandomNumberGenerator CryptoRandom = RandomNumberGenerator.Create();

        public MultiTenantOAuthHandler(
            HttpClient backchannel,
            ILoggerFactory loggerFactory,
            MultiTenantOAuthOptionsResolver tenantOptions)
        {
            Backchannel = backchannel;
            log = loggerFactory.CreateLogger<MultiTenantOAuthHandler<TOptions>>();
            this.tenantOptions = tenantOptions;
        }

        private ILogger log;
        private MultiTenantOAuthOptionsResolver tenantOptions;

        protected HttpClient Backchannel { get; private set; }

        /// <summary>
        /// Called once by common code after initialization. If an authentication middleware responds directly to
        /// specifically known paths it must override this virtual, compare the request path to it's known paths, 
        /// provide any response information as appropriate, and true to stop further processing.
        /// </summary>
        /// <returns>Returning false will cause the common code to call the next middleware in line. Returning true will
        /// cause the common code to begin the async completion journey without calling the rest of the middleware
        /// pipeline.</returns>
        public override async Task<bool> HandleRequestAsync()
        {
            // this is called on every request
            log.LogDebug("InvokeAsync called");

            //if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path) // original logic
            // we need to respond not just to /signin-facebook but also folder sites /foldername/signin-facebook
            // for example so changed to check contains instead of exact match
            if (Options.CallbackPath.HasValue &&  Request.Path.Value.Contains(Options.CallbackPath.Value))
            {
                //return await InvokeReturnPathAsync();
                return await HandleRemoteCallbackAsync();
            }
            return false;
        }

        // I think this was renamed as HandleRemoteCallbackAsync
        // since we were not modifying this code jus commented it out to
        // let the base class handle it, may need to override it though needs testing
        //public async Task<bool> InvokeReturnPathAsync()
        //{
        //    log.LogDebug("InvokeReturnPathAsync called");

        //    var ticket = await HandleAuthenticateOnceAsync();
        //    if (ticket == null)
        //    {
        //        Logger.LogWarning("Invalid return state, unable to redirect.");
        //        Response.StatusCode = 500;
        //        return true;
        //    }

        //    var context = new SigningInContext(Context, ticket)
        //    {
        //        SignInScheme = Options.SignInScheme,
        //        RedirectUri = ticket.Properties.RedirectUri,
        //    };
        //    ticket.Properties.RedirectUri = null;

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
            log.LogDebug("HandleAuthenticateAsync called");

            AuthenticationProperties properties = null;
            try
            {
                var query = Request.Query;

                // TODO: Is this a standard error returned by servers?
                var value = query["error"];
                if (!string.IsNullOrEmpty(value))
                {
                    Logger.LogVerbose("Remote server returned an error: " + Request.QueryString);
                    // TODO: Fail request rather than passing through?
                    return null;
                }

                var code = query["code"];
                var state = query["state"];

                properties = Options.StateDataFormat.Unprotect(state);
                if (properties == null)
                {
                    return null;
                }

                // OAuth2 10.12 CSRF
                bool valid = await ValidateCorrelationId(properties);
                if (!valid)
                {
                    //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                    return AuthenticateResult.Failed("Correlation failed.");
                }

                if (string.IsNullOrEmpty(code))
                {
                    // Null if the remote server returns an error.
                    //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                    return AuthenticateResult.Failed("Code was not found.");
                }

                var tokens = await ExchangeCodeAsync(code, BuildRedirectUri(Options.CallbackPath));

                // must be after rc1
                //if (tokens.Error != null)
                //{
                //    return AuthenticateResult.Failed(tokens.Error);
                //}


                if (string.IsNullOrEmpty(tokens.AccessToken))
                {
                    Logger.LogWarning("Access token was not found");
                    //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                    return AuthenticateResult.Failed("Failed to retrieve access token.");

                }

                var identity = new ClaimsIdentity(Options.ClaimsIssuer);

                if (Options.SaveTokensAsClaims)
                {
                    identity.AddClaim(new Claim("access_token", tokens.AccessToken,
                                                ClaimValueTypes.String, Options.ClaimsIssuer));

                    if (!string.IsNullOrEmpty(tokens.RefreshToken))
                    {
                        identity.AddClaim(new Claim("refresh_token", tokens.RefreshToken,
                                                    ClaimValueTypes.String, Options.ClaimsIssuer));
                    }

                    if (!string.IsNullOrEmpty(tokens.TokenType))
                    {
                        identity.AddClaim(new Claim("token_type", tokens.TokenType,
                                                    ClaimValueTypes.String, Options.ClaimsIssuer));
                    }

                    if (!string.IsNullOrEmpty(tokens.ExpiresIn))
                    {
                        identity.AddClaim(new Claim("expires_in", tokens.ExpiresIn,
                                                    ClaimValueTypes.String, Options.ClaimsIssuer));
                    }
                }

                //return await CreateTicketAsync(identity, properties, tokens);
                return AuthenticateResult.Success(await CreateTicketAsync(identity, properties, tokens));
            }
            catch (Exception ex)
            {
                Logger.LogError("Authentication failed", ex);
                //return new AuthenticationTicket(properties, Options.AuthenticationScheme);
                return AuthenticateResult.Failed("Authentication failed, exception logged.");
            }
        }

        protected virtual async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            log.LogDebug("ExchangeCodeAsync called with code " + code + " redirectUri " + redirectUri);

            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret },
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

        protected virtual async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            log.LogDebug("CreateTicketAsync called");

            var context = new OAuthCreatingTicketContext(Context, Options, Backchannel, tokens)
            {
                Principal = new ClaimsPrincipal(identity),
                Properties = properties
            };

            await Options.Events.CreatingTicket(context);

            if (context.Principal?.Identity == null)
            {
                return null;
            }

            return new AuthenticationTicket(context.Principal, context.Properties, Options.AuthenticationScheme);
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            log.LogDebug("HandleUnauthorizedAsync called");

            var properties = new AuthenticationProperties(context.Properties);

            if (string.IsNullOrEmpty(properties.RedirectUri))
            {
                properties.RedirectUri = CurrentUri;
            }

            // OAuth2 10.12 CSRF
            await GenerateCorrelationId(properties);

            var authorizationEndpoint = await BuildChallengeUrl(properties, BuildRedirectUri(Options.CallbackPath));

            var redirectContext = new OAuthRedirectToAuthorizationContext(
                Context, Options,
                properties, authorizationEndpoint);

            //await Options.Events.ApplyRedirect(redirectContext);
            await Options.Events.RedirectToAuthorizationEndpoint(redirectContext);
            return true;
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

        protected virtual Task<string> BuildChallengeUrl(
            AuthenticationProperties properties, 
            string redirectUri)
        {
            log.LogDebug("BuildChallengeUrl called with redirectUri = " + redirectUri);

            var scope = FormatScope();

            var state = Options.StateDataFormat.Protect(properties);

            var queryBuilder = new QueryBuilder()
            {
                { "client_id", Options.ClientId },
                { "scope", scope },
                { "response_type", "code" },
                { "redirect_uri", redirectUri },
                { "state", state },
            };
            string result =  Options.AuthorizationEndpoint + queryBuilder.ToString();

            return Task.FromResult(result);
        }

        protected virtual string FormatScope()
        {
            // OAuth2 3.3 space separated
            return string.Join(" ", Options.Scope);
        }

        protected async Task GenerateCorrelationId(AuthenticationProperties properties)
        {
            // I think here we need to use a different correlationkey per folder site
            // becuase that is used for the cookie name
            // unless using related sites mode we want fodler sites to use different cookies

            var correlationKey = Constants.CorrelationPrefix + Options.AuthenticationScheme;

            correlationKey = await tenantOptions.ResolveCorrelationKey(correlationKey);

            log.LogDebug("GenerateCorrelationId called, correlationKey was " + correlationKey);

            var nonceBytes = new byte[32];
            CryptoRandom.GetBytes(nonceBytes);
            var correlationId = Base64UrlTextEncoder.Encode(nonceBytes);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps
            };

            properties.Items[correlationKey] = correlationId;

            Response.Cookies.Append(correlationKey, correlationId, cookieOptions);
        }

        protected async Task<bool> ValidateCorrelationId(AuthenticationProperties properties)
        {
            
            var correlationKey = Constants.CorrelationPrefix + Options.AuthenticationScheme;
            correlationKey = await tenantOptions.ResolveCorrelationKey(correlationKey);

            log.LogDebug("ValidateCorrelationId called, correlationKey was " + correlationKey);

            var correlationCookie = Request.Cookies[correlationKey];
            if (string.IsNullOrEmpty(correlationCookie))
            {
                Logger.LogWarning("{0} cookie not found.", correlationKey);
                return false;
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps
            };
            Response.Cookies.Delete(correlationKey, cookieOptions);

            string correlationExtra;
            if (!properties.Items.TryGetValue(
                correlationKey,
                out correlationExtra))
            {
                Logger.LogWarning("{0} state property not found.", correlationKey);
                return false;
            }

            properties.Items.Remove(correlationKey);

            if (!string.Equals(correlationCookie, correlationExtra, StringComparison.Ordinal))
            {
                Logger.LogWarning("{0} correlation cookie and state property mismatch.", correlationKey);
                return false;
            }

            return true;
        }


    }
}
