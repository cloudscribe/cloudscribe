// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-29
// Last Modified:		    2015-10-17
// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.MicrosoftAccount/MicrosoftAccountMiddleware.cs


using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.MicrosoftAccount;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;

namespace cloudscribe.Core.Identity.OAuth
{
    public class MultiTenantMicrosoftAccountMiddleware : MultiTenantOAuthMiddleware<MicrosoftAccountOptions>
    {
        /// <summary>
        /// Initializes a new <see cref="MicrosoftAccountAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the HTTP pipeline to invoke.</param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="sharedOptions"></param>
        /// <param name="options">Configuration options for the middleware.</param>
        /// <param name="configureOptions"></param>
        public MultiTenantMicrosoftAccountMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            IOptions<MultiTenantOptions> multiTenantOptionsAccesor,
            IUrlEncoder encoder,
            IOptions<SharedAuthenticationOptions> sharedOptions,
            IOptions<MicrosoftAccountOptions> options)
            : base(
                  next, 
                  dataProtectionProvider, 
                  loggerFactory, 
                  encoder,
                  siteResolver,
                  multiTenantOptionsAccesor,
                  sharedOptions, 
                  options.Value)
        {
            if (Options.Scope.Count == 0)
            {
                // LiveID requires a scope string, so if the user didn't set one we go for the least possible.
                // TODO: Should we just add these by default when we create the Options?
                Options.Scope.Add("wl.basic");
            }

            this.loggerFactory = loggerFactory;
            this.siteResolver = siteResolver;
            multiTenantOptions = multiTenantOptionsAccesor.Value;
            siteRepo = siteRepository;
        }

        private ILoggerFactory loggerFactory;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="MicrosoftAccountOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<MicrosoftAccountOptions> CreateHandler()
        {
            return new MultiTenantMicrosoftAccountHandler(Backchannel, siteResolver, siteRepo, multiTenantOptions, loggerFactory);
        }

    }
}
