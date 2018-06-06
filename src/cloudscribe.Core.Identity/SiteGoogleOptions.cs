// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-27
// Last Modified:			2018-06-06
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace cloudscribe.Core.Identity
{
    public class SiteGoogleOptions : OptionsMonitor<GoogleOptions>
    {
        public SiteGoogleOptions(
            IOptionsFactory<GoogleOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<GoogleOptions>> sources,
            IOptionsMonitorCache<GoogleOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteGoogleOptions> logger
            ):base(factory, sources, cache)
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _factory = factory;
            _cache = cache;
            _log = logger;

        }

        private readonly IOptionsMonitorCache<GoogleOptions> _cache;
        private readonly IOptionsFactory<GoogleOptions> _factory;
        
        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private ILogger _log;
        

        public override GoogleOptions Get(string name)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var resolvedName = ResolveName(tenant, name);
            return _cache.GetOrAdd(resolvedName, () => CreateOptions(resolvedName, tenant));
        }
        
        private GoogleOptions CreateOptions(string name, SiteContext tenant)
        {
            var options = _factory.Create(name);
            options.ClientId = "placeholder";
            options.ClientSecret = "placeholder";
            
            ConfigureTenantOptions(tenant, options);

            return options;
        }
        
        private void ConfigureTenantOptions(SiteContext tenant, GoogleOptions options)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }
            var useFolder = !_multiTenantOptions.UseRelatedSitesMode
                                        && _multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && !string.IsNullOrWhiteSpace(tenant.SiteFolderName);

            if (!string.IsNullOrWhiteSpace(tenant.GoogleClientId))
            {
                options.ClientId = tenant.GoogleClientId;
                options.ClientSecret = tenant.GoogleClientSecret;
                options.SignInScheme = IdentityConstants.ExternalScheme;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-google";
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
