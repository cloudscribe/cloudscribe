// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Configuration;
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
    /// <summary>
    /// Default logic for determining if user must login or consent when making requests to the authorization endpoint.
    /// </summary>
    /// <seealso cref="IdentityServer4.ResponseHandling.IAuthorizeInteractionResponseGenerator" />
    public class AuthorizeInteractionResponseGenerator : IAuthorizeInteractionResponseGenerator
    {
        private readonly ILogger<AuthorizeInteractionResponseGenerator> _logger;
        private readonly IConsentService _consent;
        private readonly IProfileService _profile;

        public AuthorizeInteractionResponseGenerator(
            ILogger<AuthorizeInteractionResponseGenerator> logger, 
            IdentityServerOptions options, 
            IConsentService consent, 
            IProfileService profile)
        {
            _logger = logger;
            _consent = consent;
            _profile = profile;
        }

        /// <summary>
        /// Processes the interaction logic.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public virtual async Task<InteractionResponse> ProcessInteractionAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null)
        {
            _logger.LogTrace("ProcessInteractionAsync");

            var result = await ProcessLoginAsync(request);
            if (result.IsLogin || result.IsError)
            {
                return result;
            }

            return await ProcessConsentAsync(request, consent);
        }

        /// <summary>
        /// Processes the login logic.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected internal virtual async Task<InteractionResponse> ProcessLoginAsync(ValidatedAuthorizeRequest request)
        {
            if (request.PromptMode == OidcConstants.PromptModes.Login ||
                request.PromptMode == OidcConstants.PromptModes.SelectAccount)
            {
                // remove prompt so when we redirect back in from login page
                // we won't think we need to force a prompt again
                request.RemovePrompt();

                _logger.LogInformation("Showing login: request contains prompt={0}", request.PromptMode);

                return new InteractionResponse() { IsLogin = true };
            }

            // unauthenticated user
            var isAuthenticated = request.Subject.IsAuthenticated();
            
            // user de-activated
            bool isActive = false;

            if (isAuthenticated)
            {
                var isActiveCtx = new IsActiveContext(request.Subject, request.Client, IdentityServerConstants.ProfileIsActiveCallers.AuthorizeEndpoint);
                await _profile.IsActiveAsync(isActiveCtx);
                
                isActive = isActiveCtx.IsActive;
            }

            if (!isAuthenticated || !isActive)
            {
                // prompt=none means user must be signed in already
                if (request.PromptMode == OidcConstants.PromptModes.None)
                {
                    if (!isAuthenticated)
                    {
                        _logger.LogInformation("Showing error: prompt=none was requested but user is not authenticated");
                    }
                    else if (!isActive)
                    {
                        _logger.LogInformation("Showing error: prompt=none was requested but user is not active");
                    }

                    return new InteractionResponse
                    {
                        Error = OidcConstants.AuthorizeErrors.LoginRequired,
                    };
                }

                if (!isAuthenticated)
                {
                    _logger.LogInformation("Showing login: User is not authenticated");
                }
                else if (!isActive)
                {
                    _logger.LogInformation("Showing login: User is not active");
                }

                return new InteractionResponse() { IsLogin = true };
            }

            // check current idp
            var currentIdp = request.Subject.GetIdentityProvider();

            // check if idp login hint matches current provider
            var idp = request.GetIdP();
            if (idp.IsPresent())
            {
                if (idp != currentIdp)
                {
                    _logger.LogInformation("Showing login: Current IdP ({idp}) is not the requested IdP ({idp})", currentIdp, idp);
                    return new InteractionResponse() { IsLogin = true };
                }
            }

            // check authentication freshness
            if (request.MaxAge.HasValue)
            {
                var authTime = request.Subject.GetAuthenticationTime();
                if (DateTimeHelper.UtcNow > authTime.AddSeconds(request.MaxAge.Value))
                {
                    _logger.LogInformation("Showing login: Requested MaxAge exceeded.");

                    return new InteractionResponse() { IsLogin = true };
                }
            }

            // check local idp restrictions
            if (currentIdp == IdentityServerConstants.LocalIdentityProvider && !request.Client.EnableLocalLogin)
            {
                _logger.LogInformation("Showing login: User logged in locally, but client does not allow local logins");
                return new InteractionResponse() { IsLogin = true };
            }

            // check external idp restrictions
            if (request.Client.IdentityProviderRestrictions != null && request.Client.IdentityProviderRestrictions.Any())
            {
                if (!request.Client.IdentityProviderRestrictions.Contains(currentIdp))
                {
                    _logger.LogInformation("Showing login: User is logged in with idp: {idp}, but idp not in client restriction list.", currentIdp);
                    return new InteractionResponse() { IsLogin = true };
                }
            }

            return new InteractionResponse();
        }

        /// <summary>
        /// Processes the consent logic.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">Invalid PromptMode</exception>
        protected internal virtual async Task<InteractionResponse> ProcessConsentAsync(ValidatedAuthorizeRequest request, ConsentResponse consent = null)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.PromptMode != null &&
                request.PromptMode != OidcConstants.PromptModes.None &&
                request.PromptMode != OidcConstants.PromptModes.Consent)
            {
                _logger.LogError("Invalid prompt mode: {promptMode}", request.PromptMode);
                throw new ArgumentException("Invalid PromptMode");
            }

            var consentRequired = await _consent.RequiresConsentAsync(request.Subject, request.Client, request.RequestedScopes);

            if (consentRequired && request.PromptMode == OidcConstants.PromptModes.None)
            {
                _logger.LogInformation("Error: prompt=none requested, but consent is required.");

                return new InteractionResponse
                {
                    Error = OidcConstants.AuthorizeErrors.ConsentRequired,
                };
            }

            if (request.PromptMode == OidcConstants.PromptModes.Consent || consentRequired)
            {
                var response = new InteractionResponse();

                // did user provide consent
                if (consent == null)
                {
                    // user was not yet shown conset screen
                    response.IsConsent = true;
                    _logger.LogInformation("Showing consent: User has not yet consented");
                }
                else
                {
                    request.WasConsentShown = true;
                    _logger.LogTrace("Consent was shown to user");

                    // user was shown consent -- did they say yes or no
                    if (consent.Granted == false)
                    {
                        // no need to show consent screen again
                        // build access denied error to return to client
                        response.Error = OidcConstants.AuthorizeErrors.AccessDenied;
                        _logger.LogInformation("Error: User denied consent");
                    }
                    else
                    {
                        // double check that required scopes are in the list of consented scopes
                        var valid = request.ValidatedScopes.ValidateRequiredScopes(consent.ScopesConsented);
                        if (valid == false)
                        {
                            response.Error = OidcConstants.AuthorizeErrors.AccessDenied;
                            _logger.LogInformation("Error: User denied consent to required scopes");
                        }
                        else
                        {
                            // they said yes, set scopes they chose
                            request.ValidatedScopes.SetConsentedScopes(consent.ScopesConsented);
                            _logger.LogInformation("User consented to scopes: {scopes}", consent.ScopesConsented);

                            if (request.Client.AllowRememberConsent)
                            {
                                // remember consent
                                var scopes = Enumerable.Empty<string>();
                                if (consent.RememberConsent)
                                {
                                    // remember what user actually selected
                                    scopes = request.ValidatedScopes.GrantedResources.ToScopeNames();
                                    _logger.LogDebug("User indicated to remember consent for scopes: {scopes}", scopes);
                                }

                                await _consent.UpdateConsentAsync(request.Subject, request.Client, scopes);
                            }
                        }
                    }
                }

                return response;
            }

            return new InteractionResponse();
        }
    }
}