// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace IdentityServer4.Services.Default
{
    class DefaultIdentityServerInteractionService : IIdentityServerInteractionService
    {
        private readonly IdentityServerOptions _options;
        private readonly IHttpContextAccessor _context;
        private readonly IMessageStore<LogoutMessage> _logoutMessageStore;
        private readonly IMessageStore<ErrorMessage> _errorMessageStore;
        private readonly IMessageStore<ConsentResponse> _consentMessageStore;
        private readonly IPersistedGrantService _grants;
        private readonly IClientSessionService _clientSessionService;
        private readonly ISessionIdService _sessionIdService;
        private readonly ILogger<DefaultIdentityServerInteractionService> _logger;
        private readonly ReturnUrlParser _returnUrlParser;

        public DefaultIdentityServerInteractionService(
            IdentityServerOptions options,
            IHttpContextAccessor context,
            IMessageStore<LogoutMessage> logoutMessageStore,
            IMessageStore<ErrorMessage> errorMessageStore,
            IMessageStore<ConsentResponse> consentMessageStore,
            IPersistedGrantService grants, 
            IClientSessionService clientSessionService,
            ISessionIdService sessionIdService,
            ReturnUrlParser returnUrlParser,
            ILogger<DefaultIdentityServerInteractionService> logger)
        {
            _options = options;
            _context = context;
            _logoutMessageStore = logoutMessageStore;
            _errorMessageStore = errorMessageStore;
            _consentMessageStore = consentMessageStore;
            _grants = grants;
            _clientSessionService = clientSessionService;
            _sessionIdService = sessionIdService;
            _returnUrlParser = returnUrlParser;
            _logger = logger;
        }

        public async Task<AuthorizationRequest> GetAuthorizationContextAsync(string returnUrl)
        {
            var result = await _returnUrlParser.ParseAsync(returnUrl);

            if (result != null)
            {
                _logger.LogTrace("AuthorizationRequest being returned");
            }
            else
            {
                _logger.LogTrace("No AuthorizationRequest being returned");
            }

            return result;
        }

        public async Task<LogoutRequest> GetLogoutContextAsync(string logoutId)
        {
            var msg = await _logoutMessageStore.ReadAsync(logoutId);
            var iframeUrl = await _context.HttpContext.GetIdentityServerSignoutFrameCallbackUrlAsync(msg?.Data?.SessionId);

            if (iframeUrl != null && logoutId != null)
            {
                iframeUrl = iframeUrl.AddQueryString(_options.UserInteractionOptions.LogoutIdParameter, logoutId);
            }

            return new LogoutRequest(iframeUrl, msg?.Data);
        }

        public async Task<string> CreateLogoutContextAsync()
        {
            var sid = await _sessionIdService.GetCurrentSessionIdAsync();
            if (sid != null)
            {
                await _clientSessionService.EnsureClientListCookieAsync(sid);

                var msg = new MessageWithId<LogoutMessage>(new LogoutMessage() { SessionId = sid });

                var id = msg.Id;
                await _logoutMessageStore.WriteAsync(id, msg);

                return id;
            }

            return null;
        }

        public async Task<ErrorMessage> GetErrorContextAsync(string errorId)
        {
            if (errorId != null)
            { 
                var result = await _errorMessageStore.ReadAsync(errorId);
                var data = result?.Data;
                if (data != null)
                {
                    _logger.LogTrace("Error context loaded");
                }
                else
                {
                    _logger.LogTrace("No error context found");
                }
                return data;
            }

            _logger.LogTrace("No error context found");

            return null;
        }

        public async Task GrantConsentAsync(AuthorizationRequest request, ConsentResponse consent, string subject = null)
        {
            if (subject == null)
            {
                var user = await _context.HttpContext.GetIdentityServerUserAsync();
                subject = user?.GetSubjectId();
            }

            if (subject == null) throw new ArgumentNullException("User is not currently authenticated, and no subject id passed", nameof(subject));

            var consentRequest = new ConsentRequest(request, subject);
            await _consentMessageStore.WriteAsync(consentRequest.Id, new Message<ConsentResponse>(consent));
        }

        public bool IsValidReturnUrl(string returnUrl)
        {
            var result = _returnUrlParser.IsValidReturnUrl(returnUrl);

            if (result)
            {
                _logger.LogTrace("IsValidReturnUrl true");
            }
            else
            {
                _logger.LogTrace("IsValidReturnUrl false");
            }

            return result;
        }

        public async Task<IEnumerable<Consent>> GetAllUserConsentsAsync()
        {
            var user = await _context.HttpContext.GetIdentityServerUserAsync();
            if (user != null)
            {
                var subject = user.GetSubjectId();
                return await _grants.GetAllGrantsAsync(subject);
            }

            return Enumerable.Empty<Consent>();
        }

        public async Task RevokeUserConsentAsync(string clientId)
        {
            var user = await _context.HttpContext.GetIdentityServerUserAsync();
            if (user != null)
            {
                var subject = user.GetSubjectId();
                await _grants.RemoveAllGrantsAsync(subject, clientId);
            }
        }

        public async Task RevokeTokensForCurrentSessionAsync()
        {
            var user = await _context.HttpContext.GetIdentityServerUserAsync();
            if (user != null)
            {
                var subject = user.GetSubjectId();
                var clients = await _clientSessionService.GetClientListAsync();
                foreach (var client in clients)
                {
                    await _grants.RemoveAllGrantsAsync(subject, client);
                }
            }
        }
    }
}
