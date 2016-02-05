// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-25
// Last Modified:		    2016-02-05
// 

using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Facebook;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.WebEncoders;
using SaasKit.Multitenancy;


namespace cloudscribe.Core.Identity.OAuth
{
    /// <summary>
    /// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Facebook/FacebookMiddleware.cs
    /// </summary>
    public class MultiTenantFacebookMiddleware : MultiTenantOAuthMiddleware<FacebookOptions>
    {
        public MultiTenantFacebookMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            //ISiteResolver siteResolver,
            IHttpContextAccessor contextAccessor,
            ITenantResolver<SiteSettings> siteResolver,
            ISiteRepository siteRepository,
            IOptions<MultiTenantOptions> multiTenantOptionsAccesor,
            IUrlEncoder encoder,
            IOptions<SharedAuthenticationOptions> sharedOptions,
            FacebookOptions options)
            : base(next, 
                  dataProtectionProvider,
                  contextAccessor,
                  loggerFactory, 
                  encoder, 
                  siteResolver,
                  multiTenantOptionsAccesor,
                  sharedOptions, 
                  options)
        {
            //if (string.IsNullOrEmpty(Options.AppId))
            //{
            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.AppId)));
            //}
            //if (string.IsNullOrEmpty(Options.AppSecret))
            //{
            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.AppSecret)));
            //}
            this.loggerFactory = loggerFactory;
            this.contextAccessor = contextAccessor;
            this.siteResolver = siteResolver;
            multiTenantOptions = multiTenantOptionsAccesor.Value;
            siteRepo = siteRepository;
        }

        private ILoggerFactory loggerFactory;
        private IHttpContextAccessor contextAccessor;
        private ITenantResolver<SiteSettings> siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;

        protected override AuthenticationHandler<FacebookOptions> CreateHandler()
        {
            return new MultiTenantFacebookHandler(
                Backchannel,
                contextAccessor,
                siteResolver, 
                siteRepo,
                multiTenantOptions, 
                loggerFactory);
        }

    }
}
