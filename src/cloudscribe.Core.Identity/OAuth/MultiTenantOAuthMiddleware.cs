//// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
//// Author:					Joe Audette
//// Created:				    2014-08-27
//// Last Modified:		    2016-02-05
//// 


//using cloudscribe.Core.Models;
//using Microsoft.AspNet.Authentication;
////using Microsoft.AspNet.Authentication.DataHandler;
//using Microsoft.AspNet.Authentication.OAuth;
//using Microsoft.AspNet.Builder;
//using Microsoft.AspNet.DataProtection;
//using Microsoft.AspNet.Http;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.OptionsModel;
//using Microsoft.Extensions.WebEncoders;
//using SaasKit.Multitenancy;
//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Net.Http;

//namespace cloudscribe.Core.Identity.OAuth
//{
//    /// <summary>
//    /// An ASP.NET middleware for authenticating users using OAuth services.
//    /// </summary>
//    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Middleware are not disposable.")]
//    public class MultiTenantOAuthMiddleware<TOptions> : AuthenticationMiddleware<TOptions> where TOptions : OAuthOptions, new()
//    {
//        //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationMiddleware.cs

//        /// <summary>
//        /// Initializes a new <see cref="OAuthAuthenticationMiddleware"/>.
//        /// </summary>
//        /// <param name="next">The next middleware in the HTTP pipeline to invoke.</param>
//        /// <param name="dataProtectionProvider"></param>
//        /// <param name="loggerFactory"></param>
//        /// <param name="options">Configuration options for the middleware.</param>
//        public MultiTenantOAuthMiddleware(
//            RequestDelegate next,
//            IDataProtectionProvider dataProtectionProvider,
//            IHttpContextAccessor contextAccessor,
//            ILoggerFactory loggerFactory,
//            IUrlEncoder encoder,
//            //ISiteResolver siteResolver,
//            ITenantResolver<SiteSettings> siteResolver,
//            IOptions<MultiTenantOptions> multiTenantOptionsAccesor,
//            IOptions<SharedAuthenticationOptions> sharedOptions,
//            TOptions options)
//            : base(next, options, loggerFactory, encoder)
//        {
            
//            //if (string.IsNullOrEmpty(Options.AuthenticationScheme))
//            //{
//            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.AuthenticationScheme)));
//            //}

//            //if (string.IsNullOrEmpty(Options.ClientId))
//            //{
//            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.ClientId)));
//            //}

//            //if (string.IsNullOrEmpty(Options.ClientSecret))
//            //{
//            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.ClientSecret)));
//            //}

//            //if (string.IsNullOrEmpty(Options.AuthorizationEndpoint))
//            //{
//            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.AuthorizationEndpoint)));
//            //}

//            //if (string.IsNullOrEmpty(Options.TokenEndpoint))
//            //{
//            //    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_OptionMustBeProvided, nameof(Options.TokenEndpoint)));
//            //}

//            if (Options.StateDataFormat == null)
//            {
//                var dataProtector = dataProtectionProvider.CreateProtector(
//                    GetType().FullName, Options.AuthenticationScheme, "v1");
//                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
//            }

//            //Backchannel = new HttpClient(ResolveHttpMessageHandler(Options));
//            Backchannel = new HttpClient(Options.BackchannelHttpHandler ?? new HttpClientHandler());
//            Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET OAuth middleware");
//            Backchannel.Timeout = Options.BackchannelTimeout;
//            Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

//            if (string.IsNullOrEmpty(Options.SignInScheme))
//            {
//                Options.SignInScheme = sharedOptions.Value.SignInScheme;
//            }
//            this.contextAccessor = contextAccessor;
//            this.loggerFactory = loggerFactory;
//            this.siteResolver = siteResolver;
//            multiTenantOptions = multiTenantOptionsAccesor.Value;
//        }

//        private ILoggerFactory loggerFactory;
//        private IHttpContextAccessor contextAccessor;
//        private ITenantResolver<SiteSettings> siteResolver;
//        private MultiTenantOptions multiTenantOptions;

//        protected HttpClient Backchannel { get; private set; }

//        /// <summary>
//        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
//        /// </summary>
//        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="OAuthAuthenticationOptions"/> supplied to the constructor.</returns>
//        protected override AuthenticationHandler<TOptions> CreateHandler()
//        {
//            return new MultiTenantOAuthHandler<TOptions>(
//                Backchannel, 
//                loggerFactory,
//                new MultiTenantOAuthOptionsResolver(contextAccessor, siteResolver, multiTenantOptions)
//                );
//        }

////        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Managed by caller")]
////        private static HttpMessageHandler ResolveHttpMessageHandler(OAuthOptions options)
////        {
////            HttpMessageHandler handler = options.BackchannelHttpHandler ??
////#if DNX451
////                new WebRequestHandler();
////            // If they provided a validator, apply it or fail.
////            if (options.BackchannelCertificateValidator != null)
////            {
////                // Set the cert validate callback
////                var webRequestHandler = handler as WebRequestHandler;
////                if (webRequestHandler == null)
////                {
////                    //throw new InvalidOperationException(Resources.Exception_ValidatorHandlerMismatch);
////                    throw new InvalidOperationException("Resources.Exception_ValidatorHandlerMismatch");
////                }
////                webRequestHandler.ServerCertificateValidationCallback = options.BackchannelCertificateValidator.Validate;
////            }
////#else
////                new WinHttpHandler();
////#endif
////            return handler;
////        }

//    }
//}
