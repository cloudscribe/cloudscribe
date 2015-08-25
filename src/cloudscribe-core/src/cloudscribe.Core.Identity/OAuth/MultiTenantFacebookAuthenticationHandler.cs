// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-25
// Last Modified:		    2015-08-25
// 

using System;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Http.Extensions;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.WebUtilities;
using Newtonsoft.Json.Linq;

namespace cloudscribe.Core.Identity.OAuth
{
    /// <summary>
    /// based on  https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Facebook/FacebookAuthenticationHandler.cs
    /// </summary>
    internal class MultiTenantFacebookAuthenticationHandler : OAuthAuthenticationHandler<FacebookAuthenticationOptions>
    {
        public MultiTenantFacebookAuthenticationHandler(HttpClient httpClient)
            : base(httpClient)
        {
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var queryBuilder = new QueryBuilder()
            {
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUri },
                { "client_id", Options.AppId },
                { "client_secret", Options.AppSecret },
            };

            var response = await Backchannel.GetAsync(Options.TokenEndpoint + queryBuilder.ToString(), Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var form = new FormCollection(FormReader.ReadForm(await response.Content.ReadAsStringAsync()));
            var payload = new JObject();
            foreach (string key in form.Keys)
            {
                payload.Add(string.Equals(key, "expires", StringComparison.OrdinalIgnoreCase) ? "expires_in" : key, form[key]);
            }

            // The refresh token is not available.
            return new OAuthTokenResponse(payload);
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var endpoint = QueryHelpers.AddQueryString(Options.UserInformationEndpoint, "access_token", tokens.AccessToken);
            if (Options.SendAppSecretProof)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "appsecret_proof", GenerateAppSecretProof(tokens.AccessToken));
            }

            var response = await Backchannel.GetAsync(endpoint, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());

            var notification = new OAuthAuthenticatedContext(Context, Options, Backchannel, tokens, payload)
            {
                Properties = properties,
                Principal = new ClaimsPrincipal(identity)
            };

            var identifier = FacebookAuthenticationHelper.GetId(payload);
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var userName = FacebookAuthenticationHelper.GetUserName(payload);
            if (!string.IsNullOrEmpty(userName))
            {
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userName, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var email = FacebookAuthenticationHelper.GetEmail(payload);
            if (!string.IsNullOrEmpty(email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var name = FacebookAuthenticationHelper.GetName(payload);
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim("urn:facebook:name", name, ClaimValueTypes.String, Options.ClaimsIssuer));

                // Many Facebook accounts do not set the UserName field.  Fall back to the Name field instead.
                if (string.IsNullOrEmpty(userName))
                {
                    identity.AddClaim(new Claim(identity.NameClaimType, name, ClaimValueTypes.String, Options.ClaimsIssuer));
                }
            }

            var link = FacebookAuthenticationHelper.GetLink(payload);
            if (!string.IsNullOrEmpty(link))
            {
                identity.AddClaim(new Claim("urn:facebook:link", link, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            await Options.Notifications.Authenticated(notification);

            return new AuthenticationTicket(notification.Principal, notification.Properties, notification.Options.AuthenticationScheme);
        }

        private string GenerateAppSecretProof(string accessToken)
        {
            using (var algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(Options.AppSecret)))
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
                var builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return builder.ToString();
            }
        }

        protected override string FormatScope()
        {
            // Facebook deviates from the OAuth spec here. They require comma separated instead of space separated.
            // https://developers.facebook.com/docs/reference/dialogs/oauth
            // http://tools.ietf.org/html/rfc6749#section-3.3
            return string.Join(",", Options.Scope);
        }
    }
}
