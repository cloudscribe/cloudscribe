using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.Core.Storage.EFCore.MySql
{
    public class CoreDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CoreDbContext>
    {
        public CoreDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CoreDbContext>();

            var connString = "Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;";

            builder.UseMySql(connString, ServerVersion.AutoDetect(connString));


            return new CoreDbContext(builder.Options);
        }
    }
}
