using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using cloudscribe.Core.Models;
using Microsoft.Extensions.Hosting;

namespace sourceDev.WebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env//,
            )
        {
            _configuration = configuration;
            _environment = env;
            _sslIsAvailable =  _configuration.GetValue<bool>("AppSettings:UseSsl");
            _disableIdentityServer = _configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            
        }

        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly bool _sslIsAvailable;
        private readonly bool _disableIdentityServer;
        private bool _didSetupIdServer = false;

        
        public void ConfigureServices(IServiceCollection services)
        {
            //// **** VERY IMPORTANT *****
            // This is a custom extension method in Config/DataProtection.cs
            // These settings require your review to correctly configur data protection for your environment
            services.SetupDataProtection(_configuration, _environment);
            
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
            services.SetupDataStorage(_configuration, _environment);

            //*** Important ***
            // This is a custom extension method in Config/IdentityServerIntegration.cs
            // You should review this and understand what it does before deploying to production
            services.SetupIdentityServerIntegrationAndCORSPolicy(
                _configuration,
                _environment,
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

            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddHttpClient();

            services.Configure<ContentSecurityPolicyConfiguration>(_configuration.GetSection("ContentSecurityPolicyConfiguration"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // you can add things to this method signature and they will be injected as long as they were registered during 
        // ConfigureServices
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<ContentSecurityPolicyConfiguration> cspOptionsAccessor,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            var useMiniProfiler = _configuration.GetValue<bool>("DevOptions:EnableMiniProfiler");
            if(useMiniProfiler)
            {
                //app.UseMiniProfiler();
            }

            // NWebSec https://docs.nwebsec.com/en/latest/nwebsec/getting-started.html
            var cspConfig = cspOptionsAccessor.Value;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            else
            {
                app.UseExceptionHandler("/oops/Error");
                //NWebSec
                //https://github.com/OWASP/CheatSheetSeries/blob/master/cheatsheets/HTTP_Strict_Transport_Security_Cheat_Sheet.md
                //app.UseHsts(hsts => hsts.MaxAge(cspConfig.HstsDays));
                app.UseHsts();
            }

            app.UseHttpsRedirection();

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

            app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            // this uses the policy called "default"
            app.UseCors("default");

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore();

            if (!_disableIdentityServer)
            {
                app.UseIdentityServer();
            }

            var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
            app.UseEndpoints(endpoints =>
            {
                //*** IMPORTANT ***
                // this is in Config/RoutingExtensions.cs
                // you can change or add routes there
                endpoints.UseCustomRoutes(useFolders);
            });
        }
        
    }
}
