// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-28
// Last Modified:			2018-03-07
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace cloudscribe.Core.Identity
{
    public class SiteOpenIdConnectOptions : IOptionsMonitor<OpenIdConnectOptions>
    {
        public SiteOpenIdConnectOptions(
            IOptionsFactory<OpenIdConnectOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<OpenIdConnectOptions>> sources,
            IOptionsMonitorCache<OpenIdConnectOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<OpenIdConnectOptions> optionsInitializer,
            IDataProtectionProvider dataProtection,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment environment,
            ILogger<SiteOpenIdConnectOptions> logger
            )
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _dp = dataProtection;
            _environment = environment;
            _log = logger;
            _optionsInitializer = optionsInitializer;

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

        private readonly IOptionsMonitorCache<OpenIdConnectOptions> _cache;
        private readonly IOptionsFactory<OpenIdConnectOptions> _factory;
        private readonly IEnumerable<IOptionsChangeTokenSource<OpenIdConnectOptions>> _sources;
        internal event Action<OpenIdConnectOptions, string> _onChange;

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtectionProvider _dp;
        private IHostingEnvironment _environment;
        private ILogger _log;
        private IPostConfigureOptions<OpenIdConnectOptions> _optionsInitializer;

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

        private class StringSerializer : IDataSerializer<string>
        {
            public string Deserialize(byte[] data)
            {
                return Encoding.UTF8.GetString(data);
            }

            public byte[] Serialize(string model)
            {
                return Encoding.UTF8.GetBytes(model);
            }
        }

        private OpenIdConnectOptions ResolveOptions(string scheme)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var options = new OpenIdConnectOptions
            {
                // will throw an error if these are not populated
                ClientId = "placeholder",
                ClientSecret = "placeholder",
                Authority = "https://placeholder.com",
                SignInScheme = IdentityConstants.ExternalScheme
            };

            if (_environment.IsDevelopment())
            {
                options.RequireHttpsMetadata = false;
            }

            _optionsInitializer.PostConfigure(scheme, options);

            options.DataProtectionProvider = options.DataProtectionProvider ?? _dp;

            ConfigureTenantOptions(tenant, options);

            //if(string.IsNullOrWhiteSpace(options.SignInScheme))
            //{
            //    options.SignInScheme = IdentityConstants.ExternalScheme;
            //}
            
            if (string.IsNullOrEmpty(options.SignOutScheme))
            {
                options.SignOutScheme = options.SignInScheme;
            }

            if (options.StateDataFormat == null)
            {
                var dataProtector = options.DataProtectionProvider.CreateProtector(
                    typeof(OpenIdConnectHandler).FullName, scheme, "v1");
                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }

            if (options.StringDataFormat == null)
            {
                var dataProtector = options.DataProtectionProvider.CreateProtector(
                    typeof(OpenIdConnectHandler).FullName,
                    typeof(string).FullName,
                    scheme,
                    "v1");

                options.StringDataFormat = new SecureDataFormat<string>(new StringSerializer(), dataProtector);
            }

            if (string.IsNullOrEmpty(options.TokenValidationParameters.ValidAudience) && !string.IsNullOrEmpty(options.ClientId))
            {
                options.TokenValidationParameters.ValidAudience = options.ClientId;
            }

            if (options.Backchannel == null)
            {
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OpenIdConnect handler");
                options.Backchannel.Timeout = options.BackchannelTimeout;
                options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            }

            if (options.ConfigurationManager == null)
            {
                if (options.Configuration != null)
                {
                    options.ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(options.Configuration);
                }
                else if (!(string.IsNullOrEmpty(options.MetadataAddress) && string.IsNullOrEmpty(options.Authority)))
                {
                    if (string.IsNullOrEmpty(options.MetadataAddress) && !string.IsNullOrEmpty(options.Authority))
                    {
                        options.MetadataAddress = options.Authority;
                        if (!options.MetadataAddress.EndsWith("/", StringComparison.Ordinal))
                        {
                            options.MetadataAddress += "/";
                        }

                        options.MetadataAddress += ".well-known/openid-configuration";
                    }

                    if (options.RequireHttpsMetadata && !options.MetadataAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new InvalidOperationException("The MetadataAddress or Authority must use HTTPS unless disabled for development by setting RequireHttpsMetadata=false.");
                    }

                    options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(options.MetadataAddress, new OpenIdConnectConfigurationRetriever(),
                        new HttpDocumentRetriever(options.Backchannel) { RequireHttps = options.RequireHttpsMetadata });
                }
            }



            return options;

        }

        private void ConfigureTenantOptions(SiteContext tenant, OpenIdConnectOptions options)
        {
            if (tenant == null)
            {
                _log.LogError("tenant was null");
                return;
            }
            var useFolder = !_multiTenantOptions.UseRelatedSitesMode
                                        && _multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName
                                        && !string.IsNullOrWhiteSpace(tenant.SiteFolderName);

            if (!string.IsNullOrWhiteSpace(tenant.OidConnectAppId))
            {
                options.Authority = tenant.OidConnectAuthority;
                options.ClientId = tenant.OidConnectAppId;
                options.ClientSecret = tenant.OidConnectAppSecret;

                options.GetClaimsFromUserInfoEndpoint = true;
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.SaveTokens = true;


                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-oidc";
                    options.SignedOutCallbackPath = "/" + tenant.SiteFolderName + "/signout-callback-oidc";
                    options.RemoteSignOutPath = "/" + tenant.SiteFolderName + "/signout-oidc";
                }
            }
        }

        public OpenIdConnectOptions CurrentValue
        {
            get
            {
                return ResolveOptions(OpenIdConnectDefaults.AuthenticationScheme);
            }
        }

        public OpenIdConnectOptions Get(string name)
        {
            return ResolveOptions(name);
        }

        public IDisposable OnChange(Action<OpenIdConnectOptions, string> listener)
        {
            _log.LogDebug("onchange invoked");

            var disposable = new ChangeTrackerDisposable(this, listener);
            _onChange += disposable.OnChange;
            return disposable;
        }


        internal class ChangeTrackerDisposable : IDisposable
        {
            private readonly Action<OpenIdConnectOptions, string> _listener;
            private readonly SiteOpenIdConnectOptions _monitor;

            public ChangeTrackerDisposable(SiteOpenIdConnectOptions monitor, Action<OpenIdConnectOptions, string> listener)
            {
                _listener = listener;
                _monitor = monitor;
            }

            public void OnChange(OpenIdConnectOptions options, string name) => _listener.Invoke(options, name);

            public void Dispose() => _monitor._onChange -= OnChange;
        }

    }

    
}
