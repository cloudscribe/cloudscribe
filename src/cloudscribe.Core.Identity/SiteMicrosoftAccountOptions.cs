// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2017-08-30
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace cloudscribe.Core.Identity
{
    public class SiteMicrosoftAccountOptions : IOptionsMonitor<MicrosoftAccountOptions>
    {
        public SiteMicrosoftAccountOptions(
            IOptionsFactory<MicrosoftAccountOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<MicrosoftAccountOptions>> sources,
            IOptionsMonitorCache<MicrosoftAccountOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<MicrosoftAccountOptions> optionsInitializer,
            IDataProtectionProvider dataProtection,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteMicrosoftAccountOptions> logger
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

        private readonly IOptionsMonitorCache<MicrosoftAccountOptions> _cache;
        private readonly IOptionsFactory<MicrosoftAccountOptions> _factory;
        private readonly IEnumerable<IOptionsChangeTokenSource<MicrosoftAccountOptions>> _sources;
        internal event Action<MicrosoftAccountOptions, string> _onChange;

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        private IPostConfigureOptions<MicrosoftAccountOptions> _optionsInitializer;
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

        private MicrosoftAccountOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new MicrosoftAccountOptions
            {
                ClientId = "placeholder",
                ClientSecret = "placeholder"
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
                    typeof(OAuthHandler<MicrosoftAccountOptions>).FullName, scheme, "v1");

                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            ConfigureTenantOptions(tenant, options);

            return options;

        }

        private void ConfigureTenantOptions(SiteContext tenant, MicrosoftAccountOptions options)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }
            var useFolder = !_multiTenantOptions.UseRelatedSitesMode
                                        && _multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && !string.IsNullOrWhiteSpace(tenant.SiteFolderName);

            if (!string.IsNullOrWhiteSpace(tenant.MicrosoftClientId))
            {
                options.ClientId = tenant.MicrosoftClientId;
                options.ClientSecret = tenant.MicrosoftClientSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-microsoft";
                }
            }
        }

        public MicrosoftAccountOptions CurrentValue
        {
            get
            {
                return ResolveOptions(MicrosoftAccountDefaults.AuthenticationScheme);
            }
        }

        public MicrosoftAccountOptions Get(string name)
        {
            return ResolveOptions(name);
        }

        public IDisposable OnChange(Action<MicrosoftAccountOptions, string> listener)
        {
            _log.LogDebug("onchange invoked");

            var disposable = new ChangeTrackerDisposable(this, listener);
            _onChange += disposable.OnChange;
            return disposable;
        }


        internal class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<MicrosoftAccountOptions, string> _listener;
            private readonly SiteMicrosoftAccountOptions _monitor;

            public ChangeTrackerDisposable(SiteMicrosoftAccountOptions monitor, Action<MicrosoftAccountOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(MicrosoftAccountOptions options, string name) => _listener.Invoke(options, name);

            public void Dispose() => _monitor._onChange -= OnChange;
        }

    }

}
