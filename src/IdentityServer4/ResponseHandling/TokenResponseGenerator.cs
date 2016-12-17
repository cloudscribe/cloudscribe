// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
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
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IResourceStore _resources;
        private readonly IClientStore _clients;
       
        public TokenResponseGenerator(ITokenService tokenService, IRefreshTokenService refreshTokenService, IResourceStore resources, IClientStore clients, ILoggerFactory loggerFactory)
        {
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _resources = resources;
            _clients = clients;
            _logger = loggerFactory.CreateLogger<TokenResponseGenerator>();
        }

        public async Task<TokenResponse> ProcessAsync(TokenRequestValidationResult validationResult)
        {
            _logger.LogTrace("Creating token response");

            if (validationResult.ValidatedRequest.GrantType == OidcConstants.GrantTypes.AuthorizationCode)
            {
                return await ProcessAuthorizationCodeRequestAsync(validationResult.ValidatedRequest);
            }

            if (validationResult.ValidatedRequest.GrantType == OidcConstants.GrantTypes.RefreshToken)
            {
                return await ProcessRefreshTokenRequestAsync(validationResult.ValidatedRequest);
            }

            return await ProcessTokenRequestAsync(validationResult);
        }

        private async Task<TokenResponse> ProcessAuthorizationCodeRequestAsync(ValidatedTokenRequest request)
        {
            _logger.LogTrace("Processing authorization code request");

            //////////////////////////
            // access token
            /////////////////////////
            var accessToken = await CreateAccessTokenAsync(request);
            var response = new TokenResponse
            {
                AccessToken = accessToken.Item1,
                AccessTokenLifetime = request.Client.AccessTokenLifetime
            };

            //////////////////////////
            // refresh token
            /////////////////////////
            if (accessToken.Item2.IsPresent())
            {
                response.RefreshToken = accessToken.Item2;
            }

            //////////////////////////
            // id token
            /////////////////////////
            if (request.AuthorizationCode.IsOpenId)
            {
                // load the client that belongs to the authorization code
                Client client = null;
                if (request.AuthorizationCode.ClientId != null)
                {
                    client = await _clients.FindEnabledClientByIdAsync(request.AuthorizationCode.ClientId);
                }
                if (client == null)
                {
                    throw new InvalidOperationException("Client does not exist anymore.");
                }

                var resources = await _resources.FindEnabledResourcesByScopeAsync(request.AuthorizationCode.RequestedScopes);

                var tokenRequest = new TokenCreationRequest
                {
                    Subject = request.AuthorizationCode.Subject,
                    Client = client,
                    Resources = resources,
                    Nonce = request.AuthorizationCode.Nonce,

                    ValidatedRequest = request
                };

                var idToken = await _tokenService.CreateIdentityTokenAsync(tokenRequest);
                var jwt = await _tokenService.CreateSecurityTokenAsync(idToken);
                response.IdentityToken = jwt;
            }

            return response;
        }

        private async Task<TokenResponse> ProcessTokenRequestAsync(TokenRequestValidationResult validationResult)
        {
            _logger.LogTrace("Processing token request");

            var accessToken = await CreateAccessTokenAsync(validationResult.ValidatedRequest);
            var response = new TokenResponse
            {
                AccessToken = accessToken.Item1,
                AccessTokenLifetime = validationResult.ValidatedRequest.Client.AccessTokenLifetime,
                Custom = validationResult.CustomResponse
            };

            if (accessToken.Item2.IsPresent())
            {
                response.RefreshToken = accessToken.Item2;
            }

            return response;
        }

        private async Task<TokenResponse> ProcessRefreshTokenRequestAsync(ValidatedTokenRequest request)
        {
            _logger.LogTrace("Processing refresh token request");

            var oldAccessToken = request.RefreshToken.AccessToken;
            string accessTokenString;
            
            if (request.Client.UpdateAccessTokenClaimsOnRefresh)
            {
                // TODO: we don't seem to update the request.RefreshToken.AccessToken with the newly created one. do we need to?
                // what change in behavior would we introduce by doing that? the claims in the user would change over time for the 
                // call into IsActive.

                var subject = request.RefreshToken.Subject;

                var creationRequest = new TokenCreationRequest
                {
                    Client = request.Client,
                    Subject = subject,
                    ValidatedRequest = request,
                    Resources = await _resources.FindEnabledResourcesByScopeAsync(oldAccessToken.Scopes),
                };

                var newAccessToken = await _tokenService.CreateAccessTokenAsync(creationRequest);
                accessTokenString = await _tokenService.CreateSecurityTokenAsync(newAccessToken);
            }
            else
            {
                oldAccessToken.CreationTime = DateTimeHelper.UtcNow;
                oldAccessToken.Lifetime = request.Client.AccessTokenLifetime;

                accessTokenString = await _tokenService.CreateSecurityTokenAsync(oldAccessToken);
            }

            var handle = await _refreshTokenService.UpdateRefreshTokenAsync(request.RefreshTokenHandle, request.RefreshToken, request.Client);

            return new TokenResponse
            {
                AccessToken = accessTokenString,
                AccessTokenLifetime = request.Client.AccessTokenLifetime,
                RefreshToken = handle
            };
        }

        private async Task<Tuple<string, string>> CreateAccessTokenAsync(ValidatedTokenRequest request)
        {
            TokenCreationRequest tokenRequest;
            bool createRefreshToken;

            if (request.AuthorizationCode != null)
            {
                createRefreshToken = request.AuthorizationCode.RequestedScopes.Contains(IdentityServerConstants.StandardScopes.OfflineAccess);

                // load the client that belongs to the authorization code
                Client client = null;
                if (request.AuthorizationCode.ClientId != null)
                {
                    client = await _clients.FindEnabledClientByIdAsync(request.AuthorizationCode.ClientId);
                }
                if (client == null)
                {
                    throw new InvalidOperationException("Client does not exist anymore.");
                }

                var resources = await _resources.FindEnabledResourcesByScopeAsync(request.AuthorizationCode.RequestedScopes);

                tokenRequest = new TokenCreationRequest
                {
                    Subject = request.AuthorizationCode.Subject,
                    Client = client,
                    Resources = resources,
                    ValidatedRequest = request
                };
            }
            else
            {
                createRefreshToken = request.ValidatedScopes.ContainsOfflineAccessScope;

                tokenRequest = new TokenCreationRequest
                {
                    Subject = request.Subject,
                    Client = request.Client,
                    Resources = request.ValidatedScopes.GrantedResources,
                    ValidatedRequest = request
                };
            }

            Token accessToken = await _tokenService.CreateAccessTokenAsync(tokenRequest);

            string refreshToken = "";
            if (createRefreshToken)
            {
                refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(tokenRequest.Subject, accessToken, request.Client);
            }

            var securityToken = await _tokenService.CreateSecurityTokenAsync(accessToken);
            return Tuple.Create(securityToken, refreshToken);
        }
    }
}