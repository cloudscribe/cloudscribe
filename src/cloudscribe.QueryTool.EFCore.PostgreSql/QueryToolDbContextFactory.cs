using cloudscribe.QueryTool.EFCore.Common;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.QueryTool.EFCore.PostgreSql
{
    public class QueryToolDbContextFactory : IQueryToolDbContextFactory
    {
        public QueryToolDbContextFactory(DbContextOptions<QueryToolDbContext> options)
        {
            _options = options;
        }

        private readonly DbContextOptions<QueryToolDbContext> _options;

        public IQueryToolDbContext CreateContext()
        {
            return new QueryToolDbContext(_options);
        }
    }
}
