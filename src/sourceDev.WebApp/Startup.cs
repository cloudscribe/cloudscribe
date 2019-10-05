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
//using cloudscribe.UserProperties.Services;
//using cloudscribe.UserProperties.Models;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using cloudscribe.Core.Models;
using Microsoft.Extensions.Hosting;

namespace sourceDev.WebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env//,
            //ILogger<Startup> logger
            )
        {
            _configuration = configuration;
            _environment = env;
           // _log = logger;
            _sslIsAvailable =  _configuration.GetValue<bool>("AppSettings:UseSsl");
            _disableIdentityServer = _configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            
        }

        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly bool _sslIsAvailable;
        private readonly bool _disableIdentityServer;
        private bool _didSetupIdServer = false;
       //private readonly ILogger _log;

        
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

            //if(!string.IsNullOrWhiteSpace(_configuration["GituHubAuthSettings:ClientId"]))
            //{
            //    services.AddAuthentication()
            //    .AddOAuth("GitHub", options =>
            //    {

            //        options.ClientId = _configuration["GituHubAuthSettings:ClientId"];
            //        options.ClientSecret = _configuration["GituHubAuthSettings:ClientSecret"];
            //        options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/signin-github");
            //        options.Scope.Add("user:email");
            //        //options.SignInScheme = "GitHub";

            //        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            //        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
            //        options.UserInformationEndpoint = "https://api.github.com/user";

            //        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            //        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            //        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            //        options.ClaimActions.MapJsonKey("urn:github:login", "login");
            //        options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
            //        options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");

            //        options.Events = new OAuthEvents
            //        {
            //            OnCreatingTicket = async context =>
            //            {
            //                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            //                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            //                var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            //                response.EnsureSuccessStatusCode();

            //                var user = JObject.Parse(await response.Content.ReadAsStringAsync());
            //                var email = user.Value<string>("email");
                            

            //                if(string.IsNullOrWhiteSpace(email))
            //                {
            //                    request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint + "/emails");
            //                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            //                    response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            //                    response.EnsureSuccessStatusCode();

            //                    var emails = JArray.Parse(await response.Content.ReadAsStringAsync());
            //                    var primaryEmail = emails.FirstOrDefault(x => x.Value<bool>("primary") == true)
            //                    .Value<string>("email");
            //                    if(!string.IsNullOrEmpty(primaryEmail))
            //                    {
            //                        user["email"] = primaryEmail;
            //                    }
            //                }
                            
                            


            //                context.RunClaimActions(user);
            //            }
            //        };
            //    });
            //}

            

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
           // IServiceProvider serviceProvider,
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
                //app.UseDatabaseErrorPage();
                
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

            app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            // this uses the policy called "default"
            app.UseCors("default");

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            //app.UseCloudscribeCore(
            //        loggerFactory,
            //        multiTenantOptions,
            //        _sslIsAvailable
            //        );

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


                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();



            });


            //app.UseMvc(routes =>
            //{
            //    var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
            //    //*** IMPORTANT ***
            //    // this is in Config/RoutingExtensions.cs
            //    // you can change or add routes there
            //    routes.UseCustomRoutes(useFolders);

            //});
            

        }
        
    }
}
