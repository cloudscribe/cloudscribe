// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-05-04
// Last Modified:		    2017-10-08
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Identity
{
    public class SiteIdentityOptionsResolver : IOptions<IdentityOptions>
    {
        private IHttpContextAccessor _httpContextAccessor;
        private MultiTenantOptions _multiTenantOptions;
        private TokenOptions _tokenOptions;
        private IIdentityOptionsFactory _optionsFactory;

        public SiteIdentityOptionsResolver(
            IHttpContextAccessor httpContextAccessor,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<TokenOptions> tokenOptionsAccessor,
            IIdentityOptionsFactory identityOptionsFactory
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _tokenOptions = tokenOptionsAccessor.Value;
            _optionsFactory = identityOptionsFactory;
        }

        public IdentityOptions Value
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var tenant = context.GetTenant<SiteContext>();
                var identityOptions = _optionsFactory.CreateOptions();

                identityOptions.Tokens = _tokenOptions;

                identityOptions.Password.RequiredLength = tenant.MinRequiredPasswordLength;
                identityOptions.Password.RequireNonAlphanumeric = tenant.PwdRequireNonAlpha;
                identityOptions.Password.RequireLowercase = tenant.PwdRequireLowercase;
                identityOptions.Password.RequireUppercase = tenant.PwdRequireUppercase;
                identityOptions.Password.RequireDigit = tenant.PwdRequireDigit;

                identityOptions.Lockout.MaxFailedAccessAttempts = tenant.MaxInvalidPasswordAttempts;

                identityOptions.Lockout.AllowedForNewUsers = true;
                
                identityOptions.SignIn.RequireConfirmedEmail = tenant.RequireConfirmedEmail;

                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                
                return identityOptions;
            }
        }
    }
}
