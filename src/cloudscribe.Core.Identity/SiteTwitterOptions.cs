// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2017-07-27
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace cloudscribe.Core.Identity
{
    public class SiteTwitterOptions : IOptionsSnapshot<TwitterOptions>
    {
        public SiteTwitterOptions(
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
        }

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        private IPostConfigureOptions<TwitterOptions> _optionsInitializer;
        private readonly IDataProtectionProvider _dp;

        private TwitterOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new TwitterOptions();
            options.ConsumerKey = "placeholder";
            options.ConsumerSecret = "placeholder";

            _optionsInitializer.PostConfigure(scheme, options);

            options.DataProtectionProvider = options.DataProtectionProvider ?? _dp;

            if (options.Backchannel == null)
            {
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                options.Backchannel.Timeout = options.BackchannelTimeout;
                options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
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
                                        && tenant.SiteFolderName.Length > 0;

            if (!string.IsNullOrWhiteSpace(tenant.TwitterConsumerKey))
            {
                options.ConsumerKey = tenant.TwitterConsumerKey;
                options.ConsumerSecret = tenant.TwitterConsumerSecret;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-twitter";
                }
            }
        }

        public TwitterOptions Value
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


    }
}
