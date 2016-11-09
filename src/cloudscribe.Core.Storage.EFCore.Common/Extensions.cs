using Microsoft.EntityFrameworkCore;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public static class Extensions
    {

        public static void RemoveAll<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

    }
}
