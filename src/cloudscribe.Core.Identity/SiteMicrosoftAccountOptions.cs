// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2017-07-27
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace cloudscribe.Core.Identity
{
    public class SiteMicrosoftAccountOptions : IOptionsSnapshot<MicrosoftAccountOptions>
    {
        public SiteMicrosoftAccountOptions(
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
        }

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        private IPostConfigureOptions<MicrosoftAccountOptions> _optionsInitializer;
        private readonly IDataProtectionProvider _dp;

        private MicrosoftAccountOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new MicrosoftAccountOptions();
            options.ClientId = "placeholder";
            options.ClientSecret = "placeholder";

            _optionsInitializer.PostConfigure(scheme,options);

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
            if(tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }
            var useFolder = !_multiTenantOptions.UseRelatedSitesMode
                                        && _multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && tenant.SiteFolderName.Length > 0;

            if (!string.IsNullOrWhiteSpace(tenant.MicrosoftClientId))
            {
                options.ClientId = tenant.MicrosoftClientId;
                options.ClientSecret = tenant.MicrosoftClientSecret;
         
                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-microsoft";
                }
            }
        }

        public MicrosoftAccountOptions Value
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
    }
}
