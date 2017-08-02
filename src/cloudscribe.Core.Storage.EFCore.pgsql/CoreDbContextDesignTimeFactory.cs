using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Core.Storage.EFCore.pgsql
{
    public class CoreDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        public CoreDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CoreDbContext>();
            builder.UseNpgsql("server=yourservername;UID=yourdatabaseusername;PWD=yourdatabaseuserpassword;database=yourdatabasename");
            return new CoreDbContext(builder.Options);
        }
    }
}
