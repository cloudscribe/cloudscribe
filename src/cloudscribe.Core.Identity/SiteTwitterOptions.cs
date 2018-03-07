// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2018-03-07
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Twitter;
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
   
    public class SiteTwitterOptions : IOptionsMonitor<TwitterOptions>
    {
        public SiteTwitterOptions(
            IOptionsFactory<TwitterOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<TwitterOptions>> sources,
            IOptionsMonitorCache<TwitterOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<TwitterOptions> optionsInitializer,
            IDataProtectionProvider dataProtection,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteTwitterOptions> logger
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

        private readonly IOptionsMonitorCache<TwitterOptions> _cache;
        private readonly IOptionsFactory<TwitterOptions> _factory;
        private readonly IEnumerable<IOptionsChangeTokenSource<TwitterOptions>> _sources;
        internal event Action<TwitterOptions, string> _onChange;

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        private IPostConfigureOptions<TwitterOptions> _optionsInitializer;
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

        private TwitterOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new TwitterOptions
            {
                ConsumerKey = "placeholder",
                ConsumerSecret = "placeholder"
            };

            _optionsInitializer.PostConfigure(scheme, options);

            options.DataProtectionProvider = options.DataProtectionProvider ?? _dp;

            if (options.Backchannel == null)
            {
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler())
                {
                    Timeout = options.BackchannelTimeout,
                    MaxResponseContentBufferSize = 1024 * 1024 * 10 // 10 MB
                };
                options.Backchannel.DefaultRequestHeaders.Accept.ParseAdd("*/*");
                options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core Twitter handler");
                options.Backchannel.DefaultRequestHeaders.ExpectContinue = false;
            }

            if (options.StateDataFormat == null)
            {
                var dataProtector = options.DataProtectionProvider.CreateProtector(
                    typeof(RemoteAuthenticationHandler<TwitterOptions>).FullName, scheme, "v1");

                options.StateDataFormat = new SecureDataFormat<RequestToken>(
                    new RequestTokenSerializer(),
                    dataProtector);
            }

            ConfigureTenantOptions(tenant, options);

            return options;

        }

        private void ConfigureTenantOptions(SiteContext tenant, TwitterOptions options)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }
            var useFolder = !_multiTenantOptions.UseRelatedSitesMode
                                        && _multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && !string.IsNullOrWhiteSpace(tenant.SiteFolderName);

            if (!string.IsNullOrWhiteSpace(tenant.TwitterConsumerKey))
            {
                options.ConsumerKey = tenant.TwitterConsumerKey;
                options.ConsumerSecret = tenant.TwitterConsumerSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-twitter";
                }
            }
        }

        public TwitterOptions CurrentValue
        {
            get
            {
                return ResolveOptions(TwitterDefaults.AuthenticationScheme);
            }
        }

        public TwitterOptions Get(string name)
        {
            return ResolveOptions(name);
        }

        public IDisposable OnChange(Action<TwitterOptions, string> listener)
        {
            _log.LogDebug("onchange invoked");

            var disposable = new ChangeTrackerDisposable(this, listener);
            _onChange += disposable.OnChange;
            return disposable;
        }


        internal class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<TwitterOptions, string> _listener;
            private readonly SiteTwitterOptions _monitor;

            public ChangeTrackerDisposable(SiteTwitterOptions monitor, Action<TwitterOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(TwitterOptions options, string name) => _listener.Invoke(options, name);

            public void Dispose() => _monitor._onChange -= OnChange;
        }

    }

   
}
