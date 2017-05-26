
using cloudscribe.Core.Web.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCloudscribeEnforceSiteRulesMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EnforceSiteRulesMiddleware>();
        }

        //public static IApplicationBuilder UseCommonExceptionHandler(this IApplicationBuilder builder)
        //{
        //    return builder.UseMiddleware<CommonExceptionHandlerMiddleware>();
        //}

        //public static IApplicationBuilder UseCommonExceptionHandler(this IApplicationBuilder app, ExceptionHandlerOptions options)
        //{
        //    if (app == null)
        //    {
        //        throw new ArgumentNullException(nameof(app));
        //    }
        //    if (options == null)
        //    {
        //        throw new ArgumentNullException(nameof(options));
        //    }

        //    return app.UseMiddleware<CommonExceptionHandlerMiddleware>(options);
        //}
    }
}
