// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-28
// Last Modified:			2017-07-28
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Text;
using System.Net.Http;

namespace cloudscribe.Core.Identity
{
    public class SiteOpenIdConnectOptions : IOptionsSnapshot<OpenIdConnectOptions>
    {
        public SiteOpenIdConnectOptions(
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IPostConfigureOptions<OpenIdConnectOptions> optionsInitializer,
            IDataProtectionProvider dataProtection,
            IHttpContextAccessor httpContextAccessor,
            ILogger<SiteOpenIdConnectOptions> logger
            )
        {
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _dp = dataProtection;
            _log = logger;
            _optionsInitializer = optionsInitializer;
        }

        private MultiTenantOptions _multiTenantOptions;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtectionProvider _dp;
        private ILogger _log;
        private IPostConfigureOptions<OpenIdConnectOptions> _optionsInitializer;

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
            var options = new OpenIdConnectOptions();

            // will throw an error if these are not populated
            options.ClientId = "placeholder";
            options.ClientSecret = "placeholder";
            options.Authority = "https://placeholder.com";

            _optionsInitializer.PostConfigure(scheme, options);

            options.DataProtectionProvider = options.DataProtectionProvider ?? _dp;

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

            ConfigureTenantOptions(tenant, options);

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
                                        && tenant.SiteFolderName.Length > 0;

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

        public OpenIdConnectOptions Value
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

    }
}
