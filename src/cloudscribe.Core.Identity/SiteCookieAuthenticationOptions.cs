// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-26
// Last Modified:			2018-01-23
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;

namespace cloudscribe.Core.Identity
{
    //https://github.com/aspnet/Options/blob/dev/src/Microsoft.Extensions.Options/OptionsMonitor.cs

    public class SiteCookieAuthenticationOptions : IOptionsMonitor<CookieAuthenticationOptions>
    {
        public SiteCookieAuthenticationOptions(
            IOptionsFactory<CookieAuthenticationOptions> factory, 
            IEnumerable<IOptionsChangeTokenSource<CookieAuthenticationOptions>> sources, 
            IOptionsMonitorCache<CookieAuthenticationOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<CookieAuthenticationOptions> cookieOptionsInitializer,
            IHttpContextAccessor httpContextAccessor,
            ICookieAuthRedirector cookieAuthRedirector,
            ILogger<SiteCookieAuthenticationOptions> logger
            )
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _cookieOptionsInitializer = cookieOptionsInitializer;
            _httpContextAccessor = httpContextAccessor;
            _cookieAuthRedirector = cookieAuthRedirector;
            _log = logger;

            _factory = factory;
            _sources = sources;
            _cache = cache;

            foreach (var source in _sources)
            {
                ChangeToken.OnChange<string>(
                    () => source.GetChangeToken(),
                    (name) => InvokeChanged(name),
                    source.Name);
            }
        }

        private MultiTenantOptions _multiTenantOptions;
        private IPostConfigureOptions<CookieAuthenticationOptions> _cookieOptionsInitializer;
        private IHttpContextAccessor _httpContextAccessor;
        private ICookieAuthRedirector _cookieAuthRedirector;
        private ILogger _log;

        private readonly IOptionsMonitorCache<CookieAuthenticationOptions> _cache;
        private readonly IOptionsFactory<CookieAuthenticationOptions> _factory;
        private readonly IEnumerable<IOptionsChangeTokenSource<CookieAuthenticationOptions>> _sources;
        internal event Action<CookieAuthenticationOptions, string> _onChange;


        private CookieAuthenticationOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();

            var options = new CookieAuthenticationOptions();
            _cookieOptionsInitializer.PostConfigure(scheme, options);

            if (scheme == IdentityConstants.ApplicationScheme)
            {
                ConfigureApplicationCookie(tenant, options, scheme);
            }
            else
            {
                ConfigureOtherCookies(tenant, options, scheme);
            }

            return options;

        }

        private void ConfigureApplicationCookie(SiteContext tenant, CookieAuthenticationOptions options, string scheme)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }

            if (_multiTenantOptions.UseRelatedSitesMode)
            {
                options.Cookie.Name = scheme;
                options.Cookie.Path = "/";
            }
            else
            {
                options.Cookie.Name = $"{scheme}-{tenant.SiteFolderName}";
                options.Cookie.Path = "/" + tenant.SiteFolderName;
                options.Events.OnValidatePrincipal = SiteAuthCookieValidator.ValidatePrincipalAsync;
                
            }

            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";
            options.AccessDeniedPath = tenantPathBase + "/account/accessdenied";
            
            //https://github.com/IdentityServer/IdentityServer4.AspNetIdentity/blob/dev/src/IdentityServer4.AspNetIdentity/IdentityServerBuilderExtensions.cs
            // we need to disable to allow iframe for authorize requests
            options.Cookie.SameSite = SameSiteMode.None;

            options.Events.OnRedirectToAccessDenied = _cookieAuthRedirector.ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
            options.Events.OnRedirectToLogin = _cookieAuthRedirector.ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);
       

        }

        private void ConfigureOtherCookies(SiteContext tenant, CookieAuthenticationOptions options, string scheme)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }

            if (_multiTenantOptions.UseRelatedSitesMode)
            {
                options.Cookie.Name = scheme;
                options.Cookie.Path = "/";
            }
            else
            {
                options.Cookie.Name = $"{scheme}-{tenant.SiteFolderName}";
                options.Cookie.Path = "/" + tenant.SiteFolderName;
            }

        }

        public CookieAuthenticationOptions CurrentValue
        {
            get
            {
                return ResolveOptions(IdentityConstants.ApplicationScheme);
            }
        }

        public CookieAuthenticationOptions Get(string name)
        {
            return ResolveOptions(name);      
        }

        private void InvokeChanged(string name)
        {
            name = name ?? Options.DefaultName;
            _cache.TryRemove(name);
            var options = Get(name);
            if (_onChange != null)
            {
                _onChange.Invoke(options, name);
            }
        }

        public IDisposable OnChange(Action<CookieAuthenticationOptions, string> listener)
        {
            _log.LogDebug("onchange invoked");

            var disposable = new ChangeTrackerDisposable(this, listener);
            _onChange += disposable.OnChange;
            return disposable;
        }

       
        internal class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<CookieAuthenticationOptions, string> _listener;
            private readonly SiteCookieAuthenticationOptions _monitor;

            public ChangeTrackerDisposable(SiteCookieAuthenticationOptions monitor, Action<CookieAuthenticationOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(CookieAuthenticationOptions options, string name) => _listener.Invoke(options, name);

            public void Dispose() => _monitor._onChange -= OnChange;
        }
    }

}
