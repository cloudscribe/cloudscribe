// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-29
// Last Modified:		    2015-09-04
// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Google/GoogleAuthenticationMiddleware.cs


using cloudscribe.Core.Models;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.OAuth;
using Microsoft.AspNet.Authentication.Google;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Framework.Internal;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;

namespace cloudscribe.Core.Identity.OAuth
{
    /// <summary>
    /// An ASP.NET middleware for authenticating users using Google OAuth 2.0.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Middleware are not disposable.")]
    public class MultiTenantGoogleAuthenticationMiddleware : MultiTenantOAuthAuthenticationMiddleware<GoogleAuthenticationOptions>
    {
        /// <summary>
        /// Initializes a new <see cref="GoogleAuthenticationMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the HTTP pipeline to invoke.</param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="sharedOptions"></param>
        /// <param name="options">Configuration options for the middleware.</param>
        /// <param name="configureOptions"></param>
        public MultiTenantGoogleAuthenticationMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            ISiteResolver siteResolver,
            ISiteRepository siteRepository,
            IOptions<MultiTenantOptions> multiTenantOptionsAccesor,
            IUrlEncoder encoder,
            IOptions<SharedAuthenticationOptions> sharedOptions,
            IOptions<GoogleAuthenticationOptions> options,
            ConfigureOptions<GoogleAuthenticationOptions> configureOptions = null)
            : base(
                  next, 
                  dataProtectionProvider, 
                  loggerFactory, 
                  encoder,
                  siteResolver,
                  multiTenantOptionsAccesor,
                  sharedOptions, 
                  options, 
                  configureOptions)
        {
            if (Options.Scope.Count == 0)
            {
                // Google OAuth 2.0 asks for non-empty scope. If user didn't set it, set default scope to 
                // "openid profile email" to get basic user information.
                // TODO: Should we just add these by default when we create the Options?
                Options.Scope.Add("openid");
                Options.Scope.Add("profile");
                Options.Scope.Add("email");
            }

            this.loggerFactory = loggerFactory;
            this.siteResolver = siteResolver;
            multiTenantOptions = multiTenantOptionsAccesor.Options;
            siteRepo = siteRepository;
        }


        private ILoggerFactory loggerFactory;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="GoogleAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<GoogleAuthenticationOptions> CreateHandler()
        {
            return new MultiTenantGoogleAuthenticationHandler(Backchannel,siteResolver, siteRepo, multiTenantOptions, loggerFactory);
        }

    }
}
