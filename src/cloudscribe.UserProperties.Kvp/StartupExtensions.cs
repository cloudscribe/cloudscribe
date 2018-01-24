using cloudscribe.Core.Web.ExtensionPoints;
using cloudscribe.Kvp.Models;
using cloudscribe.UserProperties.Models;
using cloudscribe.UserProperties.Services;
using cloudscribe.UserProperties.Kvp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeKvpUserProperties(this IServiceCollection services, IConfigurationRoot configuration = null)
        {
            services.TryAddScoped<IUserPropertyService, UserPropertyService>();
            services.TryAddScoped<IUserPropertyValidator, UserPropertyValidator>();
            services.TryAddScoped<IProfileOptionsResolver, TenantProfileOptionsResolver>();
            services.TryAddScoped<IKvpStorageService, KvpStorageService>();

            services.TryAddScoped<IHandleCustomRegistration, KvpRegistrationHandler>();
            services.TryAddScoped<IHandleCustomUserInfo, KvpUserInfoHandler>();
            services.TryAddScoped<IHandleCustomUserInfoAdmin, KvpUserInfoAdminHandler>();


            return services;
        }
    }
}
