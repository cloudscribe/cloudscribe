

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.Scaffolding.Metadata;

namespace cloudscribe.Logging.EF
{
    public static class Extensions
    {

        public static void RemoveAll<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }

    }
}
