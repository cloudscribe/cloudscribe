// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Threading.Tasks;
using IdentityServer4.Configuration;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Validation;
using IdentityServer4.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer4.Endpoints.Results
{
    public class ConsentPageResult : IEndpointResult
    {
        private readonly ValidatedAuthorizeRequest _request;

        public ConsentPageResult(ValidatedAuthorizeRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            _request = request;
        }

        internal ConsentPageResult(
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

            var returnUrl = context.Request.PathBase.ToString().EnsureTrailingSlash() + Constants.ProtocolRoutePaths.AuthorizeAfterConsent;
            returnUrl = returnUrl.AddQueryString(_request.Raw.ToQueryString());

            var consentUrl = _options.UserInteractionOptions.ConsentUrl;
            if (!consentUrl.IsLocalUrl())
            {
                // this converts the relative redirect path to an absolute one if we're 
                // redirecting to a different server
                returnUrl = context.GetIdentityServerBaseUrl().EnsureTrailingSlash() + returnUrl.RemoveLeadingSlash();
            }

            var url = consentUrl.AddQueryString(_options.UserInteractionOptions.ConsentReturnUrlParameter, returnUrl);
            context.Response.RedirectToAbsoluteUrl(url);

            return Task.FromResult(0);
        }
    }
}
