using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace cloudscribe.QueryTool.EFCore.Common
{
    public static class QueryToolStartup
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider scopedServiceProvider)
        {
            var db = scopedServiceProvider.GetService<IQueryToolDbContext>();

            if (db != null)
                await db.Database.MigrateAsync();
        }
    }
}
