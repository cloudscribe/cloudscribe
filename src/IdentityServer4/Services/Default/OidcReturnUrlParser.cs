// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Services.Default
{
    internal class OidcReturnUrlParser : IReturnUrlParser
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger _logger;
        private readonly IAuthorizeRequestValidator _validator;

        public OidcReturnUrlParser(
            IHttpContextAccessor httpContext, 
            IAuthorizeRequestValidator validator,
            ILogger<OidcReturnUrlParser> logger)
        {
            _context = httpContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task<AuthorizationRequest> ParseAsync(string returnUrl)
        {
            if (IsValidReturnUrl(returnUrl))
            {
                var parameters = returnUrl.ReadQueryStringAsNameValueCollection();
                var user = await _context.HttpContext.GetIdentityServerUserAsync();
                var result = await _validator.ValidateAsync(parameters, user);
                if (!result.IsError)
                {
                    _logger.LogTrace("AuthorizationRequest being returned");
                    return new AuthorizationRequest(result.ValidatedRequest);
                }
            }

            _logger.LogTrace("No AuthorizationRequest being returned");
            return null;
        }

        public bool IsValidReturnUrl(string returnUrl)
        {
            // TODO: allow remote urls, once supported
            if (returnUrl.IsLocalUrl())
            {
                var index = returnUrl.IndexOf('?');
                if (index >= 0)
                {
                    returnUrl = returnUrl.Substring(0, index);
                }

                if (returnUrl.EndsWith(Constants.ProtocolRoutePaths.Authorize, StringComparison.Ordinal) ||
                    returnUrl.EndsWith(Constants.ProtocolRoutePaths.AuthorizeAfterLogin, StringComparison.Ordinal) ||
                    returnUrl.EndsWith(Constants.ProtocolRoutePaths.AuthorizeAfterConsent, StringComparison.Ordinal))
                {
                    _logger.LogTrace("returnUrl is valid");
                    return true;
                }
            }

            _logger.LogTrace("returnUrl is not valid");
            return false;
        }
    }
}
