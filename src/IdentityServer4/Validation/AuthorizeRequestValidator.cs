// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Logging;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer4.Validation
{
    internal class AuthorizeRequestValidator : IAuthorizeRequestValidator
    {
        private readonly IdentityServerOptions _options;
        private readonly IClientStore _clients;
        private readonly ICustomAuthorizeRequestValidator _customValidator;
        private readonly IRedirectUriValidator _uriValidator;
        private readonly ScopeValidator _scopeValidator;
        private readonly ISessionIdService _sessionId;
        private readonly ILogger _logger;

        private readonly ResponseTypeEqualityComparer
            _responseTypeEqualityComparer = new ResponseTypeEqualityComparer();

        public AuthorizeRequestValidator(
            IdentityServerOptions options, 
            IClientStore clients, 
            ICustomAuthorizeRequestValidator customValidator, 
            IRedirectUriValidator uriValidator, 
            ScopeValidator scopeValidator,
            ISessionIdService sessionId,
            ILogger<AuthorizeRequestValidator> logger)
        {
            _options = options;
            _clients = clients;
            _customValidator = customValidator;
            _uriValidator = uriValidator;
            _scopeValidator = scopeValidator;
            _sessionId = sessionId;
            _logger = logger;
        }

        public async Task<AuthorizeRequestValidationResult> ValidateAsync(NameValueCollection parameters, ClaimsPrincipal subject = null)
        {
            _logger.LogDebug("Start authorize request protocol validation");

            var request = new ValidatedAuthorizeRequest
            {
                Options = _options,
                Subject = subject ?? Principal.Anonymous
            };
            
            if (parameters == null)
            {
                _logger.LogCritical("Parameters are null.");
                throw new ArgumentNullException(nameof(parameters));
            }

            request.Raw = parameters;

            // validate client_id and redirect_uri
            var clientResult = await ValidateClientAsync(request);
            if (clientResult.IsError)
            {
                return clientResult;
            }

            // state, response_type, response_mode
            var mandatoryResult = ValidateCoreParameters(request);
            if (mandatoryResult.IsError)
            {
                return mandatoryResult;
            }

            // scope, scope restrictions and plausability
            var scopeResult = await ValidateScopeAsync(request);
            if (scopeResult.IsError)
            {
                return scopeResult;
            }

            // nonce, prompt, acr_values, login_hint etc.
            var optionalResult = await ValidateOptionalParametersAsync(request);
            if (optionalResult.IsError)
            {
                return optionalResult;
            }

            // custom validator
            _logger.LogDebug("Calling into custom validator: {type}", _customValidator.GetType().FullName);
            var customResult = await _customValidator.ValidateAsync(request);

            if (customResult.IsError)
            {
                LogError("Error in custom validation: " + customResult.Error, request);
                return Invalid(request, customResult.Error);
            }

            _logger.LogTrace("Authorize request protocol validation successful");

            return Valid(request);
        }

        async Task<AuthorizeRequestValidationResult> ValidateClientAsync(ValidatedAuthorizeRequest request)
        {
            //////////////////////////////////////////////////////////
            // client_id must be present
            /////////////////////////////////////////////////////////
            var clientId = request.Raw.Get(OidcConstants.AuthorizeRequest.ClientId);
            if (clientId.IsMissingOrTooLong(_options.InputLengthRestrictions.ClientId))
            {
                LogError("client_id is missing or too long", request);
                return Invalid(request);
            }

            request.ClientId = clientId;


            //////////////////////////////////////////////////////////
            // redirect_uri must be present, and a valid uri
            //////////////////////////////////////////////////////////
            var redirectUri = request.Raw.Get(OidcConstants.AuthorizeRequest.RedirectUri);

            if (redirectUri.IsMissingOrTooLong(_options.InputLengthRestrictions.RedirectUri))
            {
                LogError("redirect_uri is missing or too long", request);
                return Invalid(request);
            }

            Uri uri;
            if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out uri))
            {
                LogError("malformed redirect_uri: " + redirectUri, request);
                return Invalid(request);
            }

            request.RedirectUri = redirectUri;


            //////////////////////////////////////////////////////////
            // check for valid client
            //////////////////////////////////////////////////////////
            var client = await _clients.FindEnabledClientByIdAsync(request.ClientId);
            if (client == null)
            {
                LogError("Unknown client or not enabled: " + request.ClientId, request);
                return Invalid(request, OidcConstants.AuthorizeErrors.UnauthorizedClient);
            }

            request.Client = client;

            //////////////////////////////////////////////////////////
            // check if client protocol type is oidc
            //////////////////////////////////////////////////////////
            if (request.Client.ProtocolType != IdentityServerConstants.ProtocolTypes.OpenIdConnect)
            {
                LogError($"Invalid protocol type for OIDC authorize endpoint: {request.Client.ProtocolType}", request);
                return Invalid(request, OidcConstants.AuthorizeErrors.UnauthorizedClient);
            }

            //////////////////////////////////////////////////////////
            // check if redirect_uri is valid
            //////////////////////////////////////////////////////////
            if (await _uriValidator.IsRedirectUriValidAsync(request.RedirectUri, request.Client) == false)
            {
                LogError("Invalid redirect_uri: " + request.RedirectUri, request);
                return Invalid(request, OidcConstants.AuthorizeErrors.UnauthorizedClient);
            }

            return Valid(request);
        }

        private AuthorizeRequestValidationResult ValidateCoreParameters(ValidatedAuthorizeRequest request)
        {
            //////////////////////////////////////////////////////////
            // check state
            //////////////////////////////////////////////////////////
            var state = request.Raw.Get(OidcConstants.AuthorizeRequest.State);
            if (state.IsPresent())
            {
                request.State = state;
            }

            //////////////////////////////////////////////////////////
            // response_type must be present and supported
            //////////////////////////////////////////////////////////
            var responseType = request.Raw.Get(OidcConstants.AuthorizeRequest.ResponseType);
            if (responseType.IsMissing())
            {
                LogError("Missing response_type", request);
                return Invalid(request, OidcConstants.AuthorizeErrors.UnsupportedResponseType);
            }

            // The responseType may come in in an unconventional order.  
            // Use an IEqualityComparer that doesn't care about the order of multiple values.
            // Per https://tools.ietf.org/html/rfc6749#section-3.1.1 - 
            // 'Extension response types MAY contain a space-delimited (%x20) list of
            // values, where the order of values does not matter (e.g., response
            // type "a b" is the same as "b a").'
            // http://openid.net/specs/oauth-v2-multiple-response-types-1_0-03.html#terminology - 
            // 'If a response type contains one of more space characters (%20), it is compared 
            // as a space-delimited list of values in which the order of values does not matter.'
            if (!Constants.SupportedResponseTypes.Contains(responseType, _responseTypeEqualityComparer))
            {
                LogError("Response type not supported: " + responseType, request);
                return Invalid(request, OidcConstants.AuthorizeErrors.UnsupportedResponseType);
            }

            // Even though the responseType may have come in in an unconventional order,
            // we still need the request's ResponseType property to be set to the
            // conventional, supported response type.
            request.ResponseType = Constants.SupportedResponseTypes.First(
                supportedResponseType => _responseTypeEqualityComparer.Equals(supportedResponseType, responseType));

            //////////////////////////////////////////////////////////
            // match response_type to grant type
            //////////////////////////////////////////////////////////
            request.GrantType = Constants.ResponseTypeToGrantTypeMapping[request.ResponseType];

            // set default response mode for flow; this is needed for any client error processing below
            request.ResponseMode = Constants.AllowedResponseModesForGrantType[request.GrantType].First();

            //////////////////////////////////////////////////////////
            // check if flow is allowed at authorize endpoint
            //////////////////////////////////////////////////////////
            if (!Constants.AllowedGrantTypesForAuthorizeEndpoint.Contains(request.GrantType))
            {
                LogError("Invalid grant type", request);
                return Invalid(request);
            }

            //////////////////////////////////////////////////////////
            // check if PKCE is required and validate parameters
            //////////////////////////////////////////////////////////
            if (request.GrantType == GrantType.AuthorizationCode || request.GrantType == GrantType.Hybrid)
            {
                if (request.Client.RequirePkce)
                {
                    _logger.LogDebug("Client requires a proof key for code exchange. Starting PKCE validation");

                    /////////////////////////////////////////////////////////////////////////////
                    // validate code_challenge and code_challenge_method
                    /////////////////////////////////////////////////////////////////////////////
                    var proofKeyResult = ValidatePkceParameters(request);
                    if (proofKeyResult.IsError)
                    {
                        return proofKeyResult;
                    }
                }
            }

            //////////////////////////////////////////////////////////
            // check response_mode parameter and set response_mode
            //////////////////////////////////////////////////////////

            // check if response_mode parameter is present and valid
            var responseMode = request.Raw.Get(OidcConstants.AuthorizeRequest.ResponseMode);
            if (responseMode.IsPresent())
            {
                if (Constants.SupportedResponseModes.Contains(responseMode))
                {
                    if (Constants.AllowedResponseModesForGrantType[request.GrantType].Contains(responseMode))
                    {
                        request.ResponseMode = responseMode;
                    }
                    else
                    {
                        LogError("Invalid response_mode for flow: " + responseMode, request);
                        return Invalid(request, OidcConstants.AuthorizeErrors.UnsupportedResponseType);
                    }
                }
                else
                {
                    LogError("Unsupported response_mode: " + responseMode, request);
                    return Invalid(request, OidcConstants.AuthorizeErrors.UnsupportedResponseType);
                }
            }

            
            //////////////////////////////////////////////////////////
            // check if grant type is allowed for client
            //////////////////////////////////////////////////////////
            if (!request.Client.AllowedGrantTypes.Contains(request.GrantType))
            {
                LogError("Invalid grant type for client: " + request.GrantType, request);
                return Invalid(request, OidcConstants.AuthorizeErrors.UnauthorizedClient);
            }

            //////////////////////////////////////////////////////////
            // check if response type contains an access token, 
            // and if client is allowed to request access token via browser
            //////////////////////////////////////////////////////////
            var responseTypes = responseType.FromSpaceSeparatedString();
            if (responseTypes.Contains(OidcConstants.ResponseTypes.Token))
            {
                if (!request.Client.AllowAccessTokensViaBrowser)
                {
                    LogError("Client requested access token - but client is not configured to receive access tokens via browser", request);
                    return Invalid(request);
                }
            }

            return Valid(request);
        }

        private AuthorizeRequestValidationResult ValidatePkceParameters(ValidatedAuthorizeRequest request)
        {
            var fail = Invalid(request);

            var codeChallenge = request.Raw.Get(OidcConstants.AuthorizeRequest.CodeChallenge);
            if (codeChallenge.IsMissing())
            {
                LogError("code_challenge is missing", request);
                fail.ErrorDescription = "code challenge required";
                return fail;
            }

            if (codeChallenge.Length < _options.InputLengthRestrictions.CodeChallengeMinLength ||
                codeChallenge.Length > _options.InputLengthRestrictions.CodeChallengeMaxLength)
            {
                LogError("code_challenge is either too short or too long", request);
                return fail;
            }

            request.CodeChallenge = codeChallenge;

            var codeChallengeMethod = request.Raw.Get(OidcConstants.AuthorizeRequest.CodeChallengeMethod);
            if (codeChallengeMethod.IsMissing())
            {
                _logger.LogDebug("Missing code_challenge_method, defaulting to plain");
                codeChallengeMethod = OidcConstants.CodeChallengeMethods.Plain;
            }

            if (!Constants.SupportedCodeChallengeMethods.Contains(codeChallengeMethod))
            {
                LogError("Unsupported code_challenge_method: " + codeChallengeMethod, request);
                fail.ErrorDescription = "transform algorithm not supported";
                return fail;
            }

            // check if plain method is allowed
            if (codeChallengeMethod == OidcConstants.CodeChallengeMethods.Plain)
            {
                if (!request.Client.AllowPlainTextPkce)
                {
                    LogError("code_challenge_method of plain is not allowed", request);
                    fail.ErrorDescription = "transform algorithm not supported";
                    return fail;
                }
            }

            request.CodeChallengeMethod = codeChallengeMethod;

            return Valid(request);
        }

        private async Task<AuthorizeRequestValidationResult> ValidateScopeAsync(ValidatedAuthorizeRequest request)
        {
            //////////////////////////////////////////////////////////
            // scope must be present
            //////////////////////////////////////////////////////////
            var scope = request.Raw.Get(OidcConstants.AuthorizeRequest.Scope);
            if (scope.IsMissing())
            {
                LogError("scope is missing", request);
                return Invalid(request);
            }

            if (scope.Length > _options.InputLengthRestrictions.Scope)
            {
                LogError("scopes too long.", request);
                return Invalid(request);
            }

            request.RequestedScopes = scope.FromSpaceSeparatedString().Distinct().ToList();

            if (request.RequestedScopes.Contains(IdentityServerConstants.StandardScopes.OpenId))
            {
                request.IsOpenIdRequest = true;
            }

            //////////////////////////////////////////////////////////
            // check scope vs response_type plausability
            //////////////////////////////////////////////////////////
            var requirement = Constants.ResponseTypeToScopeRequirement[request.ResponseType];
            if (requirement == Constants.ScopeRequirement.Identity ||
                requirement == Constants.ScopeRequirement.IdentityOnly)
            {
                if (request.IsOpenIdRequest == false)
                {
                    LogError("response_type requires the openid scope", request);
                    return Invalid(request);
                }
            }

            //////////////////////////////////////////////////////////
            // check if scopes are valid/supported and check for resource scopes
            //////////////////////////////////////////////////////////
            if (await _scopeValidator.AreScopesValidAsync(request.RequestedScopes) == false)
            {
                return Invalid(request, OidcConstants.AuthorizeErrors.InvalidScope);
            }

            if (_scopeValidator.ContainsOpenIdScopes && !request.IsOpenIdRequest)
            {
                LogError("Identity related scope requests, but no openid scope", request);
                return Invalid(request, OidcConstants.AuthorizeErrors.InvalidScope);
            }

            if (_scopeValidator.ContainsApiResourceScopes)
            {
                request.IsApiResourceRequest = true;
            }

            //////////////////////////////////////////////////////////
            // check scopes and scope restrictions
            //////////////////////////////////////////////////////////
            if (await _scopeValidator.AreScopesAllowedAsync(request.Client, request.RequestedScopes) == false)
            {
                return Invalid(request, OidcConstants.AuthorizeErrors.UnauthorizedClient);
            }

            request.ValidatedScopes = _scopeValidator;

            //////////////////////////////////////////////////////////
            // check id vs resource scopes and response types plausability
            //////////////////////////////////////////////////////////
            if (!_scopeValidator.IsResponseTypeValid(request.ResponseType))
            {
                return Invalid(request, OidcConstants.AuthorizeErrors.InvalidScope);
            }

            return Valid(request);
        }

        private async Task<AuthorizeRequestValidationResult> ValidateOptionalParametersAsync(ValidatedAuthorizeRequest request)
        {
            //////////////////////////////////////////////////////////
            // check nonce
            //////////////////////////////////////////////////////////
            var nonce = request.Raw.Get(OidcConstants.AuthorizeRequest.Nonce);
            if (nonce.IsPresent())
            {
                if (nonce.Length > _options.InputLengthRestrictions.Nonce)
                {
                    LogError("Nonce too long", request);
                    return Invalid(request);
                }

                request.Nonce = nonce;
            }
            else
            {
                if (request.GrantType == GrantType.Implicit ||
                    request.GrantType == GrantType.Hybrid)
                {
                    // only openid requests require nonce
                    if (request.IsOpenIdRequest)
                    {
                        LogError("Nonce required for implicit and hybrid flow with openid scope", request);
                        return Invalid(request);
                    }
                }
            }


            //////////////////////////////////////////////////////////
            // check prompt
            //////////////////////////////////////////////////////////
            var prompt = request.Raw.Get(OidcConstants.AuthorizeRequest.Prompt);
            if (prompt.IsPresent())
            {
                if (Constants.SupportedPromptModes.Contains(prompt))
                {
                    request.PromptMode = prompt;
                }
                else
                {
                    _logger.LogDebug("Unsupported prompt mode - ignored: " + prompt);
                }
            }

            //////////////////////////////////////////////////////////
            // check ui locales
            //////////////////////////////////////////////////////////
            var uilocales = request.Raw.Get(OidcConstants.AuthorizeRequest.UiLocales);
            if (uilocales.IsPresent())
            {
                if (uilocales.Length > _options.InputLengthRestrictions.UiLocale)
                {
                    LogError("UI locale too long", request);
                    return Invalid(request);
                }

                request.UiLocales = uilocales;
            }

            //////////////////////////////////////////////////////////
            // check display
            //////////////////////////////////////////////////////////
            var display = request.Raw.Get(OidcConstants.AuthorizeRequest.Display);
            if (display.IsPresent())
            {
                if (Constants.SupportedDisplayModes.Contains(display))
                {
                    request.DisplayMode = display;
                }

                _logger.LogDebug("Unsupported display mode - ignored: " + display);
            }

            //////////////////////////////////////////////////////////
            // check max_age
            //////////////////////////////////////////////////////////
            var maxAge = request.Raw.Get(OidcConstants.AuthorizeRequest.MaxAge);
            if (maxAge.IsPresent())
            {
                int seconds;
                if (int.TryParse(maxAge, out seconds))
                {
                    if (seconds >= 0)
                    {
                        request.MaxAge = seconds;
                    }
                    else
                    {
                        LogError("Invalid max_age.", request);
                        return Invalid(request);
                    }
                }
                else
                {
                    LogError("Invalid max_age.", request);
                    return Invalid(request);
                }
            }

            //////////////////////////////////////////////////////////
            // check login_hint
            //////////////////////////////////////////////////////////
            var loginHint = request.Raw.Get(OidcConstants.AuthorizeRequest.LoginHint);
            if (loginHint.IsPresent())
            {
                if (loginHint.Length > _options.InputLengthRestrictions.LoginHint)
                {
                    LogError("Login hint too long", request);
                    return Invalid(request);
                }

                request.LoginHint = loginHint;
            }

            //////////////////////////////////////////////////////////
            // check acr_values
            //////////////////////////////////////////////////////////
            var acrValues = request.Raw.Get(OidcConstants.AuthorizeRequest.AcrValues);
            if (acrValues.IsPresent())
            {
                if (acrValues.Length > _options.InputLengthRestrictions.AcrValues)
                {
                    LogError("Acr values too long", request);
                    return Invalid(request);
                }

                request.AuthenticationContextReferenceClasses = acrValues.FromSpaceSeparatedString().Distinct().ToList();
            }

            //////////////////////////////////////////////////////////
            // check custom acr_values: idp
            //////////////////////////////////////////////////////////
            var idp = request.GetIdP();
            if (idp.IsPresent())
            {
                // if idp is present but client does not allow it, strip it from the request message
                if (request.Client.IdentityProviderRestrictions != null && request.Client.IdentityProviderRestrictions.Any())
                {
                    if (!request.Client.IdentityProviderRestrictions.Contains(idp))
                    {
                        _logger.LogWarning("idp requested ({idp}) is not in client restriction list.", idp);
                        request.RemoveIdP();
                    }
                }
            }

            //////////////////////////////////////////////////////////
            // check session cookie
            //////////////////////////////////////////////////////////
            if (_options.Endpoints.EnableCheckSessionEndpoint && 
                request.Subject.IsAuthenticated())
            {
                var sessionId = await _sessionId.GetCurrentSessionIdAsync();
                if (sessionId.IsPresent())
                {
                    request.SessionId = sessionId;
                }
                else
                {
                    LogError("Check session endpoint enabled, but SessionId is missing", request);
                }
            }

            return Valid(request);
        }

        private AuthorizeRequestValidationResult Invalid(ValidatedAuthorizeRequest request, string error = OidcConstants.AuthorizeErrors.InvalidRequest)
        {
            var result = new AuthorizeRequestValidationResult
            {
                IsError = true,
                Error = error,
                ValidatedRequest = request
            };

            return result;
        }

        private AuthorizeRequestValidationResult Valid(ValidatedAuthorizeRequest request)
        {
            var result = new AuthorizeRequestValidationResult
            {
                IsError = false,
                ValidatedRequest = request
            };

            return result;
        }

        private void LogError(string message, ValidatedAuthorizeRequest request)
        {
            var details = new AuthorizeRequestValidationLog(request);
            _logger.LogError(message + "\n{validationDetails}", details);
        }
    }
}