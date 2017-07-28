// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette/Derek Gray
// Created:				    2016-05-04
// Last Modified:		    2017-07-25
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.Identity
{
    public class SiteIdentityOptionsResolver : IOptions<IdentityOptions>
    {
        private IHttpContextAccessor httpContextAccessor;
        private MultiTenantOptions multiTenantOptions;
        private TokenOptions tokenOptions;

        public SiteIdentityOptionsResolver(
            IHttpContextAccessor httpContextAccessor,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<TokenOptions> tokenOptionsAccessor
           
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            multiTenantOptions = multiTenantOptionsAccessor.Value;
            tokenOptions = tokenOptionsAccessor.Value;
        }

        public IdentityOptions Value
        {
            get
            {
                var context = httpContextAccessor.HttpContext;
                var tenant = context.GetTenant<SiteContext>();
                var identityOptions = new IdentityOptions();

                identityOptions.Tokens = tokenOptions;

                identityOptions.Password.RequiredLength = tenant.MinRequiredPasswordLength;
                identityOptions.Password.RequireNonAlphanumeric = true; //tenant.PwdRequireNonAlpha; //default is true
                identityOptions.Password.RequireLowercase = true; //tenant.PwdRequireLowercase; //default is true
                identityOptions.Password.RequireUppercase = true; //tenant.PwdRequireUppercase; //default is true
                identityOptions.Password.RequireDigit = true; //tenant.PwdRequireDigit; // default is true

                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = tenant.MaxInvalidPasswordAttempts;

                identityOptions.SignIn.RequireConfirmedEmail = tenant.RequireConfirmedEmail;
                // this is a dangerous setting -existing users including admin can't login if they don't have a phone
                // number configured and there is no way for them to add the needed number
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = tenant.RequireConfirmedPhone;

                identityOptions.User.RequireUniqueEmail = true;
                identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // default value
                
                return identityOptions;
            }
        }

    }
}
