using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Core.Identity
{
    public class SiteCookieAuthenticationOptionsPreview : IOptionsSnapshot<CookieAuthenticationOptions>
    {
        public SiteCookieAuthenticationOptionsPreview(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<CookieAuthenticationOptions> cookieOptionsInitializer,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteCookieAuthenticationOptions> logger
            )
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _cookieOptionsInitializer = cookieOptionsInitializer;
            _httpContextAccessor = httpContextAccessor;
            _log = logger;
        }

        private MultiTenantOptions _multiTenantOptions;
        private IPostConfigureOptions<CookieAuthenticationOptions> _cookieOptionsInitializer;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;

        private CookieAuthenticationOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            
            var options = new CookieAuthenticationOptions();
            _cookieOptionsInitializer.PostConfigure(scheme, options);
           
            AdjustOptionsForTenant(tenant, options, scheme);
            return options;

        }

        private void AdjustOptionsForTenant(SiteContext tenant, CookieAuthenticationOptions options, string scheme)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }

            if (_multiTenantOptions.UseRelatedSitesMode)
            {
                //options.AuthenticationScheme = scheme;
                options.CookieName = scheme;
                options.CookiePath = "/";
            }
            else
            {

                //options.AuthenticationScheme = scheme;
                options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
                options.CookiePath = "/" + tenant.SiteFolderName;
                options.Events.OnValidatePrincipal = SiteAuthCookieValidator.ValidatePrincipalAsync;
                //cookieEvents.OnValidatePrincipal = SiteAuthCookieValidator.ValidatePrincipalAsync;
            }

            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";
            options.AccessDeniedPath = tenantPathBase + "/account/accessdenied";

        }

        public CookieAuthenticationOptions Value
        {
            get
            {
                return ResolveOptions(IdentityConstants.ApplicationScheme);
            }
        }

        public CookieAuthenticationOptions Get(string name)
        {
            _log.LogInformation($"CookieAuthenticationOptions requested for {name}");

            return ResolveOptions(name);
        }
    }

    // in 2.0 this will be consumed by CookieAuthHandler but in preview 2 IOptionsSnapshot is used

    public class SiteCookieAuthenticationOptions : IOptionsMonitor<CookieAuthenticationOptions>
    {
        public SiteCookieAuthenticationOptions(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<CookieAuthenticationOptions> cookieOptionsInitializer,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteCookieAuthenticationOptions> logger
            )
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _cookieOptionsInitializer = cookieOptionsInitializer;
            _httpContextAccessor = httpContextAccessor;
            _log = logger;
        }

        private MultiTenantOptions _multiTenantOptions;
        private IPostConfigureOptions<CookieAuthenticationOptions> _cookieOptionsInitializer;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        internal event Action<CookieAuthenticationOptions> _onChange;

        private CookieAuthenticationOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new CookieAuthenticationOptions();
            _cookieOptionsInitializer.PostConfigure(scheme, options);
            AdjustOptionsForTenant(tenant, options, scheme);
            return options;
            
        }

        private void AdjustOptionsForTenant(SiteContext tenant, CookieAuthenticationOptions options, string scheme)
        {
            if(tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }

            if (_multiTenantOptions.UseRelatedSitesMode)
            {
                //options.AuthenticationScheme = scheme;
                options.CookieName = scheme;
                options.CookiePath = "/";
            }
            else
            {

                //options.AuthenticationScheme = scheme;
                options.CookieName = $"{scheme}-{tenant.SiteFolderName}";
                options.CookiePath = "/" + tenant.SiteFolderName;
                options.Events.OnValidatePrincipal = SiteAuthCookieValidator.ValidatePrincipalAsync;
                //cookieEvents.OnValidatePrincipal = SiteAuthCookieValidator.ValidatePrincipalAsync;
            }

            var tenantPathBase = string.IsNullOrEmpty(tenant.SiteFolderName)
                ? PathString.Empty
                : new PathString("/" + tenant.SiteFolderName);

            options.LoginPath = tenantPathBase + "/account/login";
            options.LogoutPath = tenantPathBase + "/account/logoff";
            options.AccessDeniedPath = tenantPathBase + "/account/accessdenied";

        }

        public CookieAuthenticationOptions CurrentValue
        {
            get
            {
                return ResolveOptions(IdentityConstants.ApplicationScheme);
            }
        }
        
        public IDisposable OnChange(Action<CookieAuthenticationOptions> listener)
        {
            _log.LogInformation("onchange invoked");

            var disposable = new ChangeTrackerDisposable(this, listener);
            _onChange += disposable.OnChange;
            return disposable;
        }

        internal class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<CookieAuthenticationOptions> _listener;
            private readonly SiteCookieAuthenticationOptions _monitor;

            public ChangeTrackerDisposable(SiteCookieAuthenticationOptions monitor, Action<CookieAuthenticationOptions> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(CookieAuthenticationOptions options) => _listener.Invoke(options);

            public void Dispose() => _monitor._onChange -= OnChange;
        }
    }
}
