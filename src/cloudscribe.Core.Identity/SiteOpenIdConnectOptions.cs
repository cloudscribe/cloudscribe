// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-28
// Last Modified:			2019-04-11
// 

using cloudscribe.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Core.Identity
{
    public class SiteOpenIdConnectOptions : OptionsMonitor<OpenIdConnectOptions>
    {
        public SiteOpenIdConnectOptions(
            ICaptureOidcTokens oidcHybridFlowHelper,
            IOptionsFactory<OpenIdConnectOptions> factory,
            IEnumerable<IOptionsChangeTokenSource<OpenIdConnectOptions>> sources,
            IOptionsMonitorCache<OpenIdConnectOptions> cache,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment environment,
            ILogger<SiteOpenIdConnectOptions> logger
            ) : base(factory, sources, cache)
        {
            _oidcHybridFlowHelper = oidcHybridFlowHelper;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
            _httpContextAccessor = httpContextAccessor;
            _factory = factory;
            _cache = cache;
            _environment = environment;
            _log = logger;
            
        }

        private readonly IOptionsMonitorCache<OpenIdConnectOptions> _cache;
        private readonly IOptionsFactory<OpenIdConnectOptions> _factory;
        private readonly MultiTenantOptions _multiTenantOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICaptureOidcTokens _oidcHybridFlowHelper;


        private readonly IHostingEnvironment _environment;
        private readonly ILogger _log;

        public override OpenIdConnectOptions Get(string name)
        {
            var tenant = _httpContextAccessor.HttpContext.GetTenant<SiteContext>();
            var resolvedName = ResolveName(tenant, name);
            return _cache.GetOrAdd(resolvedName, () => CreateOptions(resolvedName, tenant));
            
        }
        
        private OpenIdConnectOptions CreateOptions(string name, SiteContext tenant)
        {
           
            var options = _factory.Create(name);

            // will throw an error if these are not populated
            options.ClientId = "placeholder";
            options.ClientSecret = "placeholder";
            options.Authority = "https://placeholder.com";
            options.SignInScheme = IdentityConstants.ExternalScheme;


            if (_environment.IsDevelopment())
            {
                options.RequireHttpsMetadata = false;
            }
            
            ConfigureTenantOptions(tenant, options);

            if (string.IsNullOrWhiteSpace(options.SignInScheme))
            {
                options.SignInScheme = IdentityConstants.ExternalScheme;
            }

            if (string.IsNullOrEmpty(options.SignOutScheme))
            {
                options.SignOutScheme = options.SignInScheme;
            }

            if (string.IsNullOrEmpty(options.TokenValidationParameters.ValidAudience) && !string.IsNullOrEmpty(options.ClientId))
            {
                options.TokenValidationParameters.ValidAudience = options.ClientId;
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

        private async Task HandleTicketRecieved(TicketReceivedContext context)
        {
            _log.LogDebug($"ticket received");

            var tokens = context.Properties.GetTokens().ToList();
            if(tokens.Count > 0)
            {
                await _oidcHybridFlowHelper.CaptureOidcTokens(context.Principal, tokens);
            }

            //return Task.CompletedTask;
        }

        //private async Task HandleTokenResponseRecieved(TokenResponseReceivedContext context)
        //{
        //    _log.LogDebug($"token response received");

        //    var token = context.TokenEndpointResponse.AccessToken;

        //    var tokens = context.Properties.GetTokens();

        //    if (!string.IsNullOrWhiteSpace(token))
        //    {
        //        await _oidcHybridFlowHelper.CaptureJwt(context.Principal, token);
        //    }

        //    //return Task.CompletedTask;
        //}

        //private Task HandleUserInformationResponseRecieved(UserInformationReceivedContext context)
        //{
        //    _log.LogWarning($"user info received");

        //    var tokens = context.Properties.GetTokens();


        //    return Task.CompletedTask;
        //}

        //private Task HandleAuthorizationCodeRecieved(AuthorizationCodeReceivedContext context)
        //{
        //    _log.LogWarning($"authorization code received {context.JwtSecurityToken}");

        //    //if (!string.IsNullOrWhiteSpace(context.JwtSecurityToken.RawPayload))
        //    //{
        //    //    await _oidcHybridFlowHelper.CaptureJwt(context.Principal, context.JwtSecurityToken.RawPayload);
        //    //}

        //    var tokens = context.Properties.GetTokens();


        //    return Task.CompletedTask;

        //}



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
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                if(!string.IsNullOrWhiteSpace(tenant.OidConnectScopesCsv))
                {
                    string[] scopes = tenant.OidConnectScopesCsv.Split(',').Select(x => x.Trim()).ToArray();
                    foreach(var s in scopes)
                    {
                        options.Scope.Add(s);
                    }
                }

                //options.Scope.Add("openid");
                //options.Scope.Add("profile");
                //options.Scope.Add("idserverapi");
               
                //options.Events.OnTokenResponseReceived += HandleTokenResponseRecieved;
                //options.Events.OnAuthorizationCodeReceived += HandleAuthorizationCodeRecieved;
                //options.Events.OnUserInformationReceived += HandleUserInformationResponseRecieved;
                options.Events.OnTicketReceived += HandleTicketRecieved;

                if (useFolder)
                {
                    options.CallbackPath = "/" + tenant.SiteFolderName + "/signin-oidc";
                    options.SignedOutCallbackPath = "/" + tenant.SiteFolderName + "/signout-callback-oidc";
                    options.RemoteSignOutPath = "/" + tenant.SiteFolderName + "/signout-oidc";
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

            return $"{name}-{tenant.Id}";
        }

    }
 
}
