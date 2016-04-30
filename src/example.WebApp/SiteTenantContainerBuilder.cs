
using System;
using cloudscribe.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.DependencyInjection;
using SaasKit.Multitenancy.StructureMap;
using StructureMap;
using System.Threading.Tasks;
using cloudscribe.Core.Identity;
using Microsoft.AspNet.Http;

namespace example.WebApp
{
    public class SiteTenantContainerBuilder : ITenantContainerBuilder<SiteSettings>
    {
        private IContainer container;

        public SiteTenantContainerBuilder(IContainer container)
        {
            this.container = container;
        }

        public Task<IContainer> BuildAsync(SiteSettings tenant)
        {
            var tenantContainer = container.CreateChildContainer();
            
            tenantContainer.Configure(config =>
            {
                config.ForSingletonOf<IConfigureOptions<IdentityOptions>>().Use(
                    new ConfigureOptions<IdentityOptions>(identityOptions =>
                {
                    // 
                    identityOptions.Cookies.ExternalCookie.CookieName = AuthenticationScheme.External + "-" + tenant.SiteFolderName;
                    identityOptions.Cookies.ExternalCookie.LoginPath = new PathString("/" + tenant.SiteFolderName + "/account/login");
                    identityOptions.Cookies.ExternalCookie.LogoutPath = new PathString("/" + tenant.SiteFolderName + "/account/logoff");
                    identityOptions.Cookies.ExternalCookie.AccessDeniedPath = new PathString("/" + tenant.SiteFolderName + "/forbidden");
                    //identityOptions.Cookies.ExternalCookie.AutomaticChallenge = true;
                    //identityOptions.Cookies.ExternalCookie.AutomaticAuthenticate = true;

                    identityOptions.Cookies.TwoFactorRememberMeCookie.CookieName = AuthenticationScheme.TwoFactorRememberMe + "-" + tenant.SiteFolderName;
                    identityOptions.Cookies.TwoFactorRememberMeCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorRememberMe + "-" + tenant.SiteFolderName;

                    identityOptions.Cookies.TwoFactorUserIdCookie.CookieName = AuthenticationScheme.TwoFactorUserId + "-" + tenant.SiteFolderName;
                    identityOptions.Cookies.TwoFactorUserIdCookie.AuthenticationScheme = AuthenticationScheme.TwoFactorUserId + "-" + tenant.SiteFolderName;

                    identityOptions.Cookies.ApplicationCookie.CookieName = AuthenticationScheme.Application + "-" + tenant.SiteFolderName;
                    identityOptions.Cookies.ApplicationCookie.AuthenticationScheme = AuthenticationScheme.Application + "-" + tenant.SiteFolderName;


                }));

        
            });

            return Task.FromResult(tenantContainer);
        }
    }
}
