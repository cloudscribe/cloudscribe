// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-26
// Last Modified:			2019-04-21
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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
            IHttpContextAccessor httpContextAccessor,
            ICookieAuthRedirector cookieAuthRedirector,
            ILogger<SiteCookieAuthenticationOptions> logger
            ) : base(factory, sources, cache)
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _cookieAuthRedirector = cookieAuthRedirector;
            _cookieAuthTicketStoreProvider = cookieAuthTicketStoreProvider;
            _siteAuthCookieEvents = siteAuthCookieEvents;
            _factory = factory;
            _cache = cache;
            _log = logger;

        }

        private readonly MultiTenantOptions _multiTenantOptions;
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
            return _cache.GetOrAdd(resolvedName, () => CreateOptions(resolvedName, tenant, isAppCookie));
        }

        


        private CookieAuthenticationOptions CreateOptions(string name, SiteContext tenant, bool isAppCookie)
        {
            var options = _factory.Create(name);
            
            if (isAppCookie)
            {
                ConfigureApplicationCookie(tenant, options, name);
            }
            else
            {
                ConfigureOtherCookies(tenant, options, name);
            }

            return options;
        }

        //private Task HandleOnSigningIn(CookieSigningInContext context)
        //{
            
        //    return Task.CompletedTask;
        //}


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
                //options.Events.OnValidatePrincipal = SiteAuthCookieValidator.ValidatePrincipalAsync;
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

            //options.Events.OnSigningIn += HandleOnSigningIn;



            //https://github.com/IdentityServer/IdentityServer4.AspNetIdentity/blob/dev/src/IdentityServer4.AspNetIdentity/IdentityServerBuilderExtensions.cs
            // we need to disable to allow iframe for authorize requests
            options.Cookie.SameSite = SameSiteMode.None;

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
