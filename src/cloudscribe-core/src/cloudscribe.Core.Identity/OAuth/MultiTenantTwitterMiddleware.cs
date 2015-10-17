// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-08-29
// Last Modified:		    2015-10-17
// based on https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Twitter/TwitterMiddleware.cs

using cloudscribe.Core.Models;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Twitter;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace cloudscribe.Core.Identity.OAuth
{
    /// <summary>
    /// ASP.NET middleware for authenticating users using Twitter
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Middleware are not disposable.")]
    public class MultiTenantTwitterMiddleware : AuthenticationMiddleware<TwitterOptions>
    {
        
        private readonly HttpClient _httpClient;
        private ILoggerFactory loggerFactory;
        private ISiteResolver siteResolver;
        private ISiteRepository siteRepo;
        private MultiTenantOptions multiTenantOptions;

        /// <summary>
        /// Initializes a <see cref="TwitterAuthenticationMiddleware"/>
        /// </summary>
        /// <param name="next">The next middleware in the HTTP pipeline to invoke</param>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="encoder"></param>
        /// <param name="sharedOptions"></param>
        /// <param name="options">Configuration options for the middleware</param>
        /// <param name="configureOptions"></param>
        public MultiTenantTwitterMiddleware(
                RequestDelegate next,
                IDataProtectionProvider dataProtectionProvider,
                ILoggerFactory loggerFactory,
                ISiteResolver siteResolver,
                ISiteRepository siteRepository,
                IOptions<MultiTenantOptions> multiTenantOptionsAccesor,
                IUrlEncoder encoder,
                IOptions<SharedAuthenticationOptions> sharedOptions,
                IOptions<TwitterOptions> options)
                : base(next, options.Value, loggerFactory, encoder)
            {
                //if (string.IsNullOrEmpty(Options.ConsumerSecret))
                //{
                //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.ConsumerSecret)));
                //}
                //if (string.IsNullOrEmpty(Options.ConsumerKey))
                //{
                //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.ConsumerKey)));
                //}

                if (Options.Events == null)
                {
                    Options.Events = new TwitterEvents();
                }
                if (Options.StateDataFormat == null)
                {
                    var dataProtector = dataProtectionProvider.CreateProtector(
                        typeof(TwitterMiddleware).FullName, Options.AuthenticationScheme, "v1");
                    Options.StateDataFormat = new SecureDataFormat<RequestToken>(
                        new RequestTokenSerializer(),
                        dataProtector);
                }

                if (string.IsNullOrEmpty(Options.SignInScheme))
                {
                    Options.SignInScheme = sharedOptions.Value.SignInScheme;
                }
                if (string.IsNullOrEmpty(Options.SignInScheme))
                {
                    //throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, "SignInScheme"));
                    throw new ArgumentException("Resources.Exception_OptionMustBeProvided, SignInScheme");

                }

            _httpClient = new HttpClient(Options.BackchannelHttpHandler ?? new HttpClientHandler());
            _httpClient.Timeout = Options.BackchannelTimeout;
                _httpClient.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
                _httpClient.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Twitter middleware");
                _httpClient.DefaultRequestHeaders.ExpectContinue = false;

                this.loggerFactory = loggerFactory;
                this.siteResolver = siteResolver;
                multiTenantOptions = multiTenantOptionsAccesor.Value;
                siteRepo = siteRepository;

            }



            /// <summary>
            /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
            /// </summary>
            /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="TwitterAuthenticationOptions"/> supplied to the constructor.</returns>
            protected override AuthenticationHandler<TwitterOptions> CreateHandler()
            {
                return new MultiTenantTwitterHandler(
                    _httpClient,
                    siteResolver,
                    siteRepo,
                    multiTenantOptions,
                    loggerFactory);
            }

//            [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Managed by caller")]
//            private static HttpMessageHandler ResolveHttpMessageHandler(TwitterAuthenticationOptions options)
//            {
//                var handler = options.BackchannelHttpHandler ??
//#if DNX451
//                new WebRequestHandler();
//                // If they provided a validator, apply it or fail.
//                if (options.BackchannelCertificateValidator != null)
//                {
//                    // Set the cert validate callback
//                    var webRequestHandler = handler as WebRequestHandler;
//                    if (webRequestHandler == null)
//                    {
//                    //throw new InvalidOperationException(Resources.Exception_ValidatorHandlerMismatch);
//                    throw new InvalidOperationException("Resources.Exception_ValidatorHandlerMismatch");
//                }
//                    webRequestHandler.ServerCertificateValidationCallback = options.BackchannelCertificateValidator.Validate;
//                }
//#else
//                new WinHttpHandler();
//#endif
//                return handler;
//            }

        }
}
