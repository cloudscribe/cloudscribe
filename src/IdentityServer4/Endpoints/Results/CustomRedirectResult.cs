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
    public class CustomRedirectResult : IEndpointResult
    {
        private readonly ValidatedAuthorizeRequest _request;
        private readonly string _url;

        public CustomRedirectResult(ValidatedAuthorizeRequest request, string url)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (url.IsMissing()) throw new ArgumentNullException(nameof(url));

            _request = request;
            _url = url;
        }

        internal CustomRedirectResult(
            ValidatedAuthorizeRequest request,
            string url,
            IdentityServerOptions options) 
            : this(request, url)
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

            var returnUrl = context.Request.PathBase.ToString().EnsureTrailingSlash() + Constants.ProtocolRoutePaths.Authorize;
            returnUrl = returnUrl.AddQueryString(_request.Raw.ToQueryString());

            if (!_url.IsLocalUrl())
            {
                // this converts the relative redirect path to an absolute one if we're 
                // redirecting to a different server
                returnUrl = context.GetIdentityServerBaseUrl().EnsureTrailingSlash() + returnUrl.RemoveLeadingSlash();
            }

            var url = _url.AddQueryString(_options.UserInteractionOptions.CustomRedirectReturnUrlParameter, returnUrl);
            context.Response.RedirectToAbsoluteUrl(url);

            return Task.FromResult(0);
        }
    }
}
