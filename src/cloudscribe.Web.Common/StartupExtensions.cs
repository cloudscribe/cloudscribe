using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;
using NodaTime.TimeZones;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.Web.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeCommmon(this IServiceCollection services)
        {
            services.TryAddSingleton<IDateTimeZoneProvider>(new DateTimeZoneCache(TzdbDateTimeZoneSource.Default));
            services.TryAddScoped<ITimeZoneHelper, TimeZoneHelper>();

            return services;
        }
    }
}
