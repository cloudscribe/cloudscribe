// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-31
// Last Modified:			2015-08-02
// 



using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.Cookies.Infrastructure;
using Microsoft.AspNet.Authentication.DataHandler;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.DataProtection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using Microsoft.Framework.WebEncoders;


namespace cloudscribe.Core.Identity
{
    // this implementation is based on the file:
    //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication.Cookies/CookieAuthenticationMiddleware.cs
    // as such we need to keep an eye on that file for any changes that we may need to sync here
    // any code we change from the default code we should comment out the default code so that we can still compare it to the current version of the file

    //https://github.com/aspnet/Security/blob/dev/src/Microsoft.AspNet.Authentication/AuthenticationMiddleware.cs


    public class MultiTenantCookieAuthenticationMiddleware : AuthenticationMiddleware<CookieAuthenticationOptions>
    {
        public MultiTenantCookieAuthenticationMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            IUrlEncoder urlEncoder,
            IOptions<CookieAuthenticationOptions> options,
            ConfigureOptions<CookieAuthenticationOptions> configureOptions,
            MultiTenantCookieOptionsResolverFactory tenantResolverFactory
            )
            : base(next, options, loggerFactory, urlEncoder, configureOptions)
        {
            this.dataProtectionProvider = dataProtectionProvider;
            this.tenantResolverFactory = tenantResolverFactory;
            this.loggerFactory = loggerFactory;

            if (Options.Notifications == null)
            {
                Options.Notifications = new CookieAuthenticationNotifications();
            }

            // commented out we are specifying a cookie name so this would not be hit anyway
            //if (String.IsNullOrEmpty(Options.CookieName))
            //{
            //    Options.CookieName = CookieAuthenticationDefaults.CookiePrefix + Options.AuthenticationScheme;
            //}

            // we are not actually using this TicketDataFormat
            // we are passing dataProtectionProvider into MultiTenantCookieAuthenticationHandler
            // so it can make tenant specific ticketDataFormat
            if (Options.TicketDataFormat == null)
            {
                var dataProtector = dataProtectionProvider.CreateProtector(
                    typeof(CookieAuthenticationMiddleware).FullName, Options.AuthenticationScheme, "v2");

                Options.TicketDataFormat = new TicketDataFormat(dataProtector);
            }

            if (Options.CookieManager == null)
            {
                Options.CookieManager = new ChunkingCookieManager(urlEncoder);
            }
        }

        private ILoggerFactory loggerFactory;
        private IDataProtectionProvider dataProtectionProvider;
        private MultiTenantCookieOptionsResolverFactory tenantResolverFactory = null;

        protected override AuthenticationHandler<CookieAuthenticationOptions> CreateHandler()
        {
            return new MultiTenantCookieAuthenticationHandler(dataProtectionProvider, tenantResolverFactory);
            
        }
    }
}
