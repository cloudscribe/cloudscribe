using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using sourceDev.WebApp.Configuration;
using cloudscribe.UserProperties.Services;
using cloudscribe.UserProperties.Models;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Http;

namespace sourceDev.WebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration, 
            IHostingEnvironment env,
            ILogger<Startup> logger
            )
        {
            _configuration = configuration;
            _environment = env;
            _log = logger;
            _sslIsAvailable =  _configuration.GetValue<bool>("AppSettings:UseSsl");
            _disableIdentityServer = _configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            
        }

        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly bool _sslIsAvailable;
        private readonly bool _disableIdentityServer;
        private bool _didSetupIdServer = false;
        private readonly ILogger _log;

        
        public void ConfigureServices(IServiceCollection services)
        {
            //// **** VERY IMPORTANT *****
            // This is a custom extension method in Config/DataProtection.cs
            // These settings require your review to correctly configur data protection for your environment
            services.SetupDataProtection(_configuration, _environment);

            // waiting for RTM compatible glimpse
            //bool enableGlimpse = Configuration.GetValue("DiagnosticOptions:EnableGlimpse", false);

            //if (enableGlimpse)
            //{
            //    services.AddGlimpse();
            //}
            
            services.AddAuthorization(options =>
            {
                //https://docs.asp.net/en/latest/security/authorization/policies.html
                //** IMPORTANT ***
                //This is a custom extension method in Config/Authorization.cs
                //That is where you can review or customize or add additional authorization policies
                options.SetupAuthorizationPolicies();

            });
            
            //// **** IMPORTANT *****
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupDataStorage(_configuration);

            //*** Important ***
            // This is a custom extension method in Config/IdentityServerIntegration.cs
            // You should review this and understand what it does before deploying to production
            services.SetupIdentityServerIntegrationAndCORSPolicy(
                _configuration,
                _environment,
                _log,
                _sslIsAvailable,
                _disableIdentityServer,
                out _didSetupIdServer
                );

            //*** Important ***
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupCloudscribeFeatures(_configuration);

            //*** Important ***
            // This is a custom extension method in Config/Localization.cs
            services.SetupLocalization(_configuration);
            
            //*** Important ***
            // This is a custom extension method in Config/RoutingAndMvc.cs
            services.SetupMvc(_configuration, _sslIsAvailable);

            //*** Important ***
            // This is a custom extension method in Config/IdentityServerIntegration.cs
            var setupApiAuthentication = _configuration.GetValue<bool>("AppSettings:SetupApiAuthentication");
            if(setupApiAuthentication)
            {
                services.SetupIdentityServerApiAuthentication();
            }
            

            //services.AddSingleton<IOptions<CookiePolicyOptions>, cloudscribe.Core.Identity.SiteCookiePolicyOptions>();

            //var container = new Container();
            //container.Populate(services);

            //return container.GetInstance<IServiceProvider>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = cloudscribe.Core.Identity.SiteCookieConsent.NeedsConsent;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookie.Name = "cookieconsent_status";
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // you can add things to this method signature and they will be injected as long as they were registered during 
        // ConfigureServices
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
           // IServiceProvider serviceProvider,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {   
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                
            }
            else
            {
                app.UseExceptionHandler("/oops/Error");
                app.UseHsts();
            }
            
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = GzipMappingFileProvider.OnPrepareResponse,
                FileProvider = new GzipMappingFileProvider(
                    loggerFactory,
                    true,
                    _environment.WebRootFileProvider
                    )
            });

            app.UseCloudscribeCommonStaticFiles();

            app.UseCookiePolicy();


            // we don't need session
            //app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            // this uses the policy called "default"
            app.UseCors("default");

            

            var multiTenantOptions = multiTenantOptionsAccessor.Value;
            
            app.UseCloudscribeCore(
                    loggerFactory,
                    multiTenantOptions,
                    _sslIsAvailable
                    );

            if (!_disableIdentityServer)
            {
                app.UseIdentityServer();
            }

            app.UseMvc(routes =>
            {
                var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
                //*** IMPORTANT ***
                // this is in Config/RoutingExtensions.cs
                // you can change or add routes there
                routes.UseCustomRoutes(useFolders);

            });
            

        }
        
    }
}
