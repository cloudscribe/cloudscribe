// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServer4.Validation
{
    public class NotSupportedResouceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ILogger<NotSupportedResouceOwnerPasswordValidator> _logger;

        public NotSupportedResouceOwnerPasswordValidator(ILogger<NotSupportedResouceOwnerPasswordValidator> logger)
        {
            _logger = logger;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.UnsupportedGrantType);

            _logger.LogWarning("Resource owner password credential type not supported. Configure an IResourceOwnerPasswordValidator.");
            return Task.FromResult(0);
        }
    }
}