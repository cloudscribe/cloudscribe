﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-26
// Last Modified:			2021-04-05
// 

using cloudscribe.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System;

namespace cloudscribe.Core.Identity
{
    //https://github.com/aspnet/Options/blob/dev/src/Microsoft.Extensions.Options/OptionsMonitor.cs

    public class SiteCookieAuthenticationOptions : OptionsMonitor<CookieAuthenticationOptions>
    {
        public SiteCookieAuthenticationOptions(
            ISiteAuthCookieEvents siteAuthCookieEvents,
            ICookieAuthTicketStoreProvider cookieAuthTicketStoreProvider,
            IOptionsFactory<CookieAuthenticationOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<CookieAuthenticationOptions>> sources,
            IOptionsMonitorCache<CookieAuthenticationOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            ICookieAuthRedirector cookieAuthRedirector,
            ILogger<SiteCookieAuthenticationOptions> logger
            ) : base(factory, sources, cache)
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _cookieAuthRedirector = cookieAuthRedirector;
            _cookieAuthTicketStoreProvider = cookieAuthTicketStoreProvider;
            _siteAuthCookieEvents = siteAuthCookieEvents;
            _factory = factory;
            _cache = cache;
            _log = logger;
        }

        private readonly MultiTenantOptions _multiTenantOptions;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookieAuthRedirector _cookieAuthRedirector;
        private readonly IOptionsMonitorCache<CookieAuthenticationOptions> _cache;
        private readonly IOptionsFactory<CookieAuthenticationOptions> _factory;
        private readonly ICookieAuthTicketStoreProvider _cookieAuthTicketStoreProvider;
        private readonly ISiteAuthCookieEvents _siteAuthCookieEvents;
        private readonly ILogger _log;

        public override CookieAuthenticationOptions Get(string name)
        {
            name = name ?? IdentityConstants.ApplicationScheme;

            var isAppCookie = IsApplicationCookieName(name);
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var resolvedName = ResolveName(tenant, name);
            var cookieOptions = CreateOptions(resolvedName, tenant, isAppCookie);

            return _cache.GetOrAdd(resolvedName, () => cookieOptions);
        }

        private CookieAuthenticationOptions CreateOptions(string name, SiteContext tenant, bool isAppCookie)
        {
            var options = _factory.Create(name);

            if (isAppCookie)
            {
                ConfigureApplicationCookie(tenant, options, name);

                try
                {
                    string cookieExpiry = tenant.MaximumInactivityInMinutes;
                    double cookieExpiryTime;

                    bool success = double.TryParse(cookieExpiry, out cookieExpiryTime);

                    if (success)
                    {
                        options.ExpireTimeSpan = TimeSpan.FromMinutes((int)cookieExpiryTime);
                        options.SlidingExpiration = true;

                        // this would set the default session cookie type to a fixed expiry one
                        // options.Cookie.MaxAge = options.ExpireTimeSpan;
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Error setting maximum inactivity cookie expiry");
                }
            }
            else
            {
                ConfigureOtherCookies(tenant, options, name);
            }

            return options;
        }

        private void ConfigureApplicationCookie(SiteContext tenant, CookieAuthenticationOptions options, string name)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }

            options.Cookie.Name = name;

            if (_multiTenantOptions.UseRelatedSitesMode)
            {
                options.Cookie.Path = "/";
            }
            else
            {
                options.Cookie.Path = "/" + tenant.SiteFolderName;
                options.EventsType = _siteAuthCookieEvents.GetCookieAuthenticationEventsType();
            }

            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";
            options.AccessDeniedPath = tenantPathBase + "/account/accessdenied";


            var ticketStore = _cookieAuthTicketStoreProvider.GetTicketStore();
            if(ticketStore != null)
            {
                options.SessionStore = ticketStore;
            }

            //https://github.com/IdentityServer/IdentityServer4.AspNetIdentity/blob/dev/src/IdentityServer4.AspNetIdentity/IdentityServerBuilderExtensions.cs
            // we need to disable to allow iframe for authorize requests
            // but only if secure - browsers block SameSite=None if not Secure
            var sslIsAvailable = _configuration.GetValue<bool>("AppSettings:UseSsl");
            if (sslIsAvailable) { 
                options.Cookie.SameSite = SameSiteMode.None;
            }

            options.Events.OnRedirectToAccessDenied = _cookieAuthRedirector.ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
            options.Events.OnRedirectToLogin = _cookieAuthRedirector.ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);

        }

        private void ConfigureOtherCookies(SiteContext tenant, CookieAuthenticationOptions options, string name)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }

            options.Cookie.Name = name;

            if (_multiTenantOptions.UseRelatedSitesMode)
            {
                options.Cookie.Path = "/";
            }
            else
            {
                options.Cookie.Path = "/" + tenant.SiteFolderName;
            }

        }

        private bool IsApplicationCookieName(string name)
        {
            if (name == IdentityConstants.ApplicationScheme)
            {
                return true;
            }
            return false;
        }

        private string ResolveName(SiteContext tenant, string name)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return name;
            }

            if (_multiTenantOptions.UseRelatedSitesMode)
            {
                return name;
            }

            return $"{name}-{tenant.SiteFolderName}";
        }

    }
}
