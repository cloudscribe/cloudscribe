using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Core.Storage.EFCore.SQLite
{
    public class CoreDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        public CoreDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CoreDbContext>();
            builder.UseSqlite("Data Source=cloudscribe.db");
            return new CoreDbContext(builder.Options);
        }
    }
}
