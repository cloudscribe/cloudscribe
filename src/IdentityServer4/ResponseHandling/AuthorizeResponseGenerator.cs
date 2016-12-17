// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.ResponseHandling
{
    class AuthorizeResponseGenerator : IAuthorizeResponseGenerator
    {
        private readonly ILogger<AuthorizeResponseGenerator> _logger;
        private readonly ITokenService _tokenService;
        private readonly IAuthorizationCodeStore _authorizationCodeStore;
        private readonly IEventService _events;

        public AuthorizeResponseGenerator(ILogger<AuthorizeResponseGenerator> logger, ITokenService tokenService, IAuthorizationCodeStore authorizationCodeStore, IEventService events)
        {
            _logger = logger;
            _tokenService = tokenService;
            _authorizationCodeStore = authorizationCodeStore;
            _events = events;
        }

        public async Task<AuthorizeResponse> CreateResponseAsync(ValidatedAuthorizeRequest request)
        {
            if (request.GrantType == GrantType.AuthorizationCode)
            {
                return await CreateCodeFlowResponseAsync(request);
            }
            if (request.GrantType == GrantType.Implicit)
            {
                return await CreateImplicitFlowResponseAsync(request);
            }
            if (request.GrantType == GrantType.Hybrid)
            {
                return await CreateHybridFlowResponseAsync(request);
            }

            _logger.LogError("Unsupported grant type: " + request.GrantType);
            throw new InvalidOperationException("invalid grant type: " + request.GrantType);
        }

        private async Task<AuthorizeResponse> CreateHybridFlowResponseAsync(ValidatedAuthorizeRequest request)
        {
            _logger.LogDebug("Creating Hybrid Flow response.");

            var code = await CreateCodeAsync(request);
            var response = await CreateImplicitFlowResponseAsync(request, code);
            response.Code = code;

            return response;
        }

        public async Task<AuthorizeResponse> CreateCodeFlowResponseAsync(ValidatedAuthorizeRequest request)
        {
            _logger.LogDebug("Creating Authorization Code Flow response.");

            var code = await CreateCodeAsync(request);

            var response = new AuthorizeResponse
            {
                Request = request,
                Code = code,
                SessionState = request.GenerateSessionStateValue()
            };

            return response;
        }

        private async Task<string> CreateCodeAsync(ValidatedAuthorizeRequest request)
        {
            var code = new AuthorizationCode
            {
                ClientId = request.Client.ClientId,
                Lifetime = request.Client.AuthorizationCodeLifetime,
                Subject = request.Subject,
                SessionId = request.SessionId,
                CodeChallenge = request.CodeChallenge.Sha256(),
                CodeChallengeMethod = request.CodeChallengeMethod,

                IsOpenId = request.IsOpenIdRequest,
                RequestedScopes = request.ValidatedScopes.GrantedResources.ToScopeNames(),
                RedirectUri = request.RedirectUri,
                Nonce = request.Nonce,

                WasConsentShown = request.WasConsentShown,
            };

            // store id token and access token and return authorization code
            var id = CryptoRandom.CreateUniqueId();
            await _authorizationCodeStore.StoreAuthorizationCodeAsync(id, code);

            await RaiseCodeIssuedEventAsync(id, code);

            return id;
        }

        public async Task<AuthorizeResponse> CreateImplicitFlowResponseAsync(ValidatedAuthorizeRequest request, string authorizationCode = null)
        {
            _logger.LogDebug("Creating Implicit Flow response.");

            string accessTokenValue = null;
            int accessTokenLifetime = 0;

            var responseTypes = request.ResponseType.FromSpaceSeparatedString();

            if (responseTypes.Contains(OidcConstants.ResponseTypes.Token))
            {
                var tokenRequest = new TokenCreationRequest
                {
                    Subject = request.Subject,
                    Client = request.Client,
                    Resources = request.ValidatedScopes.GrantedResources,

                    ValidatedRequest = request
                };

                var accessToken = await _tokenService.CreateAccessTokenAsync(tokenRequest);
                accessTokenLifetime = accessToken.Lifetime;

                accessTokenValue = await _tokenService.CreateSecurityTokenAsync(accessToken);
            }

            string jwt = null;
            if (responseTypes.Contains(OidcConstants.ResponseTypes.IdToken))
            {
                var tokenRequest = new TokenCreationRequest
                {
                    ValidatedRequest = request,
                    Subject = request.Subject,
                    Client = request.Client,
                    Resources = request.ValidatedScopes.GrantedResources,

                    Nonce = request.Raw.Get(OidcConstants.AuthorizeRequest.Nonce),
                    // if no access token is requested, then we need to include all the claims in the id token
                    IncludeAllIdentityClaims = !request.AccessTokenRequested,
                    AccessTokenToHash = accessTokenValue,
                    AuthorizationCodeToHash = authorizationCode
                };

                var idToken = await _tokenService.CreateIdentityTokenAsync(tokenRequest);
                jwt = await _tokenService.CreateSecurityTokenAsync(idToken);
            }

            var response = new AuthorizeResponse
            {
                Request = request,
                AccessToken = accessTokenValue,
                AccessTokenLifetime = accessTokenLifetime,
                IdentityToken = jwt,
                SessionState = request.GenerateSessionStateValue()
            };

            return response;
        }

        private async Task RaiseCodeIssuedEventAsync(string id, AuthorizationCode code)
        {
            await _events.RaiseAuthorizationCodeIssuedEventAsync(id, code);
        }
    }
}