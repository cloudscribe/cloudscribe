// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2017-07-28
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

namespace cloudscribe.Core.Identity
{
    public class SiteFacebookOptions : IOptionsMonitor<FacebookOptions>
    {
        public SiteFacebookOptions(
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
        }

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        private IPostConfigureOptions<FacebookOptions> _optionsInitializer;
        private readonly IDataProtectionProvider _dp;

        private FacebookOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new FacebookOptions();

            options.AppId = "placeholder";
            options.AppSecret = "placeholder";

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
                                        && tenant.SiteFolderName.Length > 0;

            if (!string.IsNullOrWhiteSpace(tenant.FacebookAppId))
            {
                options.AppId = tenant.FacebookAppId;
                options.AppSecret = tenant.FacebookAppSecret;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-facebook";
                }
            }
        }

        public FacebookOptions CurrentValue => throw new NotImplementedException();

        public FacebookOptions Get(string name)
        {
            throw new NotImplementedException();
        }

        public IDisposable OnChange(Action<FacebookOptions, string> listener)
        {
            throw new NotImplementedException();
        }
    }

    public class SiteFacebookOptionsPreview : IOptionsSnapshot<FacebookOptions>
    {
        public SiteFacebookOptionsPreview(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<FacebookOptions> optionsInitializer,
            IDataProtectionProvider dataProtection,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteFacebookOptionsPreview> logger
            )
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _log = logger;
            _optionsInitializer = optionsInitializer;
            _dp = dataProtection;
        }

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        private IPostConfigureOptions<FacebookOptions> _optionsInitializer;
        private readonly IDataProtectionProvider _dp;

        private FacebookOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new FacebookOptions();

            options.AppId = "placeholder";
            options.AppSecret = "placeholder";

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
                                        && tenant.SiteFolderName.Length > 0;

            if (!string.IsNullOrWhiteSpace(tenant.FacebookAppId))
            {
                options.AppId = tenant.FacebookAppId;
                options.AppSecret = tenant.FacebookAppSecret;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-facebook";
                }
            }
        }

        public FacebookOptions Value
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

    }
}
