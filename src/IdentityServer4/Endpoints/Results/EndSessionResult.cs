// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Validation;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Extensions;
using System;
using IdentityServer4.Services;

namespace IdentityServer4.Endpoints.Results
{
    public class EndSessionResult : IEndpointResult
    {
        private readonly EndSessionValidationResult _result;

        public EndSessionResult(EndSessionValidationResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            _result = result;
        }

        internal EndSessionResult(
            EndSessionValidationResult result,
            IdentityServerOptions options,
            IClientSessionService clientSessionService,
            IMessageStore<LogoutMessage> logoutMessageStore)
            : this(result)
        {
            _options = options;
            _clientSessionService = clientSessionService;
            _logoutMessageStore = logoutMessageStore;
        }

        private IdentityServerOptions _options;
        private IMessageStore<LogoutMessage> _logoutMessageStore;
        private IClientSessionService _clientSessionService;

        void Init(HttpContext context)
        {
            _options = _options ?? context.RequestServices.GetRequiredService<IdentityServerOptions>();
            _clientSessionService = _clientSessionService ?? context.RequestServices.GetRequiredService<IClientSessionService>();
            _logoutMessageStore = _logoutMessageStore ?? context.RequestServices.GetRequiredService<IMessageStore<LogoutMessage>>();
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            Init(context);

            var validatedRequest = _result.IsError ? null : _result.ValidatedRequest;

            string id = null;

            if (validatedRequest != null)
            {
                var msg = new MessageWithId<LogoutMessage>(new LogoutMessage(validatedRequest));
                id = msg.Id;

                await _logoutMessageStore.WriteAsync(id, msg);
                await _clientSessionService.EnsureClientListCookieAsync(validatedRequest.SessionId);
            }

            var redirect = _options.UserInteractionOptions.LogoutUrl;

            if (redirect.IsLocalUrl())
            {
                redirect = context.GetIdentityServerRelativeUrl(redirect);
            }

            if (id != null)
            {
                redirect = redirect.AddQueryString(_options.UserInteractionOptions.LogoutIdParameter, id);
            }

            context.Response.Redirect(redirect);
        }
    }
}
