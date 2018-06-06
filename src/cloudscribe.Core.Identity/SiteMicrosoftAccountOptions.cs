// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2018-06-06
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace cloudscribe.Core.Identity
{
    public class SiteMicrosoftAccountOptions : OptionsMonitor<MicrosoftAccountOptions>
    {
        public SiteMicrosoftAccountOptions(
            IOptionsFactory<MicrosoftAccountOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<MicrosoftAccountOptions>> sources,
            IOptionsMonitorCache<MicrosoftAccountOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteMicrosoftAccountOptions> logger
            ) : base(factory, sources, cache)
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _factory = factory;
            _cache = cache;
            _log = logger;
           
        }

        private readonly IOptionsMonitorCache<MicrosoftAccountOptions> _cache;
        private readonly IOptionsFactory<MicrosoftAccountOptions> _factory;
        private readonly MultiTenantOptions _multiTenantOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _log;

        public override MicrosoftAccountOptions Get(string name)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var resolvedName = ResolveName(tenant, name);
            return _cache.GetOrAdd(resolvedName, () => CreateOptions(resolvedName, tenant));
        }
        
        private MicrosoftAccountOptions CreateOptions(string name, SiteContext tenant)
        {
            var options = _factory.Create(name);
            options.ClientId = "placeholder";
            options.ClientSecret = "placeholder";
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
