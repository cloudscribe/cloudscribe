using cloudscribe.Core.Web.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCloudscribeEnforceSiteRulesMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EnforceSiteRulesMiddleware>();
        }
    }
}
