using cloudscribe.Core.Storage.UserInteractiveServiceTokens;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeUserInteractiveServiceTokens(
            this IServiceCollection services
            )
        {
            services.AddScoped<IUserInteractiveServiceTokensProvider, UserInteractiveServiceTokensProvider>();

            return services;

        }
    }
}
