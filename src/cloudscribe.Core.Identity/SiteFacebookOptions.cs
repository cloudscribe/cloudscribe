// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2017-08-30
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.Identity
{
    public class SiteFacebookOptions : IOptionsMonitor<FacebookOptions>
    {
        public SiteFacebookOptions(
            IOptionsFactory<FacebookOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<FacebookOptions>> sources,
            IOptionsMonitorCache<FacebookOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<FacebookOptions> optionsInitializer,
            IDataProtectionProvider dataProtection,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteFacebookOptions> logger
            )
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _log = logger;
            _optionsInitializer = optionsInitializer;
            _dp = dataProtection;

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

        private readonly IOptionsMonitorCache<FacebookOptions> _cache;
        private readonly IOptionsFactory<FacebookOptions> _factory;
        private readonly IEnumerable<IOptionsChangeTokenSource<FacebookOptions>> _sources;
        internal event Action<FacebookOptions, string> _onChange;

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        private IPostConfigureOptions<FacebookOptions> _optionsInitializer;
        private readonly IDataProtectionProvider _dp;

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

        private FacebookOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new FacebookOptions
            {
                AppId = "placeholder",
                AppSecret = "placeholder"
            };

            _optionsInitializer.PostConfigure(scheme, options);

            options.DataProtectionProvider = options.DataProtectionProvider ?? _dp;

            if (options.Backchannel == null)
            {
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth handler");
                options.Backchannel.Timeout = options.BackchannelTimeout;
                options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            }

            if (options.StateDataFormat == null)
            {
                var dataProtector = options.DataProtectionProvider.CreateProtector(
                    typeof(OAuthHandler<FacebookOptions>).FullName, scheme, "v1");

                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            ConfigureTenantOptions(tenant, options);

            return options;

        }

        private void ConfigureTenantOptions(SiteContext tenant, FacebookOptions options)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }
            var useFolder = !_multiTenantOptions.UseRelatedSitesMode
                                        && _multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && !string.IsNullOrWhiteSpace(tenant.SiteFolderName);

            if (!string.IsNullOrWhiteSpace(tenant.FacebookAppId))
            {
                options.AppId = tenant.FacebookAppId;
                options.AppSecret = tenant.FacebookAppSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-facebook";
                }
            }
        }

        public FacebookOptions CurrentValue
        {
            get
            {
                return ResolveOptions(FacebookDefaults.AuthenticationScheme);
            }
        }

        public FacebookOptions Get(string name)
        {
            return ResolveOptions(name);
        }

        public IDisposable OnChange(Action<FacebookOptions, string> listener)
        {
            _log.LogDebug("onchange invoked");

            var disposable = new ChangeTrackerDisposable(this, listener);
            _onChange += disposable.OnChange;
            return disposable;
        }
        

        internal class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<FacebookOptions, string> _listener;
            private readonly SiteFacebookOptions _monitor;

            public ChangeTrackerDisposable(SiteFacebookOptions monitor, Action<FacebookOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(FacebookOptions options, string name) => _listener.Invoke(options, name);

            public void Dispose() => _monitor._onChange -= OnChange;
        }

    }

    
}
