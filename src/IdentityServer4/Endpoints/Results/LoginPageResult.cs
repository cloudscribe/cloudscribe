// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using IdentityServer4.Hosting;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Extensions;
using IdentityServer4.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Endpoints.Results
{
    public class LoginPageResult : IEndpointResult
    {
        private readonly ValidatedAuthorizeRequest _request;

        public LoginPageResult(ValidatedAuthorizeRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _request = request;
        }

        internal LoginPageResult(
            ValidatedAuthorizeRequest request,
            IdentityServerOptions options) 
            : this(request)
        {
            _options = options;
        }

        private IdentityServerOptions _options;

        void Init(HttpContext context)
        {
            _options = _options ?? context.RequestServices.GetRequiredService<IdentityServerOptions>();
        }

        public Task ExecuteAsync(HttpContext context)
        {
            Init(context);

            var returnUrl = context.Request.PathBase.ToString().EnsureTrailingSlash() + Constants.ProtocolRoutePaths.AuthorizeAfterLogin;
            returnUrl = returnUrl.AddQueryString(_request.Raw.ToQueryString());

            var loginUrl = _options.UserInteractionOptions.LoginUrl;
            if (!loginUrl.IsLocalUrl())
            {
                // this converts the relative redirect path to an absolute one if we're 
                // redirecting to a different server
                returnUrl = context.GetIdentityServerBaseUrl().EnsureTrailingSlash() + returnUrl.RemoveLeadingSlash();
            }

            var url = loginUrl.AddQueryString(_options.UserInteractionOptions.LoginReturnUrlParameter, returnUrl);
            context.Response.RedirectToAbsoluteUrl(url);

            return Task.FromResult(0);
        }
    }
}
